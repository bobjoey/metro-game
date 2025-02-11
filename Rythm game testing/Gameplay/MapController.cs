using Godot;
using System;
using System.Collections.Generic;
using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Security.Policy;

public class MapController : Node2D
{
	public Editor editor;
	public AudioStreamPlayer songPlayer;

	[Export]
	public PackedScene tapNote;
	[Export]
	public PackedScene holdNote;
	[Export]
	public PackedScene swipeNote;
	[Export]
	public PackedScene noteSlot;
	[Export]
	public PackedScene noteShader;

	public NoteSlot[,] noteSlots;
	// public System.Collections.Generic.List<GenericNote> notes; // split into lanes? sort by lane?? --> manage in editor

	public Vector2 displaySize = new Vector2(1181, 2560); // should be 2560 was 1968
	public Vector2 playRegion; // y1, y2: bottom 1/8 of screen +- 1/4 sec
	public int gameState = 0; // 0 = pause, 1 = play, 2 = edit (?)
	public int keyCount = 4;
	public float bpm = 120;
	public float noteSpeed = 200; // px per sec
	public float time = 0;
	public float scrollPos = 0;
	public float space;
	public float songLengthPx;
	public string songCode; // the song code, used in saveNotes()
	public int score = 0;
	public RichTextLabel scoreLabel;
	public List<String>songFile; // Read the entire file at the start into an array, make all the changes you want to the array, and then write the array back into the file at the end
	private string filePath;
	public TouchController touchController;
	public AudioStreamPlayer tapPlayer;
	public AudioStreamPlayer holdPlayer;
	public AudioStreamPlayer swipePlayer;
	public Timer holdTimer;
	private string volume;
	public float current = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		editor = GetNode<Editor>("Editor");
		songPlayer = GetNode<AudioStreamPlayer>("SongPlayer");
		scoreLabel = GetNode<RichTextLabel>("ScoreLabel");
		touchController = GetNode<TouchController>("TouchController");
		tapPlayer = GetNode<AudioStreamPlayer>("tapPlayer");
		holdPlayer = GetNode<AudioStreamPlayer>("holdPlayer");
		swipePlayer = GetNode<AudioStreamPlayer>("swipePlayer");
		holdTimer = GetNode<Timer>("HoldSoundTimer");

