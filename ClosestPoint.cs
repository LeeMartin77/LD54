using Godot;
using System;

public class ClosestPoint : Node2D
{
  private Player _player;
  private Line2D _filament;
  private AnimatedSprite _particles;

  int _maxParticles = 25;

  public override void _Ready()
  {
    _player = GetNode<Player>("/root/World/Player");
    _filament = GetNode<Line2D>("/root/World/Filament");
    _particles = GetNode<AnimatedSprite>("AnimatedSprite");
  }

  public override void _Process(float delta)
  {
    var lastPoint = _filament.Points[0];
    Vector2 closestPoint = lastPoint; // bit of a cheaty hack
    foreach (var vec in _filament.Points)
    {
      var loopPoint = Geometry.GetClosestPointToSegment2d(_player.Position, lastPoint, vec);

      if (_player.Position.DistanceTo(loopPoint) < _player.Position.DistanceTo(closestPoint))
      {
        closestPoint = loopPoint;
      }
      lastPoint = vec;
    }

    Position = closestPoint;

    Rotation = Position.AngleToPoint(_player.Position) + Mathf.Deg2Rad(90);

    if (Position.DistanceTo(_player.Position) < _player.FilamentRideStart)
    {
      _particles.Visible = true;
      var closeness = 1 - (Position.DistanceTo(_player.Position) / _player.FilamentRideStart);
      _particles.Modulate = new Color(255, 255, 255, closeness * 0.5f);
      _particles.Scale = new Vector2(12, 18 * closeness);
    }
    else
    {
      _particles.Visible = false;
    }
  }
}
