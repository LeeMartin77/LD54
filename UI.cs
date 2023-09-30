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
  private RichTextLabel _flight;
  private RichTextLabel _fuel;

  private Button _thrust;
  private Button _left;
  private Button _right;
  private Button _reset;


  private TextureRect[] _friction;
  private TextureRect[] _proximity;

  private UIUpdate _latestUpdate = new UIUpdate { };

  public Color OffColor = new Color(10, 10, 10, 0.2f);
  public Color LitButton = new Color(255, 255, 0);

  float flightTime = 0.0f;

  public override void _Ready()
  {
    _prx = GetNode<RichTextLabel>("PRX");
    _stat = GetNode<RichTextLabel>("STAT");

    _spd = GetNode<RichTextLabel>("SPD");

    _frc = GetNode<RichTextLabel>("FRC");

    _alert = GetNode<RichTextLabel>("ALERT");

    _flight = GetNode<RichTextLabel>("FLIGHT");
    _fuel = GetNode<RichTextLabel>("FUEL");


    List<TextureRect> frictionLights = new List<TextureRect>();
    List<TextureRect> proximityLights = new List<TextureRect>();

    _thrust = GetNode<Button>("Thrust");
    _left = GetNode<Button>("Left");
    _right = GetNode<Button>("Right");
    _reset = GetNode<Button>("RESET");

    foreach (var lbl in new string[] { "1", "2", "3", "4", "5" })
    {
      frictionLights.Add(GetNode<TextureRect>($"Friction/{lbl}"));
      proximityLights.Add(GetNode<TextureRect>($"Proximity/{lbl}"));
    }

    _friction = frictionLights.ToArray();
    _proximity = proximityLights.ToArray();
  }

  public override void _Process(float delta)
  {

    if (_latestUpdate.PlayerAlive && !_latestUpdate.PlayerVictorious)
    {
      flightTime += delta;
      _flight.Text = $"FLT {flightTime:000.0}";
    }
    UpdateButtons();
  }

  public void OnPressReset()
  {
    Input.ActionPress("reset");
  }

  public void OnReleaseReset()
  {
    Input.ActionRelease("reset");
  }

  public void OnPressLeft()
  {
    Input.ActionPress("left");
  }

  public void OnReleaseLeft()
  {
    Input.ActionRelease("left");
  }

  public void OnPressRight()
  {
    Input.ActionPress("right");
  }

  public void OnReleaseRight()
  {
    Input.ActionRelease("right");
  }

  public void OnPressUp()
  {
    Input.ActionPress("up");
  }
  public void OnReleaseUp()
  {
    Input.ActionRelease("up");
  }

  private void UpdateButtons()
  {
    if (_latestUpdate.PlayerAlive && Input.IsActionPressed("up"))
    {
      _thrust.Modulate = LitButton;
    }
    else
    {
      _thrust.Modulate = OffColor;
    }
    if (_latestUpdate.PlayerAlive && Input.IsActionPressed("left"))
    {
      _left.Modulate = LitButton;
    }
    else
    {

      _left.Modulate = OffColor;
    }
    if (_latestUpdate.PlayerAlive && Input.IsActionPressed("right"))
    {
      _right.Modulate = LitButton;
    }
    else
    {

      _right.Modulate = OffColor;
    }

    if (Input.IsActionPressed("reset"))
    {
      _reset.Modulate = LitButton;
    }
    else
    {
      _reset.Modulate = OffColor;
    }
  }

  public void UpdateData(UIUpdate update)
  {
    _latestUpdate = update;
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
    if (!update.PlayerAlive)
    {
        _stat.Text = "NULL";
        _stat.Modulate = OffColor;
    } else if (update.RemainingFuel < 5f)
    {
        _stat.Text = "FUEL";
        _stat.Modulate = new Color(255, 255, 0);
    } else if (currentTime == 0)
    {
      _stat.Text = "SAFE";
      _stat.Modulate = new Color(255, 255, 255);
    }
    else if (currentTime < maxTime)
    {
      _stat.Text = "WARN";
      _stat.Modulate = new Color(255, 255, 0);
    } 
    _fuel.Text = $"FUEL {update.RemainingFuel:00.0}";
    UpdateFrictionLights(update);
    UpdateProximityLights(update);
    _spd.Text = $"{update.Speed:0000} SPD";
  }

  private void UpdateFrictionLights(UIUpdate update)
  {
    int i = 0;
    int lightsOn = (int)Math.Floor(5 * ((update.MaxFriction - update.Friction) / update.MaxFriction));
    if (update.Friction == 0)
    {
      lightsOn = 5;
    }
    foreach (var l in _friction)
    {
      l.Modulate = OffColor;
      if (i < lightsOn && i < 4)
      {
        l.Modulate = new Color(0, 255, 0);
      }

      if (lightsOn == 5 && i == 4)
      {
        l.Modulate = new Color(255, 0, 0);
      }
      i++;
    }
  }


  private void UpdateProximityLights(UIUpdate update)
  {
    int i = 1;
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
