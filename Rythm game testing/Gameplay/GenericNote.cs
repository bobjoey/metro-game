using Godot;
using System;

public class GenericNote : Area2D
{
    public NoteSlot slot;
    public MapController controller;

    public Vector2 playRegion; // y1, y2
    public bool active = true;
    public bool holdStarted = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        slot = GetParent<NoteSlot>();
        controller = slot.controller;
        playRegion = controller.playRegion;
        var sprite = GetNode<Sprite>("Sprite");
        sprite.Visible = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Translate(new Vector2(0, delta * speed));
        Position = new Vector2(0, controller.scrollPos);

        float pos = controller.scrollPos + slot.Position.y;

        if (pos > playRegion.x && pos < playRegion.y)
        {
            InputPickable = active && controller.gameState == 1;
        }
        else
        {
            InputPickable = false;
        }
        // else active = false; // miss if tp.y > pr.y
        // Visible = active;
    }

    public void setColorType(string color, int type){
         slot.color = color;
         slot.noteType = type;
    }

    

/*
    public void setColorType(string color, int type){
        string path = getSpritePath(color, type);
        var icon = ResourceLoader.Load(path) as Texture;
        var sprite = GetNode<Sprite>("Sprite");
        sprite.Texture = icon;
        float x = icon.GetWidth();
        float y = icon.GetHeight();
        sprite.Scale = new Vector2(200/x, 200/y);
        if(type == 44){// left swipe
            sprite.FlipH = true;
        }
         slot = GetParent<NoteSlot>();
         slot.color = color;
         slot.noteType = type;
    }

    public string getSpritePath(string color, int type){
		switch(color){
			case "g":
				color = "green";
				break;
			case "p":
				color = "purple";
				break;
			case "r":
				color = "red";
				break;
			case "y":
				color = "yellow";
				break;
		}

        var noteType = "Note";
		switch(type){
			case 11:
				noteType = "Note";
				break;
			case 22:
				noteType = "HoldNote";
				break;
			case 33:
				noteType = "SwipeNote";
				break;
			case 44:
				noteType = "SwipeNote"; // make left swipe note when
				break;
		}
		return "res://gameSprites/gameplay/"+color+noteType+".png";
	}
*/
}