		string path = "user://settings.txt";
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			volume = file.GetLine();
			string song = file.GetLine(); // is there a better way to open the second line? (pls fix)
			file.Close();
			songCode = song.Substring(6);
		} else {
			GD.Print("not found: " + path);
		}
		loadSong(new songCard(songCode));
		//readFile(new songCard(songCode));
	}
	// public void readFile(songCard song){
	// 	string path = "user://"+song.songCode+".txt";
	// 	filePath = path;
	// 	File file = new File();
	// 	if(file.FileExists(path)){
	// 		file.Open(path, File.ModeFlags.Read);
	// 		songFile = new List<String>();
	// 		int iter = 0;
	// 		while(!file.EofReached()){
	// 			songFile.Add(file.GetLine());
	// 			iter++;
	// 		}
	// 		file.Close();
	// 	} else {
	// 		GD.Print("not found: " + path);
	// 	}
	// }

	// public void writeFile(){
	// 	string path = filePath;
	// 	File file = new File();
	// 	if(file.FileExists(path)){
	// 		file.Open(path, File.ModeFlags.Write);
	// 		int iter = 0;
	// 		while(iter < songFile.Count){
	// 			file.StoreLine(songFile[iter]);
	// 			GD.Print(songFile[iter]);
	// 			iter++;
	// 		}
	// 		file.Close();
	// 	} else {
	// 		GD.Print("not found: " + path);
	// 	}
	// }

	public void updateInfo() // updates noteSpeed, playRegion, space, songLengthPx, noteSlots
	{
		// displaySize = OS.WindowSize;
		noteSpeed = bpm / 60 * displaySize.y / 10; // time to fall = 480/bpm seconds changed it from displaySize.y/8 to /10 now 600/bpm seconds
		playRegion = new Vector2(displaySize.y * 0.875f - noteSpeed * 0.25f, displaySize.y * 0.875f + noteSpeed * 0.25f);
		//playRegion = new Vector2(displaySize.y/2, displaySize.y);
		// scrollPos = getPositionRatio() * displaySize.y;

		space = displaySize.x / (keyCount + 1);
		songLengthPx = getSongLength() * noteSpeed;
		int vSlotCnt = (int)(songLengthPx / space) + 1;
		noteSlots = new NoteSlot[keyCount, vSlotCnt];

		for (int x = 0; x < keyCount; x++)
		{
			for (int y = 0; y < vSlotCnt; y++)
			{
				NoteSlot slot = noteSlot.Instance<NoteSlot>();
				slot.Position = new Vector2(x * space + space, -y * space);
				noteSlots[x, y] = slot;
				AddChild(slot);
			}
		}
		NoteShader shader = noteShader.Instance<NoteShader>();
		AddChild(shader);
		editor.init();
	}

	public void loadSong(songCard song)
	{
		time = 0;
		songPlayer.Stream = song.songAudio;
		bpm = song.bpm;
		// some other cosmetic data like title and author to be displayed at the start maybe
		GD.Print("Current highscore: "+song.highScore);
		updateInfo();
		editor.setNotes(song.songCode);
		//enterPlay();
		startAnimation();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		switch (gameState)
		{
			case 0:
				break;

			case 1:
				if (time > getSongLength()){
					songPlayer.Stop();
					fadeToBlack();
				}

				time += delta;
				scrollPos = time * noteSpeed + playRegion.x;

				break;

			case 2:
				break;

			default: break;
		}
		
	}

	public void updateTime(float newTime)
	{
		time = newTime;
		scrollPos = time * noteSpeed + playRegion.x;
	}

	// starts the game with an initial delay to show the title card (playing songs in normal game)
	public void enterPlay()
	{
		gameState = 1;
		AnimatedSprite loadScreen = GetNode<AnimatedSprite>("LoadScreen");
		loadScreen.Visible = false;
		loadScreen.Stop();
		GetNode<Sprite>("RedundantBG").Visible=false;
		// delay & title
		// delay can be done by generating notes and "song start bar" higher up
		// there will be an interaction bar for where notes can be pressed and when the song start bar hits there the song & game starts
		// ^ will be done elsewhere (perhaps in their own funcs or the interaction bar)

		// play song and start the game
		songPlayer.Play(time);
		editButtonVisible(false);
		pauseButtonVisible(false);
	}

	// immediately starts game at a certain timestamp (used in editor)
	public void enterPlayInsert(float time)
	{
		gameState = 1;
		// take some code from startplay but generation wise may be issues with start bar
		songPlayer.Play(time);
		editButtonVisible(false);
		pauseButtonVisible(false);
	}

	public void enterPause()
	{
		gameState = 0;
		songPlayer.Stop();
		editButtonVisible(false);
		pauseButtonVisible(true);
	}

	public void enterEdit()
	{
		gameState = 2;
		songPlayer.Stop();

		foreach (NoteSlot slot in noteSlots)
		{
			slot.reset();
		}
		editButtonVisible(true);
		pauseButtonVisible(false);
		increaseScore(-score); // no cheaters >:(
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
		{
			
		}
	}

	// Loads a song into songPlayer - instead of AudioStream take in a class that contains audiostream and other info like title, author, difficulty that would be used in song selection -> title card at start of play?
	// should also include bpm and stuff too, basically rework this into essentially a constructor that is called whenever entering the game state
	public void loadSong(AudioStream song)
	{
		songPlayer.Stop();
		songPlayer.Stream = song;
	}

	public float getSongLength() // in seconds
	{
		return songPlayer.Stream.GetLength();
	}

	public float getPositionRatio()
	{
		return songPlayer.GetPlaybackPosition() / getSongLength();
	}

	public void saveNotes(){ // saves notes to a txt
		GD.Print("attempting to save notes");
		string path = "res://songs/"+songCode+"/"+songCode+"Notes.txt";
		GD.Print(path);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Write);
			for(int x = 0; x < noteSlots.GetLength(0); x++){
				for(int y = 0; y < noteSlots.GetLength(1); y++){
					if (noteSlots[x, y].full){
						string color = noteSlots[x, y].color;
						string type = Convert.ToString(noteSlots[x, y].noteType);
						string yString = Convert.ToString(y);
						if (y<100){
							yString = "0"+yString;
							if(y<10){
								yString = "0"+yString;
							}
						}
						string line = Convert.ToString(x) + " " + yString + " " + color + " " + type;
						file.StoreLine(line);
					}
				}
			}
			file.Close();
		} else {
			GD.Print("not found: " + path);
		}
		saveMaxScore();
	}

	public void saveMaxScore(){
		GD.Print("attempting to max score notes");
		songCard song = new songCard(songCode);
		string path = "res://songs/"+songCode+"/"+songCode+".txt";
		GD.Print(path);
		int maxScore = 0;
		for(int x = 0; x < noteSlots.GetLength(0); x++){
			for(int y = 0; y < noteSlots.GetLength(1); y++){
				if (noteSlots[x, y].full){
					maxScore += noteSlots[x, y].note.pointValue;
				}
			}
		}
		//songFile[6] = maxScore.ToString();
		GD.Print("max score = "+maxScore);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Write);
			file.StoreLine(Convert.ToString(song.difficulty));
            file.StoreLine(song.songTitle);
            file.StoreLine(song.songAuthor);
            file.StoreLine(Convert.ToString(song.songLength));
            file.StoreLine(Convert.ToString(song.previewTime));
            file.StoreLine(Convert.ToString(song.bpm));
            file.StoreLine(Convert.ToString(maxScore));
			file.Close();
		}
	}

	public void setNotePlaceSetting(string color, int type){
		if(color!="e"){
			editor.noteColor = color;
		}
		if(type!=0){
			editor.noteType = type;
		}
		GD.Print("Color: "+editor.noteColor+", type: "+editor.noteType);
	}

	public void editButtonVisible(bool yesno){
		GetNode<Button>("TapNoteButton").Visible = yesno;
		GetNode<Button>("HoldNoteButton").Visible = yesno;
		GetNode<Button>("Hold1NoteButton").Visible = yesno;
		GetNode<Button>("LSwipeNoteButton").Visible = yesno;
		GetNode<Button>("RSwipeNoteButton").Visible = yesno;
		GetNode<Button>("GNoteButton").Visible = yesno;
		GetNode<Button>("PNoteButton").Visible = yesno;
		GetNode<Button>("YNoteButton").Visible = yesno;
		GetNode<Button>("RNoteButton").Visible = yesno;
	}

	public void pauseButtonVisible(bool yesno){
		GetNode<Button>("RetryButton").Visible = yesno;
		GetNode<Button>("ExitButton").Visible = yesno;
		GetNode<Button>("LeavePauseButton").Visible = yesno;
		GetNode<Sprite>("PauseMenu").Visible = yesno;
	}

	public void retryLevel(){
		//retry		
		GD.Print("re-trying level");		
		saveScore(songCode);
		GetTree().ReloadCurrentScene();
	}

	public void exitLevel(){
		saveScore(songCode);
		GD.Print("exiting level");
		GetTree().ChangeScene("res://songComplete/SongComplete.tscn");
		//GetTree().ChangeScene("res://songSelect/songSelect.tscn");
	}
	
	public void saveScore(string songCode){
		songCard curSong = new songCard(songCode);
		if(score > curSong.highScore){
			//songFile[7] = score.ToString();
			string path = "user://"+songCode+".txt";
			File file = new File();
			if(file.FileExists(path)){
				file.Open(path, File.ModeFlags.Write);
				file.StoreLine(score.ToString());
				file.Close();
			}
			GD.Print("new highscore: "+score);
		} else{
			GD.Print("not a high score, do better");
		}
		
		File otherFile = new File();
		if(otherFile.FileExists("user://settings.txt")){
			otherFile.Open("user://settings.txt", File.ModeFlags.Write);
			otherFile.StoreLine(volume);
			otherFile.StoreLine("song: " + songCode);
			otherFile.StoreLine(score.ToString());
			otherFile.Close();
		}
		else{
		}
		
		//writeFile();
	}
	public void increaseScore(int amount){
		score+=amount;
		// *** add score ui update here ***
		GD.Print("Score: "+score);
		scoreLabel.Text = "Score: " + score;
	}

	public void stopHoldNoteSound(){
		holdPlayer.Stop();
	}

	public void startAnimation(){
		AnimatedSprite loadScreen = GetNode<AnimatedSprite>("LoadScreen");
		loadScreen.Visible = true;
		loadScreen.Play("opening");
		GetNode<Sprite>("RedundantBG").Visible=true;
	}

	public void fadeToBlack(){
		Sprite fade = GetNode<Sprite>("FadeToBlack");
		fade.Visible = true;
		current += 0.05f;
		fade.Modulate = new Color(0, 0, 0, current);
		if(current>1){
			exitLevel();
		}
	}
}
