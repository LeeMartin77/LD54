using Godot;
using System;

public class Instructions : Control
{
  // Declare member variables here. Examples:
  // private int a = 2;
  // private string b = "text";

  private Button _levels;
  private Button _main;
  public override void _Ready()
  {
    _levels = GetNode<Button>("Levels");
    _main = GetNode<Button>("Main");

  }

  // I can't be bothered to wire up in old-godot ways so we're going to do janky things
  public override void _Process(float delta)
  {
    base._Process(delta);

    if (_levels.Pressed)
    {
      GetTree().ChangeScene("res://LevelSelect.tscn");
    }

    if (_main.Pressed)
    {
      GetTree().ChangeScene("res://Main.tscn");
    }
  }


  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
