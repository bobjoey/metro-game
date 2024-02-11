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
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
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
