using Godot;
using System;

public class Editor : Node2D
{
	public MapController controller;
	// menu, grid, etc (timebar?)
	public Vector2 grid;
	public Vector2[,] snapPoints;

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
		Vector2 displaySize = controller.displaySize; // 480, 800
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

}
