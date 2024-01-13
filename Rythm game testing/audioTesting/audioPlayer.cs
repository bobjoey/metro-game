using Godot;
using System;


public class audioPlayer : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public AudioStreamPlayer musicPlayer;
	
	public Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		musicPlayer = GetNode<AudioStreamPlayer>("musicPlayer");
		timer = GetNode<Timer>("timer");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public void songChange(String songName){ // idk man just call this when u change the song thing
		var globalVariables = (globalVariables)GetNode("/root/globalVariables"); // changing the global variable to the changed song
		globalVariables.song = songName;

		musicPlayer.Stream = globalVariables.getSong(songName); // setting song into musicPlayer
		
		float startTime = globalVariables.getPreviewTime(songName); // getting the start time for the preview
		
		float volumeConversion = ((float) globalVariables.volume) * 0.01F; // setting volume by getting bus index & converting volume variable to Db
		float volume = (float)Math.Log(volumeConversion) * 8.685889f;
		int busIndex = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(busIndex, volume);
		
		musicPlayer.Play(startTime);
		
		timer.Start(10); // starts timer for audio preview to stop 
	}
	
	
	private void stopPlaying(){ // stops any song if it is played for 10 seconds
		musicPlayer.Stop();
	}
}
