using Godot;
using System;

public class GenericNote : Area2D
{
    public NoteSlot slot;
    public MapController controller;

    public Vector2 playRegion; // y1, y2
    public bool active;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        slot = GetParent<NoteSlot>();
        controller = slot.controller;
        playRegion = controller.playRegion;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // Translate(new Vector2(0, delta * speed));
        Position = new Vector2(0, controller.scrollPos);
        if (Position.y > playRegion.x && Position.y < playRegion.y) active = true;
        else active = false; // miss if tp.y > pr.y
        InputPickable = active;
        // Visible = active;
    }
}
