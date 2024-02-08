using Godot;
using System;

public class Editor : Node2D
{
	public MapController controller;
	// menu, grid, etc (timebar?)
	public Vector2 grid;
	public Vector2[,] snapPoints;

	public Vector2[] noteGrid; // grid of notes in song, x being the laneOption & y being the noteOption

	public bool active;
	public int laneOption, noteOption; // laneOption 0 1 2 3, noteOption 0 none 1 tap 2 hold 3 swipeL 4 swipeR ?
	public float space, songLengthPx;
	

	// Lists to help organize and access notes
	// one for lanes
	// one for color/chaining together

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controller = GetParent<MapController>();
		space = controller.displaySize.x / (controller.keyCount+1);
		songLengthPx = controller.getSongLength() * controller.noteSpeed;
		grid = new Vector2(controller.keyCount + 1, songLengthPx / space+1);

		snapPoints = new Vector2[(int)grid.x, (int)grid.y];
		for (int x = 0; x < grid.x; x++)
		{
			for (int y = 0; y < (int)grid.y; y++)
			{
				snapPoints[x, y] = new Vector2(x*space+space, y*space);
			}
		}
		fillNoteGrid();
		for(int i=0;i<noteGrid.Length;i++){
			controller.addNote(noteGrid[i],snapPoints);
		}
		//controller.addNote(noteGrid[0],snapPoints);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		active = controller.gameState == 2;
		// Visible = active; // there should be a better way to do this, maybe have some kinda popup/down animation for the menu and stuff and/or have the editor ver be a separate scene

		// if (!active) return;

		drawGrid();
	}


	// vertical and horizontal lines based on truePos (somehow find a way to snap onto intersection pts)
	public void drawGrid()
	{
		/*
		Vector2 displaySize = controller.displaySize; // 480, 800  edited to 540 temporary, 890 for updated screen size of 540x891
		float offset = controller.scrollPos;

		Vector2[] pts = new Vector2[(int) (grid.x*2 + grid.y*2)];
		int index = 0;

		// vertical
		for (int i = 1; i < grid.x; i++)
		{
			pts[index] = new Vector2(i*space, 0);
			pts[index+1] = new Vector2(i*space, displaySize.y);
			index += 2;
		}

		// horizontal
		for (int i = 0; i <= grid.y; i++)
		{
			pts[index] = new Vector2(0, i*space + offset);
			pts[index+1] = new Vector2(displaySize.x, i*space + offset);
			index += 2;
		}

		// DrawMultiline(pts, new Color(0.1f, 0.1f, 0.1f)); // ???AAAAAAAAAAAAAAAAAAAAAAAAAAA */
		Update();

	}

	public override void _Draw() // AAAAAAAAAAAAAAAAAAAAAAAAAAA
	{
		Vector2 displaySize = controller.displaySize; // 540, 891
		float offset = controller.scrollPos;

		Vector2[] pts = new Vector2[(int)(grid.x * 2 + grid.y * 2)];
		int index = 0;

		// vertical
		for (int i = 1; i < grid.x; i++)
		{
			pts[index] = new Vector2(i * space, 0);
			pts[index + 1] = new Vector2(i * space, displaySize.y);
			index += 2;
		}

		// horizontal
		for (int i = 0; i <= grid.y; i++)
		{
			pts[index] = new Vector2(0, - i * space + offset);
			pts[index + 1] = new Vector2(displaySize.x, - i * space + offset);
			index += 2;
		}
		DrawMultiline(pts, new Color(0.9f, 0.2f, 0.2f));
	}


	// placing notes: use note/lane options, snap to grid, add to relevant lists and create connecting track


	// choosing note and lane

	// noteGrid: each number in the Vector2 has important parts:
	// first number: first digit = lane, second part = row number
	// second number: first digit = color, second digit = noteType
	// colors: 1=green, 2=purple, 3=red, 4=yellow
	// noteTypes: 1 = tap, 2=hold, 3=swipe left, 4=swipe right,
	// idk how were gonna do holds, but it could be that nums 5-9 are for hold values, and ones of same value connect to each other
	public void fillNoteGrid(){
		var globalVariables = (globalVariables)GetNode("/root/globalVariables");
		string songCode = globalVariables.songCode;
		string path = "res://songs/"+songCode+"/"+songCode+"Notes.txt";
		GD.Print(path);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			string[] lines = new string[Convert.ToInt32(file.GetLine())];
			int iter = 0;
			while(!file.EofReached()){
				lines[iter] = file.GetLine();
				iter++;
			}
			file.Close();
			noteGrid = new Vector2[lines.Length];
			for(int i=0;i<noteGrid.Length;i++){
				noteGrid[i].x = Convert.ToInt32(lines[i].Substring(0, 4));
				noteGrid[i].y = Convert.ToInt32(lines[i].Substring(5, 2));
				GD.Print(noteGrid[i]);
			}
		} else {
			GD.Print("not found: " + path);
		}
	}

}
