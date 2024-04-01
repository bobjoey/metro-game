using Godot;
using System;
using System.Runtime.InteropServices;

public class SongComplete : Control
{
    songCard currentSong;
    int score;
    int pct;

    string volume;
    string song;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		string path = "user://settings.txt";
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			volume = file.GetLine();
			song = file.GetLine();
            score = Convert.ToInt32(file.GetLine());
			file.Close();
			currentSong = new songCard(song.Substring(6));
		} else {
			GD.Print("not found: " + path);
		}

        file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Write);
			file.StoreLine(volume);
            file.StoreLine(song);
			file.Close();
		}

        pct = (int)(((double)score/(double)currentSong.maxScore) * 100);
        GetNode<RichTextLabel>("SongName").BbcodeText = "Song: " + currentSong.songTitle;
        GetNode<RichTextLabel>("Percentage").BbcodeText = pct + "%";
        GetNode<RichTextLabel>("Fraction").BbcodeText = "Score: " + score + " / " + currentSong.maxScore;
        if(pct < 70){
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampF.png") as Texture;
        } else if(pct >=70 && pct < 80){
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampC.png") as Texture;
        } else if(pct >= 80 && pct < 90){
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampB.png") as Texture;
        } else if(pct >= 90 && pct < 98){
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampA.png") as Texture;
        } else if(pct >= 98 && pct < 100){
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampS.png") as Texture;
        } else{
            GetNode<Sprite>("RankStamp").Texture = ResourceLoader.Load("res://gameSprites/SongComplete/StampP.png") as Texture;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      
  }
}
