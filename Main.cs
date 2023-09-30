using Godot;
using System;

public class Main : Node2D
{
  // Declare member variables here. Examples:
  // private int a = 2;
  // private string b = "text";

	// Called when the node enters the scene tree for the first time.

	private Player _player;

	private Node2D _startPoint;

	public override void _Ready()
	{
	  _startPoint = GetNode<Node2D>("StartPoint");

	  _player = GetNode<Player>("Player");
	}

	public override void _Process(float delta)
	{

	  if (Input.IsActionJustReleased("reset"))
	  {
			GetTree().ReloadCurrentScene();
	  }


    if (Input.IsActionJustReleased("escape"))
    {
      GetTree().ChangeScene("res://LevelSelect.tscn");
    }
  }
}
