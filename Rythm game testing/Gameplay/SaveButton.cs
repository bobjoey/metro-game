using Godot;
using System;

public class SaveButton : Area2D
{
    public MapController controller;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InputPickable = true;
        controller = GetParent<MapController>();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        /*bool active = true; // controller.gameState != 1;
        Visible = active;
        InputPickable = active;*/
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            if (controller.gameState == 2){
                GD.Print("its savin time B)");
                controller.saveNotes();
            } 
            else GD.Print("Not in Edit mode, Can't Save :(");
        }

        GetTree().SetInputAsHandled();
    }
}
