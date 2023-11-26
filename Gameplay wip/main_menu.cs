using Godot;
using System;

public class main_menu : Control
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

	private void play_button_pressed()
	{
		GD.Print("Play button pressed");
		GetTree().ChangeScene("res://game.tscn");
	}
	
	private void settings_button_pressed()
{
	GD.Print("Settings button pressed");
	GetTree().ChangeScene("res://settings.tscn");
}


}





