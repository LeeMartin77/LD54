using Godot;
using System;

public class EndZone : Area2D
{
  protected bool _playerInZone = false;

  protected float _successTimer = 1.5f;

  private Sprite _sprite;
  private RichTextLabel _label;

  private bool _hasDinged = false;
  private float _dingTime = 0.0f; // I'm sure there is a "proper" way of doing this but fuck it

  public override void _Ready()
  {

    //BodyEntered += _OnBodyEntered;
    //BodyExited += _OnBodyExited;

    _sprite = GetNode<Sprite>("Sprite");

    _label = GetNode<RichTextLabel>("Docking");
  }

  public void OnEndZoneBodyEntered(PhysicsBody2D body)
  {
    if (body.Name == "Player")
    {
      ((Player)body).SetEndZone(true);
      _successTimer = ((Player)body).RequiredEndzoneTime;
      _playerInZone = true;
      Scale = new Vector2(1.2f, 1.2f);
    }
  }

  public void OnEndZoneBodyExited(PhysicsBody2D body)
  {
    if (body.Name == "Player")
    {
      ((Player)body).SetEndZone(false);
      _playerInZone = false;
      Scale = new Vector2(1.0f, 1.0f);
    }
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (_playerInZone)
    {
      _successTimer = Math.Max(0, _successTimer - delta);
    }

    if (_playerInZone && _successTimer > 0)
    {
      _sprite.RotationDegrees += 90 * delta;
    } 
    else
    {
      _sprite.RotationDegrees += 45 * delta;
    }

    if (_sprite.RotationDegrees > 360)
    {
      _sprite.RotationDegrees -= 360;
    }

    if (_playerInZone && _successTimer > 0)
    {
      _label.Visible = true;
      _label.Text = $"DOCKING\n{_successTimer:0.000}";
    }
    else
    {
      _label.Visible = false;
    }

    if (_playerInZone && _successTimer <= 0 && !_hasDinged)
    {
      _hasDinged = true;
      GetNode<AudioStreamPlayer>("Success").Play();
    }
    if (_hasDinged)
    {
      _dingTime += delta;
      if (_dingTime > 1.2f)
      {
        GetNode<AudioStreamPlayer>("Success").Stop();
      }
    }
  }
}




