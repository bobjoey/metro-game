using Godot;
using System;

public class PauseButton : Area2D
{
    public MapController controller;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InputPickable = true;
        controller = GetParent<MapController>();
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            if (controller.gameState != 0) controller.enterPause();
            else controller.enterPlay();
        }

        GetTree().SetInputAsHandled();
    }
}
