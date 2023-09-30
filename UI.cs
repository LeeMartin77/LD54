using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class UI : Control
{
  private RichTextLabel _prx;
  private RichTextLabel _stat;
  private RichTextLabel _spd;
  private RichTextLabel _frc;
  private RichTextLabel _alert;

  private TextureRect[] _friction;
  private TextureRect[] _proximity;

  public override void _Ready()
	{
    _prx = GetNode<RichTextLabel>("PRX");
    _stat = GetNode<RichTextLabel>("STAT");

    _spd = GetNode<RichTextLabel>("SPD");

    _frc = GetNode<RichTextLabel>("FRC");

    _alert = GetNode<RichTextLabel>("ALERT");

    List<TextureRect> frictionLights = new List<TextureRect>();
    List<TextureRect> proximityLights = new List<TextureRect>();

    foreach (var lbl in new string[] { "1", "2", "3", "4", "5"})
    {
      frictionLights.Add(GetNode<TextureRect>($"Friction/{lbl}"));
      proximityLights.Add(GetNode<TextureRect>($"Proximity/{lbl}"));
    }

    _friction = frictionLights.ToArray();
    _proximity = proximityLights.ToArray();
  }

  public Color OffColor = new Color(10, 10, 10);


  public void UpdateData(UIUpdate update)
  {
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
      _stat.Modulate = OffColor;
    }
    UpdateFrictionLights(update);
    UpdateProximityLights(update);
    _spd.Text = $"{update.Speed:0000} SPD";
  }

  private void UpdateFrictionLights(UIUpdate update)
  {
    int i = 0;
    int lightsOn = (int)Math.Floor(5 * ((update.MaxFriction - update.Friction) / update.MaxFriction));
    foreach (var l in _friction)
    {
      if (i > lightsOn)
      {
        l.Modulate = OffColor;
      }
      if (i <= lightsOn && i < 4)
      {

        l.Modulate = new Color(0, 255, 0);
      }

      if (i <= lightsOn && i == 4)
      {
        l.Modulate = new Color(255, 0, 0);
      }
      i++;
    }
  }


  private void UpdateProximityLights(UIUpdate update)
  {
    int i = 0;
    var (maxTime, currentTime) = update.TimeTilDestruction;
    int lightsOn = (int)Math.Floor(5 * (currentTime / maxTime));
    foreach (var l in _proximity)
    {
      if (i > lightsOn)
      {
        l.Modulate = OffColor;
      }
      if (i <= lightsOn)
      {
        l.Modulate = new Color(255, 255, 0);
      }
      i++;
    }
  }
}
