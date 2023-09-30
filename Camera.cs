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
  public (float, float) TimeTilDestruction { get; set; } // Max time, current time
}

public class Camera : Camera2D
{
  private RichTextLabel _speedLabel;
  private RichTextLabel _timeTilDestruction;

  private RichTextLabel _frictionLabel;
  private RichTextLabel _proximityLabel;
  public override void _Ready()
	{

    _speedLabel = GetNode<RichTextLabel>("UI/CurrentSpeed");
    _frictionLabel = GetNode<RichTextLabel>("UI/CurrentFriction");
    _proximityLabel = GetNode<RichTextLabel>("UI/CurrentProximity");
    _timeTilDestruction = GetNode<RichTextLabel>("UI/TimeTilDestruction");

  }

  public void UpdateUI(UIUpdate update)
  {
    _proximityLabel.Text = $"{update.Proximity} Proximity";
    _speedLabel.Text = $"{update.Speed} Speed";
    _frictionLabel.Text = $"{update.Friction} Friction";

    var (maxTime, currentTime) = update.TimeTilDestruction;
    if (currentTime == 0)
    {
      _timeTilDestruction.Text = "Safe";
    } 
    else if (currentTime < maxTime)
    {
      _frictionLabel.Text = $"{maxTime - currentTime:0.00} Til Deresolution";
    }
    if (!update.PlayerAlive)
    {
      _timeTilDestruction.Text = "Phase Deresolution Detected";
    }
  }

  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
