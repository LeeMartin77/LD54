using Godot;
using System;

public class EndZone : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	
  
  public override void _Ready()
	{

	//BodyEntered += _OnBodyEntered;
	//BodyExited += _OnBodyExited;
  }

	public void OnEndZoneBodyEntered(PhysicsBody2D body)
  {
	if (body.Name == "Player")
		{
			((Player)body).SetEndZone(true);
		}
	}

  public void OnEndZoneBodyExited(PhysicsBody2D body)
  {
	if (body.Name == "Player")
	{
	  ((Player)body).SetEndZone(false);
	}
  }

}




