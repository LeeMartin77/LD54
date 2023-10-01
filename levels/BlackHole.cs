using Godot;
using System;

public class BlackHole : Area2D
{
  private Player _player;

  [Export]
  public float GravityPower = 0.001f;

  [Export]
  public float GravityDistance = 250.0f;
  public override void _Ready()
    {

    _player = GetNode<Player>("/root/World/Player");
  }

  public override void _PhysicsProcess(float delta)
  {
    var dist = Position.DistanceTo(_player.Position);
    if (Position.DistanceTo(_player.Position) < GravityDistance)
    {
      _player.LinearVelocity += _player.Position.DirectionTo(Position) * 
        Mathf.Lerp(Mathf.Pow(GravityDistance - dist, 2), 0, delta) * GravityPower;
    }
  }

  public void OnBlackHoleBodyEntered(PhysicsBody2D body)
    {
      if (body.Name == "Player")
      {
        ((Player)body).ExplodeOnDeath = false;
        ((Player)body).TimeInDangerZone = 9999;
      }
    }
}
