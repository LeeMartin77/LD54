using Godot;
using System;

public class ClosestPoint : Node2D
{
  private Player _player;
  private Line2D _filament;
  private CPUParticles2D _particles;

  int _maxParticles = 25;

  public override void _Ready()
  {
    _player = GetNode<Player>("/root/World/Player");
    _filament = GetNode<Line2D>("/root/World/Filament");
    _particles = GetNode<CPUParticles2D>("CPUParticles2D");
  }

  public override void _Process(float delta)
  {
    var lastPoint = _filament.Points[0];
    Vector2 closestPoint = lastPoint; // bit of a cheaty hack
    foreach (var vec in _filament.Points)
    {
      var loopPoint = Geometry.GetClosestPointToSegment2d(_player.Position, lastPoint, vec);
      lastPoint = vec;

      if (_player.Position.DistanceTo(loopPoint) < _player.Position.DistanceTo(closestPoint))
      {
        closestPoint = loopPoint;
      }
    }
    Position = closestPoint;

    _particles.Direction = Position.DirectionTo(_player.Position);
    //_particles.Position = Position;

    if (Position.DistanceTo(_player.Position) < _player.FilamentDangerStart)
    {
      //_particles.Amount = _maxParticles;
      _particles.Modulate = new Color(255, 255, 255, 1);
    } 
    else if (Position.DistanceTo(_player.Position) < _player.FilamentRideStart)
    {
      //var safeArea = Position.DistanceTo(_player.Position) - _player.FilamentDangerStart;
      //var totalArea = _player.FilamentRideStart - _player.FilamentDangerStart;
      //var rawPrtcls = _maxParticles - (_maxParticles * (safeArea / totalArea));
      //var particles = Math.Max((int)Math.Round(rawPrtcls), 1);
      //GD.Print(particles);
      //_particles.Amount = particles;
      var safeArea = Position.DistanceTo(_player.Position) - _player.FilamentDangerStart;
      var totalArea = _player.FilamentRideStart - _player.FilamentDangerStart;
      _particles.Modulate = new Color(255, 255, 255, 1 - (safeArea / totalArea));
    }
    else
    {
      _particles.Modulate = new Color(255, 255, 255, 0);
      //_particles.Amount = 1;
    }
  }
}
