using Godot;
using System;

public class Surge : Area2D
{
  private Line2D _filament;
  private int _filamentMaxIndex;
  private int _nextNode;

  [Export]
  public float MovementSpeed = 150.0f;
  public override void _Ready()
    {

    _filament = GetNode<Line2D>("/root/World/Filament");
    _nextNode = 0;
    var i = 0;
    foreach (var vec in _filament.Points)
    {
      if (Position.DistanceTo(_filament.Points[_nextNode]) > Position.DistanceTo(vec))
      {
        _nextNode = i;
      }
      i++;
    }
    _filamentMaxIndex = _filament.Points.Length - 1;
    Position = _filament.Points[_nextNode];
  }

  public override void _Process(float delta)
  {

    // Move along filament
    // Go to next node as target
    if (Position.DistanceTo(_filament.Points[_nextNode]) < 10f)
    {
      if (0 == _nextNode)
      {
        // reset
        _nextNode = _filamentMaxIndex;
        Position = _filament.Points[_nextNode];
      }
      _nextNode = _nextNode - 1;
    }

    Position = Position.MoveToward(_filament.Points[_nextNode], MovementSpeed * delta);
    base._Process(delta);
  }

  public void OnSurgeBodyEntered(PhysicsBody2D body)
    {
      if (body.Name == "Player")
      {
        ((Player)body).TimeInDangerZone = 9999;
      }
    }
}
