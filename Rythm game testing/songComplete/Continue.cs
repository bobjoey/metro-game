using Godot;
using System;

public class Continue : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Continue_pressed(){
        //GetTree().ChangeScene("res://songSelect/songSelect.tscn");
        AnimatedSprite loadScreen = GetNode<AnimatedSprite>("../LoadScreen");
        loadScreen.Visible = true;
        loadScreen.Play("closing");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    
}
