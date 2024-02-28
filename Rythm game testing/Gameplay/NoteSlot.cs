using Godot;
using System;

public class NoteSlot : Node2D
{
    public MapController controller;
    public GenericNote note;

    public bool full = false;
    public string color = "g"; // g - green, p - purple, y - yellow, r - red
    public int noteType = 11; // 11 = tap, 22 = hold, 33 == r swipe, 44 = l swipe

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        controller = GetParent<MapController>();
        // speed = controller.noteSpeed;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    /*public override void _Process(float delta)
    {

    }*/

    public void addNote(GenericNote note, string color, int type)
    {
        this.note = note;
        AddChild(note);
        full = true;
        note.setColorType(color, type);
        // add type and color data etc
    }

    public void removeNote()
    {
        note.QueueFree();
        note = null;
        full = false;
    }

    public void reset()
    {
        if (note == null) return;
        note.active = true;
        note.Visible = true;
    }
}
