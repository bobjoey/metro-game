using Godot;
using System;
using System.Runtime.CompilerServices;

public class CloseTutorial : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_OpenTutorial_pressed(){
        this.Visible = true;
    }
    public void _on_CloseTutorial_pressed(){
        this.Visible = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
