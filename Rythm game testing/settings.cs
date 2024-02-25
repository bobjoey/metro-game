using Godot;
using System;
using System.Collections.Generic;

public class settings : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private AnimatedSprite _VolumeMinusMute;
	private HSlider VolumeSlider;

	private int volume = 50; // does change
	private string song; // doesn't change

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_VolumeMinusMute = GetNode<AnimatedSprite>("VolumeMinusMute");
		var globalVariables = (globalVariables)GetNode("/root/globalVariables");
		// attempting to get rid of global vars
		setSettings();

		// window size stuff?
		Vector2 screenSize = OS.GetScreenSize();
		int curHeight = (int)ProjectSettings.GetSetting("display/window/size/height");
		int curWidth = (int)ProjectSettings.GetSetting("display/window/size/width");
		float scaleFac = screenSize.y/curHeight;
		curHeight = (int)(curHeight*scaleFac);
		curWidth = (int)(curWidth*scaleFac);
		
		// GD.Print("Screen dimensions = " + screenSize.x + ", " + screenSize.y);
		// GD.Print("Game dimensions = " + curWidth + ", " + curHeight);
		// GD.Print("Scale factor = " + scaleFac);
		// GD.Print("New dimensions = " + (int)(curWidth*scaleFac) + ", " + (int)(curHeight*scaleFac));
		Vector2 newSize = new Vector2((float)curWidth, (float)curHeight);
		OS.WindowSize = newSize;
		OS.WindowPosition = new Vector2((screenSize.x-curWidth)/2,0);
		//ProjectSettings.SetSetting("display/window/size/height",curHeight);
		//ProjectSettings.SetSetting("display/window/size/width",curWidth);
	//	GD.Print(ProjectSettings.GetSetting("display/window/size/height"));
	//	GD.Print(ProjectSettings.GetSetting("display/window/size/width"));
	//	GD.Print(curHeight);
	//	GD.Print(curWidth);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	private void return_button_pressed() // returns to main scene
	{
		string volumeConvert = Convert.ToString(1000 + volume);
		string volumeString = "volume: "+volumeConvert.Substring(1, 3);
		string path = "res://settings.txt";
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Write);
            file.StoreLine(volumeString);
			file.StoreLine(song);
			file.Close();
		} else {
			GD.Print("not found: " + path);
		}

		GD.Print("Return button pressed");
		GetTree().ChangeScene("res://main_menu.tscn");
	}

	private void volume_changed(float value)
	{
		GD.Print("Volume: "+value);
		// get rid of global vars thing
		var globalVariables = (globalVariables)GetNode("/root/globalVariables"); // updating global variable of volume
		globalVariables.volume = (int) value;
		// keep from here
		volume = (int) value;
		if(volume == 0){ // setting icon to mute if needed
			_VolumeMinusMute.Play("mute");
		} else{
			_VolumeMinusMute.Play("minus");
		}
	}

	private void setSettings(){
		string path = "res://settings.txt";
		GD.Print(path);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			List<String> lines = new List<String>();
			while(!file.EofReached()){
				lines.Add(file.GetLine());
			}
			file.Close();
			VolumeSlider = GetNode<HSlider>("VolumeSlider"); // setting volume slider to current volume
			VolumeSlider.Value = Convert.ToInt32(lines[0].Substring(8,3));
			song = lines[1];
			// space to put others settings once they're done
		} else {
			GD.Print("not found: " + path);
		}
	}

}

