using Godot;
using System;
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

    public NoteSlot[,] noteSlots;
    // public System.Collections.Generic.List<GenericNote> notes; // split into lanes? sort by lane?? --> manage in editor

    public Vector2 displaySize = new Vector2(1181, 1968);
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        editor = GetNode<Editor>("Editor");
        songPlayer = GetNode<AudioStreamPlayer>("SongPlayer");

        // updateInfo();
        // finding the song:
        string path = "res://settings.txt";
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			string song = file.GetLine();
            song = file.GetLine(); // is there a better way to open the second line? (pls fix)
			file.Close();
            songCode = song.Substring(6);
		} else {
			GD.Print("not found: " + path);
		}

        loadSong(new songCard(songCode));
    }

    public void updateInfo() // updates noteSpeed, playRegion, space, songLengthPx, noteSlots
    {
        // displaySize = OS.WindowSize;
        noteSpeed = bpm / 60 * displaySize.y / 8; // time to fall = 480/bpm seconds
        playRegion = new Vector2(displaySize.y * 0.875f - noteSpeed * 0.25f, displaySize.y * 0.875f + noteSpeed * 0.25f);
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

        editor.init();
    }

    public void loadSong(songCard song)
    {
        time = 0;
        songPlayer.Stream = song.songAudio;
        bpm = song.bpm;
        // some other cosmetic data like title and author to be displayed at the start maybe

        updateInfo();
        editor.setNotes(song.songCode);
        enterPlay();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (gameState)
        {
            case 0:
                break;

            case 1:
                if (time > getSongLength()) enterPause();

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
        // delay & title
        // delay can be done by generating notes and "song start bar" higher up
        // there will be an interaction bar for where notes can be pressed and when the song start bar hits there the song & game starts
        // ^ will be done elsewhere (perhaps in their own funcs or the interaction bar)

        // play song and start the game
        songPlayer.Play(time);
        editButtonVisible(false);
    }

    // immediately starts game at a certain timestamp (used in editor)
    public void enterPlayInsert(float time)
    {
        gameState = 1;
        // take some code from startplay but generation wise may be issues with start bar
        songPlayer.Play(time);
        editButtonVisible(false);
    }

    public void enterPause()
    {
        gameState = 0;
        songPlayer.Stop();
        editButtonVisible(false);

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
}
