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
  public float RequiredEndzoneTime = 2.0f;


  [Export]
  public float RotationSpeed = 2f;

	[Export]
	public float DefaultFriction = 0.5f;


  private Sprite _closestPointMarker;

	private Line2D _filament;

	private Vector2 _closestFilamentPoint;

	private bool _inEndZone = false;
	private float _endzoneTime = 0.0f;

	private float _timeInDangerZone = 0.0f;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
	{

		_camera = GetNode<Camera>("/root/World/Camera");

		_filament = GetNode<Line2D>("/root/World/Filament");
		_closestPointMarker = GetNode<Sprite>("/root/World/ClosestPoint");

		Friction = DefaultFriction;
  }

	public void SetEndZone(bool inZone)
	{
		_inEndZone = inZone;
		_endzoneTime = 0.0f;
  }

	public float Speed {  get
		{
			return Math.Abs(LinearVelocity.x) + Math.Abs(LinearVelocity.y);

	} }

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
				return;
			}

		if (!Alive)
			{
			GD.Print(FilamentMaxDangerPeriodSeconds);
				Friction = 0;
				return;
			}


		_camera.Position = Position;
		_camera.Rotation = Rotation;

		var lastPoint = _filament.Points[0];
		
		Vector2 closestPoint = lastPoint; // bit of a cheaty hack
		foreach (var vec in _filament.Points)
		{
			var loopPoint = Geometry.GetClosestPointToSegment2d(Position, lastPoint, vec);
				lastPoint = vec;

			if (Position.DistanceTo(loopPoint) < Position.DistanceTo(closestPoint))
			{
				closestPoint = loopPoint;
			}
		}
		_closestFilamentPoint = closestPoint;


	  var distance = Position.DistanceTo(_closestFilamentPoint);

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

		if (Input.IsActionPressed("up"))
		{
			// We only ever go "up"
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
		if (_closestFilamentPoint != null)
			{
				_closestPointMarker.Position = _closestFilamentPoint;
		}
		if (_inEndZone)
		{
				_endzoneTime += delta;
		}

		_camera.UpdateUI(new UIUpdate
		{
			PlayerAlive = Alive,
			PlayerVictorious = Success,
			Speed = Speed,
			Proximity = Position.DistanceTo(_closestFilamentPoint),
			Friction = Friction,
			MaxFriction = DefaultFriction,
			TimeTilDestruction = (FilamentMaxDangerPeriodSeconds, _timeInDangerZone)
		});

		base._Process(delta);
  }
}
