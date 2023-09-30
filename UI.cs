using Godot;
using System;

public class UI : Control
{
  private RichTextLabel _prx;
  private RichTextLabel _stat;
  private RichTextLabel _spd;
  private RichTextLabel _frc;
  private RichTextLabel _alert;

  public override void _Ready()
	{
    _prx = GetNode<RichTextLabel>("PRX");
    _stat = GetNode<RichTextLabel>("STAT");

    _spd = GetNode<RichTextLabel>("SPD");

    _frc = GetNode<RichTextLabel>("FRC");

    _alert = GetNode<RichTextLabel>("ALERT");
  }
	
	public void UpdateData(UIUpdate update) {
    if (update.PlayerAlive && !update.PlayerVictorious)
    {
      _alert.Hide();
    } 
    else
    {
      if (update.PlayerVictorious)
      {
        _alert.Text = "CLEAR";
      } 
      else
      {
        _alert.Text = "L.O.S";
      }
      _alert.Show();
    }

    var (maxTime, currentTime) = update.TimeTilDestruction;
    if (currentTime == 0)
    {
      _stat.Text = "SAFE";
      _stat.Modulate = new Color(255, 255, 255);
    }
    else if (currentTime < maxTime)
    {
      _stat.Text = "WARN";
      _stat.Modulate = new Color(255, 255, 0);
    }
    if (!update.PlayerAlive)
    {
      _stat.Text = "NULL";
      _stat.Modulate = new Color(10, 10, 10);
    }

    _spd.Text = $"{update.Speed:0000} SPD";
  }
}
