using Godot;
using System;

public class settings : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AnimatedSprite _VolumeMinusMute;
	private HSlider VolumeSlider;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_VolumeMinusMute = GetNode<AnimatedSprite>("VolumeMinusMute");
		var globalVariables = (globalVariables)GetNode("/root/globalVariables");
		VolumeSlider = GetNode<HSlider>("VolumeSlider"); // setting volume slider to current volume
		VolumeSlider.Value = globalVariables.volume;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void return_button_pressed() // returns to main scene
	{
		GD.Print("Return button pressed");
		GetTree().ChangeScene("res://main_menu.tscn");
	}

	private void volume_changed(float value)
	{
		GD.Print("Volume: "+value);
		var globalVariables = (globalVariables)GetNode("/root/globalVariables"); // updating global variable of volume
		globalVariables.volume = (int) value;
		if(globalVariables.volume == 0){ // setting icon to mute if needed 
			_VolumeMinusMute.Play("mute");
		} else{
			_VolumeMinusMute.Play("minus");
		}
	}

}

