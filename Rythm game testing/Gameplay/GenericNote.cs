using Godot;
using System;
using System.Diagnostics;

public class GenericNote : Area2D
{
	public MapController controller;

	public Editor editor;

	public Vector2 playRegion; // y1, y2
	public float speed;
	public bool active;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controller = GetParent<MapController>();
		//editor = GetNode<Editor>("/root/Gameplay/Editor.cs");
		speed = controller.noteSpeed;
		playRegion = controller.playRegion;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Translate(new Vector2(0, delta * speed));
		if (Position.y > playRegion.x && Position.y < playRegion.y) active = true;
		else active = false; // miss if tp.y > pr.y
		InputPickable = active;
	}

	public void spawn(Vector2 location, int info, Vector2[,] snapPoints){
		Vector2 spawnPlace = snapPoints[(int)location.x, (int)location.y];
		spawnPlace.y *= -1;
		GD.Print(spawnPlace);
		Translate(spawnPlace);
		/*var path = getSpritePath(info);
		var sprite = ResourceLoader.Load(path) as Texture;

		GetNode<Sprite>("Sprite").Texture = sprite;*/
	}

	public string getSpritePath(int info){
		var color = "green";
		switch(info/10){
			case 1:
				color = "green";
				break;
			case 2:
				color = "purple";
				break;
			case 3:
				color = "red";
				break;
			case 4:
				color = "yellow";
				break;
		}
		var name = "Note";
		switch(info%10){
			case 1:
				name = "Note";
				break;
			case 2:
				color = "HoldNote";
				break;
			case 3:
				color = "SwipeNote";
				break;
			case 4:
				color = "SwipeNote"; // make left swipe note when
				break;
		}
		return "res://gameSprites/gameplay/"+color+name+".png";
	}
}
