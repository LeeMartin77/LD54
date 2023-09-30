using Godot;
using System;

public class Player : RigidBody2D
{
  private Camera2D _camera;

	[Export]
	public float SpeedLimit = 250f;
  [Export]
  public float Thrust = 2f;


  [Export]
  public float RotationSpeed = 2f;

	private RichTextLabel _speedLabel;
  private RichTextLabel _rotationLabel;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
	{

		_camera = GetNode<Camera2D>("Camera2D");

		_speedLabel = GetNode<RichTextLabel>("UI/CurrentSpeed");
		_rotationLabel = GetNode<RichTextLabel>("UI/CurrentRotation");
  }


  public override void _PhysicsProcess(float delta)
  {
		base._Process(delta);

		if (Input.IsActionPressed("up"))
		{
			// We only ever go "up"
			// LinearVelocity is world tied
			// this is dumb maths unfuck it
			float currentVelocity = Math.Abs(LinearVelocity.x) + Math.Abs(LinearVelocity.y);
			currentVelocity = Math.Max(SpeedLimit, currentVelocity + (delta * Thrust));
			LinearVelocity = new Vector2
			{
				x = 0,
				y = -currentVelocity
			}.Rotated(Rotation);
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
		base._Process(delta);
		_speedLabel.Text = $"{Math.Abs(LinearVelocity.x) + Math.Abs(LinearVelocity.y)} Speed";
	_rotationLabel.Text = $"{RotationDegrees} Rotation";
  }
}