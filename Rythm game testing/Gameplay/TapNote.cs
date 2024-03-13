using Godot;
using System;

public class TapNote : GenericNote
{


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);

    }
    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            active = false;
            Visible = false;
            // increase score here
            // GD.Print("U press me");
        }

        // GetTree().SetInputAsHandled();
    }
}
