using Godot;
using System;

public class Player : RigidBody2D
{
  private Camera2D _camera;

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

	private RichTextLabel _speedLabel;
  private RichTextLabel _rotationLabel;

  private RichTextLabel _frictionLabel;
  private RichTextLabel _proximityLabel;

  private Sprite _closestPointMarker;

	private Line2D _filament;

	private Vector2 _closestFilamentPoint;

	private bool _inEndZone = false;
	private float _endzoneTime = 0.0f;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
	{

		_camera = GetNode<Camera2D>("Camera2D");

		_speedLabel = GetNode<RichTextLabel>("UI/CurrentSpeed");
		_rotationLabel = GetNode<RichTextLabel>("UI/CurrentRotation");
		_frictionLabel = GetNode<RichTextLabel>("UI/CurrentFriction");
		_proximityLabel = GetNode<RichTextLabel>("UI/CurrentProximity");

		_filament = GetNode<Line2D>("/root/World/Filament");
		_closestPointMarker = GetNode<Sprite>("/root/World/ClosestPoint");

		Friction = DefaultFriction;
  }

	public void SetEndZone(bool inZone)
	{
		_inEndZone = inZone;
		_endzoneTime = 0.0f;
  }


  public override void _PhysicsProcess(float delta)
  {
		base._Process(delta);

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
	  }
	  else if (distance < FilamentRideStart)
	  {
			//otherwise falloff into danger zone
			var distanceToDanger = distance - FilamentDangerStart;
			// straight percentage for now but kinda want a curve
			var fraction = distanceToDanger / (FilamentRideStart - FilamentDangerStart);
			Friction = DefaultFriction * fraction;
	  }
	  else
	  {
			// Friction is default
			Friction = DefaultFriction;
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
	  _proximityLabel.Text = $"{Position.DistanceTo(_closestFilamentPoint)} Proximity";
	}
		if (_inEndZone)
		{
			_endzoneTime += delta;
		}

		if (_endzoneTime > RequiredEndzoneTime)
		{
			GD.Print("You win!");
		}
		base._Process(delta);
		_speedLabel.Text = $"{Math.Abs(LinearVelocity.x) + Math.Abs(LinearVelocity.y)} Speed";
		_rotationLabel.Text = $"{RotationDegrees} Rotation";


	_frictionLabel.Text = $"{Friction} Friction";
  }

	public void Reset(Vector2 startPoint)
	{
		_inEndZone = false;
		_endzoneTime = 0.0f;
		Position = startPoint;
		LinearVelocity = new Vector2();
		AngularVelocity = 0.0f;
		Rotation = 0;
	}
}