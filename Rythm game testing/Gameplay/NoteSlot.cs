using Godot;
using System;

public class NoteSlot : Node2D
{
    public MapController controller;
    public GenericNote note;

    public bool full = false;
    public int noteType, color;

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

    public void addNote(GenericNote note)
    {
        this.note = note;
        AddChild(note);
        full = true;
        // add type and color data etc
    }

    public void removeNote()
    {
        note.QueueFree();
        note = null;
        full = false;
    }
}
