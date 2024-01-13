using Godot;
using System;

public class songCard : PathFollow2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	//[Export(PropertyHint.Enum,"Easy,Medium,Hard")]
	//public string difficulty = "";
	
	//public static string[] songNames = {"Lagtrain", "Sphere", "GHOST"};
	//[Export(PropertyHint.Enum, "Lagtrain - INABAKUMORI,Sphere - Creo,GHOST - Camellia")]
	//public string currentSong = "";
	public int difficulty; //0 = Easy, 1 = Medium, 2 = Hard
	public string songCode;
	public string imagePath;
	public AudioStreamMP3 songAudio;
	public float previewTime;
	public string songAuthor;
	public string songLength;
	public string songTitle;
	public songCard(int difficulty, string songCode, string imagePath)
	{
		var globalVariables = (globalVariables)GetNode("/root/globalVariables");
		
		songAudio = globalVariables.getSong(songCode);
		previewTime = globalVariables.getPreviewTime(songCode);
		songAuthor = globalVariables.getAuthor(songCode);
		songLength = globalVariables.getSongLength(songCode);
		songTitle = globalVariables.getSongTitle(songCode);
	}
	
	public override void _Ready()
	{
		//print(difficulty);
		//print(currentSong);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
 // public override void _Process(float delta)
 // {
	  
 // }
}
