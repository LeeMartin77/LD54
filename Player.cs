using Godot;
using System;

public class Player : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

  public override void _PhysicsProcess(float delta)
  {
		if (Input.IsActionJustPressed("up"))
		{
			Position = new Vector2{ x = Position.x, y = Position.y - 10 };
	}
	if (Input.IsActionJustPressed("down"))
	{
	  Position = new Vector2 { x = Position.x, y = Position.y + 10 };
	}
	if (Input.IsActionJustPressed("left"))
	{
	  Position = new Vector2 { x = Position.x -10 , y = Position.y };
	}
	if (Input.IsActionJustPressed("right"))
	{
	  Position = new Vector2 { x = Position.x + 10, y = Position.y };
	}
  }
}
