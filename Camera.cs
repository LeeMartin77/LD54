using Godot;
using System;

public struct UIUpdate
{
  public bool PlayerAlive { get; set; }
  public bool PlayerVictorious { get; set; }
  public float Speed { get; set; }
  public float Rotation { get; set; }
  public float Proximity { get; set; }
  public float Friction { get; set; }
  public float MaxFriction { get; set; }
  public float RemainingFuel { get; set; }
  public (float, float) TimeTilDestruction { get; set; } // Max time, current time
}

public enum Difficulty
{
  Apprentice,
  Journeyman,
  Master
}

public class Camera : Camera2D
{

  [Export]
  public string LevelName = "LEVEL_NAME";
  [Export]
  public Difficulty Difficulty = Difficulty.Journeyman;

  private UI _ui;
  public override void _Ready()
	{

    _ui = GetNode<UI>("UI");

  }

  public void UpdateUI(UIUpdate update)
  {
    _ui.UpdateData(update);
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
