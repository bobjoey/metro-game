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
		
		var path = getPath(songName); // gets path of song
		
		var song = new AudioStreamMP3(); // changing the song in musicPlayer
		song = (AudioStreamMP3)GD.Load(path);
		musicPlayer.Stream = song;
		
		float startTime = getStartTime(songName); // getting the start time for the preview
		
		float volumeConversion = ((float) globalVariables.volume) * 0.01F; // setting volume by getting bus index & converting volume variable to Db
		float volume = (float)Math.Log(volumeConversion) * 8.685889f;
		int busIndex = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(busIndex, volume);
		
		musicPlayer.Play(startTime);
		
		timer.Start(10); // starts timer for audio preview to stop 
	}
	
	public float getStartTime(String song){ // returns the start time for the playback to start at ** EDIT START TIMES ONCE ACTUAL SONGS MADE **
		switch(song){
			case "duck":
				return 5;
			case "rasputin":
				return 86;
			case "whisper":
				return 1;
			case "pururin":
				return 0;
		}
		return 0;
	}
	
	public String getPath(String song){ // returns paths of files ** EDIT ONCE REAL AUDIO FILES IMPORTED **
		switch(song){
			case "duck":
				return "res://audioTesting/testAudioFiles/The Duck Song.mp3";
			case "rasputin":
				return "res://audioTesting/testAudioFiles/Boney M. - Rasputin (Sopot Festival 1979).mp3";
			case "whisper":
				return "res://audioTesting/testAudioFiles/George Michael - Careless Whisper (Official Video).mp3";
			case "pururin":
				return "res://audioTesting/testAudioFiles/Purupuru Pururin 4K Edition.mp3";
		}
		return "res://audioTesting/testAudioFiles/The Duck Song.mp3";
	}
	
	private void stopPlaying(){ // stops any song if it is played for 10 seconds
		musicPlayer.Stop();
	}
}
