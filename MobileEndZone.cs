using Godot;
using System;

public class MobileEndZone : EndZone
{
    private Line2D _filament;
    private int _filamentMaxIndex;
    private int _nextNode;

    [Export]
    public float MovementSpeed = 100.0f;

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
      base._Ready();
    }

    public override void _Process(float delta)
    {

      if (_successTimer > 0)
      {
      // Move along filament
      // Go to next node as target
        if (Position.DistanceTo(_filament.Points[_nextNode]) < 10f)
        {
          if (_filamentMaxIndex == _nextNode)
          {
          // reset
            _nextNode = 0;
          }
          else
          {
            _nextNode = _nextNode + 1;
          }
        }

        Position = Position.MoveToward(_filament.Points[_nextNode], MovementSpeed * delta);
        }
        base._Process(delta);
  }
}
