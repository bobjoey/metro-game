using Godot;
using System;
using System.CodeDom;
using System.Security.Policy;

public class MapController : Node2D
{
	public AudioStreamPlayer songPlayer;

	[Export]
	public PackedScene tapNote;
	[Export]
	public PackedScene holdNote;
	[Export]
	public PackedScene swipeNote;

	public System.Collections.Generic.List<GenericNote> notes; // split into lanes? sort by lane?? --> manage in editor

	public Vector2 displaySize = new Vector2(540, 890);
	public Vector2 playRegion = new Vector2(650, 750); // y1, y2: bottom 1/8 of screen +- 1/4 sec
	public int gameState; // 0 = pause, 1 = play, 2 = edit
	public int keyCount = 4;
	public float bpm = 120;
	public float noteSpeed = 200; // px per sec
	public float scrollPos = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		songPlayer = GetNode<AudioStreamPlayer>("SongPlayer");
		noteSpeed = bpm/60 * displaySize.y/8; // time to fall = 480/bpm seconds
		playRegion = new Vector2(displaySize.y*0.875f - noteSpeed*0.25f, displaySize.y*0.875f + noteSpeed*0.25f);
		// scrollPos = getPositionRatio() * displaySize.y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		scrollPos += delta*noteSpeed;
		/*
		if (gameState == 0) {
			return;
		}
		else if (gameState == 1)
		{
			foreach (GenericNote n in notes)
			{
				n.update(delta);
			}
		}
		*/
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
	}

	// immediately starts game at a certain timestamp (used in editor)
	public void enterPlayInsert(float time)
	{
		gameState = 1;
		// take some code from startplay but generation wise may be issues with start bar

	}

	public void enterPause()
	{
		gameState = 0;
		songPlayer.Stop(); // does it need to store timestamp?
		// some stuff
	}

	public void enterEdit()
	{
		gameState = 2;
		songPlayer.Stop();

	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventMouseButton eventMouseButton)
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
		return 120;
		//return songPlayer.Stream.GetLength();
	}

	public float getPositionRatio()
	{
		return songPlayer.GetPlaybackPosition() / getSongLength();
	}



}
