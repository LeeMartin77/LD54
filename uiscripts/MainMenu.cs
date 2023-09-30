using Godot;
using System;

public class MainMenu : Control
{
  // Declare member variables here. Examples:
  // private int a = 2;
  // private string b = "text";

  // Called when the node enters the scene tree for the first time.


    private Button _instruction;
  private Button _levels;
  public override void _Ready()
    {
    _instruction = GetNode<Button>("Instruction");
    _levels = GetNode<Button>("Levels");

  }

  // I can't be bothered to wire up in old-godot ways so we're going to do janky things
  public override void _Process(float delta)
  {
    base._Process(delta);

    if (_instruction.Pressed)
    {
      GetTree().ChangeScene("res://Instructions.tscn");
    }

    if (_levels.Pressed)
    {
      GetTree().ChangeScene("res://LevelSelect.tscn");
    }
  }

  public void InstructionsPressed()
  {

  }



//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
