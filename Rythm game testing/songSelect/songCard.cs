//using System.IO;
using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class songCard : PathFollow2D
{
	public string songCode;
	public string diffCard; //difficulty 1-3 = Easy, 4-6 = Medium, 7-9 = Hard
	public string diffBanner;
	public string fPath = "res://songs/";
	
	//vars to be initialized with data from .txt file (MAKE SURE THE ORDER IS THE SAME AS HERE)
	public int difficulty;
	public string songTitle;
	public string songAuthor;
	public float songLength;
	public float previewTime;
	public int bpm;

	public AudioStreamMP3 songAudio; //isnt part of the .txt file, is read the mp3 directly with its path
	public string imagePath;//Isn't part of the .txt file, is read directly with its path
		
	public songCard(string songCode)
	{
		fPath+=(songCode+"/");
		getInfo(songCode);
		switch(difficulty){
			case 1: 
			case 2: 
			case 3:
				diffCard = "res://gameSprites/SongSelect/scEasy.png";
				//diffBanner = "res://gameSprites/SongSelect/sbEasy.png";
				diffBanner = "Easy";
				break;
			case 4: 
			case 5: 
			case 6:
				diffCard = "res://gameSprites/SongSelect/scMed.png";
				//diffBanner = "res://gameSprites/SongSelect/sbMed.png";
				diffBanner = "Medium";
				break;
			case 7: 
			case 8: 
			case 9:
				diffCard = "res://gameSprites/SongSelect/scHard.png";
				//diffBanner = "res://gameSprites/SongSelect/sbHard.png";
				diffBanner = "Hard";
				break;
			default:
				diffCard = "res://gameSprites/SongSelect/songCardBlankSkewed.png";
				diffBanner = "Medium";
				break;
		}
		//imagePath = ("res://gameSprites/SongsList/" + imagePath + ".png");
	}
	
	public void getInfo(string songCode) //res://songs/Lagtrain/lagtrain.png
	{
 		string path = (fPath + songCode + ".txt");
		//GD.Print(path);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			List<String> lines = new List<String>();
			int iter = 0;
			while(!file.EofReached()){
				lines.Add(file.GetLine());
				iter++;
			}
			file.Close();
			difficulty = Convert.ToInt32(lines[0]);
			songTitle = lines[1];
			songAuthor = lines[2];
			songLength = Convert.ToInt32(lines[3]);
			previewTime = Convert.ToInt32(lines[4]);
			bpm = Convert.ToInt32(lines[5]);
			imagePath = (fPath + songCode + ".png");
			songAudio = (AudioStreamMP3)GD.Load(fPath + songCode + ".mp3");
		} else {
			GD.Print("not found: " + path);
			difficulty = 0;
			songTitle = "FileNotFound";
			songAuthor = "FileNotFound";
			songLength = 0;
			previewTime = 0;
			imagePath = "res://icon.png";
			songAudio = null;
			bpm = 0;
		}
	}
	
	public void WriteAll(){
		//Console.WriteLine("songCode = " + songCode);
		//Console.WriteLine("diffCard = " + diffCard);
		//Console.WriteLine("imagePath = " + imagePath);
	}
}
