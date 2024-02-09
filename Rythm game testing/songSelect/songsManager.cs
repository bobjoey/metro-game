using Godot;
using System;
using System.ComponentModel.Design;
using System.Data;

public class songsManager : Path2D
{
	
	//const songCard = import("res://path/to/word_printer.gd")
	//extends Path2D
	//var length = 150
	//public songBanner songBanner = GD.Load("res://songSelect/songBanner.cs");
	[Signal]
	public delegate void update_bannerEventHandler(songCard newSongCard);	
	[Signal]
	public delegate void songChangeEventHandler(songCard newSong);	
	public Vector2 startPos;
	public Vector2 curPos;
	public int selected = 1; //Selected starts at 1 ( center/selected songCard is songsList[1] )
//	public int nextSong = 3; //SongsList array index
	public static songCard[] songsList = {new songCard("Lagtrain"), new songCard("ZenZenZense"), new songCard("Bus"), new songCard("KickBack"), new songCard("OCOLIMBO"), new songCard("cOin")};
	//public static songCard[] songsList = {new songCard("Lagtrain"), new songCard("ZenZenZense"), new songCard("KickBack"), new songCard("Bus"), new songCard("OCOLIMBO"), new songCard("cOin")};
	public float[] songPosList = new float[songsList.GetLength(0)];
	//public int cooldown = 0;
	//private int timeDelay = 0;

	private string mostMid = "Main";
	
	public void UpdateCard(string path, songCard newSongCard){
		var newCard = ResourceLoader.Load(newSongCard.diffCard) as Texture;
		var newBG = ResourceLoader.Load(newSongCard.imagePath) as Texture;

		GetNode<Sprite>(path+"Song/"+path+"Card").Texture = newCard;
		GetNode<Polygon2D>(path+"Song/"+path+"BG").Texture = newBG;
		GetNode<RichTextLabel>(path+"Song/"+path+"Diff").BbcodeText = "[color=black]" + newSongCard.difficulty.ToString() + "[/color]";
	}

	// public void UpdateBanner(songCard newSongCard){
	// 	GetNode<Sprite>("scene/songBanner").Texture = GetNode<Sprite>(newSongCard.diffBanner).Texture;
	// }

	public songCard GetSelected(){
		return songsList[selected];
	}

	public int GetIndex(int nextNum, int change){
		int result = nextNum+change;
		if(result >= songsList.GetLength(0)){
			result -= songsList.GetLength(0);
		}
		if(result < 0){
			result += songsList.GetLength(0);
		}
		return result;
	}

	public string PrevOf(string path){
		if(path == "Left"){
			return "Right";
		} else if (path == "Main"){
			return "Left";
		} else {
			return "Main";
		}
	}

	public string NextOf(string path){
		if(path == "Left"){
			return "Main";
		} else if (path == "Main"){
			return "Right";
		} else {
			return "Left";
		}
	}

	public void checkCard(string path, float calcOffset){
		float oldOffset = GetNode<PathFollow2D>(path + "Song").UnitOffset;
		GetNode<PathFollow2D>(path + "Song").UnitOffset -= calcOffset;
		if (oldOffset - calcOffset < 0) {
			UpdateCard(path, songsList[GetIndex(selected, -2)]);
			selected = GetIndex(selected, -1);
			mostMid = PrevOf(mostMid);
		} else if (oldOffset - calcOffset > 1){
			UpdateCard(path, songsList[GetIndex(selected, 2)]);
			selected = GetIndex(selected, 1);
			mostMid = NextOf(mostMid);
		}
	}
	public void SoundBounceReciever(float magnitude){
		//GD.Print(songsList[selected].bpm);
		if(magnitude > 0.25)magnitude=0.25f;
		//magnitude/=2;
		if((1+magnitude) > (GetNode<PathFollow2D>(mostMid+"Song").Scale.x)+0.1){
			GetNode<PathFollow2D>(mostMid+"Song").Scale = new Vector2(1+magnitude, 1+magnitude);
		}
		
	}
	public override void _Ready()
	{		
		
		UpdateCard("Left", songsList[0]);
		UpdateCard("Main", songsList[1]);
		UpdateCard("Right", songsList[2]);
		this.EmitSignal("update_bannerEventHandler", songsList[1]);
		this.EmitSignal("songChangeEventHandler", songsList[selected]);
	}
	
	public override void _Process(float delta)
	{
		//comment out these 3 lines of code to disable songcard rotation
		GetNode<PathFollow2D>("MainSong").RotationDegrees = -69 + (138*GetNode<PathFollow2D>("MainSong").UnitOffset);
		GetNode<PathFollow2D>("LeftSong").RotationDegrees = -69 + (138*GetNode<PathFollow2D>("LeftSong").UnitOffset);
		GetNode<PathFollow2D>("RightSong").RotationDegrees = -69 + (138*GetNode<PathFollow2D>("RightSong").UnitOffset);
		startPos = curPos;

		if(Input.IsActionJustPressed("press")){	
			startPos = GetGlobalMousePosition();
			curPos = GetGlobalMousePosition();
		} else if (Input.IsActionPressed("press")){
	
			songPosList[0] = GetNode<PathFollow2D>("LeftSong").UnitOffset;
			songPosList[1] = GetNode<PathFollow2D>("MainSong").UnitOffset;
			songPosList[2] = GetNode<PathFollow2D>("RightSong").UnitOffset;
			curPos = GetGlobalMousePosition();
		}
		float calcOffset = ((curPos.x-startPos.x)/1000);
		if(calcOffset > 0.3){calcOffset = (float)0.3;}else if(calcOffset < -0.3){calcOffset = (float)-0.3;}
		int prevSelected = selected;
		checkCard("Main", calcOffset);
		checkCard("Left", calcOffset);
		checkCard("Right", calcOffset);
		this.EmitSignal("update_bannerEventHandler", songsList[selected]);
		if(prevSelected != selected){
			this.EmitSignal("songChangeEventHandler", songsList[selected]);
		}
	}
}