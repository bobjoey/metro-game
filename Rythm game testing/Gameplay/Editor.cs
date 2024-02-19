using Godot;
using System;

public class Editor : Node2D
{
    public MapController controller;
    // menu, grid, etc (timebar?)
    public Vector2 grid;

    public bool active;
    public int noteOption; // noteOption 0 none 1 tap 2 hold 3 swipeL 4 swipeR ? also maybe an option for color/line series
    public float space, songLengthPx;

    // Lists to help organize and access notes
    // one for lanes
    // one for color/chaining together

    // Called when the node enters the scene tree for the first time.
    // public override void _Ready() { }

    public void init()
    {
        controller = GetParent<MapController>();
        space = controller.space;
        songLengthPx = controller.songLengthPx;
        grid = new Vector2(controller.keyCount + 1, songLengthPx / space);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        active = controller.gameState == 2;

        // Visible = active; // there should be a better way to do this, maybe have some kinda popup/down animation for the menu and stuff and/or have the editor ver be a separate scene
        // if (!active) return;

        Update();
    }

    public override void _Draw()
    {
        Vector2 displaySize = controller.displaySize; // 480, 800
        float offset = controller.scrollPos;
        Color red = new Color(0.9f, 0.2f, 0.2f);
        float width = 3;

        // vertical
        for (int i = 1; i < grid.x; i++)
        {
            DrawLine(new Vector2(i * space, 0), new Vector2(i * space, displaySize.y), red, width);
        }

        // horizontal
        for (int i = 0; i <= grid.y; i++)
        {
            DrawLine(new Vector2(0, -i * space + offset), new Vector2(displaySize.x, -i * space + offset), red, width);
        }

        Vector2 ar = controller.playRegion;
        DrawLine(new Vector2(0, ar.x), new Vector2(displaySize.x, ar.x), new Color(0.2f, 0.9f, 0.2f), width);
        DrawLine(new Vector2(0, ar.y), new Vector2(displaySize.x, ar.y), new Color(0.2f, 0.9f, 0.2f), width);
    }


    // placing notes: use note/lane options, snap to grid, add to relevant lists and create connecting track
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (!active) return;
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            if (eventMouseButton.Position.x - space / 2 < 0) return;
            int x = (int)((eventMouseButton.Position.x - space/2) / space);
            if (x >= controller.keyCount) return;
            int y = (int)((-eventMouseButton.Position.y + controller.scrollPos + space/2) / space);
            if (y < 0) y = 0; else if (y >= controller.noteSlots.GetLength(1)) y = controller.noteSlots.GetLength(1)-1;
            placeNote(x, y);
        }
    }

    public void placeNote(int x, int y)
    {
        NoteSlot slot = controller.noteSlots[x, y];

        if (slot.full) {
            slot.removeNote();
        }
        else
        {
            GenericNote note = controller.tapNote.Instance<TapNote>(); // add a switch for diff note types based on noteOption
            controller.noteSlots[x, y].addNote(note);
        }
    }

    // choosing note and lane

}
