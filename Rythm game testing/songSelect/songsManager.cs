using Godot;
using System;
using System.ComponentModel.Design;

public class songsManager : Path2D
{
	
	//const songCard = import("res://path/to/word_printer.gd")
	//extends Path2D
	//var length = 150
	public Vector2 startPos;
	public Vector2 curPos;
	public int selected = 1; //Selected starts at 1 ( center/selected songCard is songsList[1] )
	public int nextSong = 3; //SongsList array index
	public static songCard[] songsList = {new songCard("Lagtrain"), new songCard("ZenZenZense"), new songCard("Bus"), new songCard("KickBack")};
	public float[] songPosList = new float[songsList.GetLength(0)];
	public int cooldown = 0;
	private int timeDelay = 0;
	//public songBanner banner = 
	
	public void UpdateCard(string path, songCard newSongCard){
		//GetNode<Sprite>("songBanner").UpdateBanner(newSongCard);
		//songBanner.UpdateBanner();
		var newCard = ResourceLoader.Load(newSongCard.diffCard) as Texture;
		var newBG = ResourceLoader.Load(newSongCard.imagePath) as Texture;

		GetNode<Sprite>(path+"Song/"+path+"Card").Texture = newCard;
		GetNode<Polygon2D>(path+"Song/"+path+"BG").Texture = newBG;
		GetNode<RichTextLabel>(path+"Song/"+path+"Diff").BbcodeText = "[color=black]" + newSongCard.difficulty.ToString() + "[/color]";
	}

	public songCard GetSelected(){
		return songsList[selected];
	}

	public int GetIndex(int nextNum, int change){
		// if(nextNum+change >= songsList.GetLength(0)){
		// 	return Math.Abs(nextNum+change - songsList.GetLength(0));
		// } else if (nextNum+change < 0){
		// 	return songsList.GetLength(0) + (nextSong+change);
		// } else {
		// 	return nextNum+change;
		// }
		if(nextNum + change == songsList.GetLength(0)){
			return 0;
		} else if (nextNum + change == -1){
			return songsList.GetLength(0)-1;
		} else {
			return nextNum + change;
		}
	}
	public bool IsNew(PathFollow2D song, string path, float lastPos){
		//GD.Print("lastPos = " + lastPos + " unit offset = " + song.UnitOffset + "\n");
		decimal curOffset = (decimal)song.UnitOffset;
		decimal prevOffset = (decimal)lastPos;
		curOffset = Decimal.Round(curOffset, 2);
		prevOffset = Decimal.Round(prevOffset, 2);
		//GD.Print("curOffset = " + curOffset + "\nprevOffset = " + prevOffset);
		if(curOffset >= 0 && curOffset <= (decimal)0.02 && prevOffset <= 1 && prevOffset >= (decimal)0.98)
		{
			UpdateCard(path, songsList[nextSong]);
			selected = GetIndex(selected, 1);
			nextSong = GetIndex(nextSong, 1);
			cooldown = 10;
			return true;
		} 
		
		else if(curOffset <= 1 && curOffset >= (decimal)0.98 && prevOffset >= 0 && prevOffset <= (decimal)0.02)
		{
			UpdateCard(path, songsList[GetIndex(nextSong, -1)]);
			selected = GetIndex(selected, -1);
			nextSong = GetIndex(nextSong, -1);
			cooldown = 10;
			return true;
		}

		return false;
	}
	
	public override void _Ready()
	{
		UpdateCard("Left", songsList[0]);
		UpdateCard("Main", songsList[1]);
		UpdateCard("Right", songsList[2]);
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
			if(cooldown == 0){
				if( IsNew(GetNode<PathFollow2D>("LeftSong"), "Left", songPosList[0]) ){}
				else if( IsNew(GetNode<PathFollow2D>("MainSong"), "Main", songPosList[1]) ){}
				else{IsNew(GetNode<PathFollow2D>("RightSong"), "Right", songPosList[2]);}
			}
			songPosList[0] = GetNode<PathFollow2D>("LeftSong").UnitOffset;
			songPosList[1] = GetNode<PathFollow2D>("MainSong").UnitOffset;
			songPosList[2] = GetNode<PathFollow2D>("RightSong").UnitOffset;
			curPos = GetGlobalMousePosition();
		}
		float calcOffset = ((curPos.x-startPos.x)/1000);
		if(calcOffset > 0.02){calcOffset = (float)0.02;}else if(calcOffset < -0.02){calcOffset = (float)-0.02;}
		GetNode<PathFollow2D>("MainSong").UnitOffset -= calcOffset;
		GetNode<PathFollow2D>("LeftSong").UnitOffset -= calcOffset;
		GetNode<PathFollow2D>("RightSong").UnitOffset -= calcOffset;

		// if(timeDelay % 10 == 0){
		// GD.Print(timeDelay);
		if(timeDelay % 50 == 0){
			GD.Print("selected = " + selected);
			GD.Print("nextSong = " + nextSong);
		}
		if(cooldown > 0){cooldown--;}
		//timeDelay++;
	}
}
