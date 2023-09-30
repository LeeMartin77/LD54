using Godot;
using System;
using System.Collections.Generic;

public class LevelSelect : Control
{
  // Declare member variables here. Examples:
  // private int a = 2;
  // private string b = "text";
  private Button _instruction;
  private Button _main;

  private List<Button> _levels = new List<Button>();
  public override void _Ready()
  {
    _instruction = GetNode<Button>("Instruction");
    _main = GetNode<Button>("Main");
    _levels = new List<Button>();
    foreach (var i in new int[] {1, 2, 3, 4, 5, 6, 7,8, 9})
    {
      _levels.Add(GetNode<Button>($"Level{i}"));
    }
  }

  // I can't be bothered to wire up in old-godot ways so we're going to do janky things
  public override void _Process(float delta)
  {
    base._Process(delta);

    if (_instruction.Pressed)
    {
      GetTree().ChangeScene("res://Instructions.tscn");
    }

    if (_main.Pressed)
    {
      GetTree().ChangeScene("res://Main.tscn");
    }

    int i = 1;
    // Jesus this is horrific
    foreach (var button in _levels)
    {
      if (button.Pressed)
      {
        GetTree().ChangeScene($"res://levels/{i}.tscn");
      }
    }
  }


  //  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //  public override void _Process(float delta)
  //  {
  //      
  //  }
}
