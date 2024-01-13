using Godot;
using System;

public class globalVariables : Node
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public int volume = 50; // volume set in settings

	public string song = "duck"; // should be set in song selection scene, ** MUST BE CHANGED WHEN USING REAL SONGS, DUCK SONG WONT BE IN THERE **
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public AudioStreamMP3 getSong(string songCode){
		switch(songCode){
			case "duck":
				return (AudioStreamMP3)GD.Load("res://audioTesting/testAudioFiles/The Duck Song.mp3");
			case "rasputin":
				return (AudioStreamMP3)GD.Load("res://audioTesting/testAudioFiles/Boney M. - Rasputin (Sopot Festival 1979).mp3");
			case "whisper":
				return (AudioStreamMP3)GD.Load("res://audioTesting/testAudioFiles/George Michael - Careless Whisper (Official Video).mp3");
			case "pururin":
				return (AudioStreamMP3)GD.Load("res://audioTesting/testAudioFiles/Purupuru Pururin 4K Edition.mp3");
		}
		return (AudioStreamMP3)GD.Load("res://audioTesting/testAudioFiles/The Duck Song.mp3");
	}
	
	public float getPreviewTime(string songCode){
		switch(songCode){
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
	
	public string getAuthor(string songCode){
		switch(songCode){
			case "duck":
				return "Bryant Oden";
			case "rasputin":
				return "Boney M.";
			case "whisper":
				return "George Michael";
			case "pururin":
				return "some dude"; // ** who is this author idk fill in pls **
		}
		return "author not found";
	}
	
	public string getTimeSignature(string songCode){
		switch(songCode){ // bro idk how to figure out these songs time signatures
			case "duck":
				return "4:4";
			case "rasputin":
				return "4:4";
			case "whisper":
				return "4:4";
			case "pururin":
				return "4:4";
		}
		return "time signature not found";
	}
	
	public string getSongLength(string songCode){
		switch(songCode){ 
			case "duck":
				return "3:11";
			case "rasputin":
				return "4:27";
			case "whisper":
				return "5:00";
			case "pururin":
				return "3:59";
		}
		return "time signature not found";
	}
	
	public string getSongTitle(string songCode){
		switch(songCode){ 
			case "duck":
				return "The Duck Song";
			case "rasputin":
				return "Rasputin";
			case "whisper":
				return "Careless Whisper";
			case "pururin":
				return "Purupuru Pururin";
		}
		return "name not found";
	}

}
