using Godot;
using System;

public class Player : RigidBody2D
{
  private Camera _camera;

  [Export]
  public float SpeedLimit = 500f;
  [Export]
  public float MaxExcessSpeedLimitSeconds = 5.0f;

  [Export]
  public float Thrust = 50f;

  [Export]
  public float FilamentRideStart = 250f;
  [Export]
  public float FilamentDangerStart = 50f;
  [Export]
  public float FilamentMaxDangerPeriodSeconds = 5.0f;


  [Export]
  public float RequiredEndzoneTime = 1.5f;


  [Export]
  public float RotationSpeed = 2f;

  [Export]
  public float DefaultFriction = 0.5f;

  private Node2D _closestFilamentPoint;

  private AnimatedSprite _exhaust;
  private AnimatedSprite _left;
  private AnimatedSprite _right;

  private CPUParticles2D _explosion;
  private Sprite _ship;

  private bool _inEndZone = false;
  private float _endzoneTime = 0.0f;

  private float _timeInDangerZone = 0.0f;

    private float _remainingFuel = 99.9f;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {

    _camera = GetNode<Camera>("/root/World/Camera");
    _closestFilamentPoint = GetNode<Node2D>("/root/World/ClosestPoint");
    _explosion = GetNode<CPUParticles2D>("Explosion");

    _exhaust = GetNode<AnimatedSprite>("Exhaust");
    _ship = GetNode<Sprite>("Ship");
    _left = GetNode<AnimatedSprite>("Left");
    _right = GetNode<AnimatedSprite>("Right");

    Friction = DefaultFriction;
  }

  public void SetEndZone(bool inZone)
  {
    if (Success || !Alive)
        {
            //We just reset to go back, so if success, we don't allow to go back
            return;
        }
    _inEndZone = inZone;
    _endzoneTime = 0.0f;
  }

  public float Speed
  {
    get
    {
      return Math.Abs(LinearVelocity.x) + Math.Abs(LinearVelocity.y);

    }
  }

  public bool Alive
  {
    get
    {
      return _timeInDangerZone < FilamentMaxDangerPeriodSeconds;
    }
  }

  public bool Success
  {

    get
    {
      return _endzoneTime > RequiredEndzoneTime;
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    base._Process(delta);

    if (Success)
    {
      Friction = 1;
            LinearVelocity = new Vector2
            {
                x = Mathf.Lerp(LinearVelocity.x, 0, delta),
                y = Mathf.Lerp(LinearVelocity.y, 0, delta),
            };
      return;
    }

    if (!Alive)
    {
      Friction = 0;
      return;
    }


    _camera.Position = Position;
    //_camera.Rotation = Rotation;

    var distance = Position.DistanceTo(_closestFilamentPoint.Position);

    if (distance < FilamentDangerStart)
    {
      //// Friction 0 in danger zone 
      Friction = 0;
      _timeInDangerZone += delta;
    }
    else if (distance < FilamentRideStart)
    {
      //otherwise falloff into danger zone
      var distanceToDanger = distance - FilamentDangerStart;
      // straight percentage for now but kinda want a curve
      var fraction = distanceToDanger / (FilamentRideStart - FilamentDangerStart);
      Friction = DefaultFriction * fraction;
      _timeInDangerZone = 0;
    }
    else
    {
      // Friction is default
      Friction = DefaultFriction;
      _timeInDangerZone = 0;
    }

    if (Input.IsActionPressed("up") && _remainingFuel > 0)
    {
            // We only ever go "up"
            _remainingFuel = Math.Max(0, _remainingFuel - delta);
      Vector2 angledThrust = (new Vector2(0, -1) * (delta * Thrust)).Rotated(Rotation);
      LinearVelocity += angledThrust;
    }
    if (Input.IsActionPressed("left"))
    {
      AngularVelocity += RotationSpeed * delta;
    }
    if (Input.IsActionPressed("right"))
    {
      AngularVelocity -= RotationSpeed * delta;
    }
  }


  public override void _Process(float delta)
  {

    if (_inEndZone)
    {
      _endzoneTime += delta;
    }

    _camera.UpdateUI(new UIUpdate
    {
      PlayerAlive = Alive,
      PlayerVictorious = Success,
      Speed = Speed,
      Proximity = Position.DistanceTo(_closestFilamentPoint.Position),
      Friction = Friction,
      MaxFriction = DefaultFriction,
      RemainingFuel = _remainingFuel,
      TimeTilDestruction = (FilamentMaxDangerPeriodSeconds, _timeInDangerZone)
    });

    if (!Alive && _ship.Visible)
    {
      _ship.Visible = false;
      _explosion.Emitting = true;
    }

    if (Alive && Input.IsActionPressed("up") && _remainingFuel > 0)
    {
      _exhaust.Show();
    } 
    else
    {
      _exhaust.Hide();
    }

    if (Alive && Input.IsActionPressed("left"))
    {
      _left.Show();
    }
    else
    {
      _left.Hide();
    }

    if (Alive && Input.IsActionPressed("right"))
    {
      _right.Show();
    }
    else
    {
      _right.Hide();
    }

    base._Process(delta);
  }
}
