using Godot;
using System;

public class TimeBar : Area2D
{
    public MapController controller;

    public float size = 320;
    public Node2D scroll;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        size *= Scale.y;
        InputPickable = true;
        controller = GetParent<MapController>();
        scroll = GetNode<Node2D>("Scroll");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        // scroll.Position = new Vector2(0, size - controller.getPositionRatio() * size);
        scroll.Position = new Vector2(0, size - (controller.time / controller.getSongLength()) * size*2) / Scale; // temp
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (controller.gameState != 2) return;
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            // GD.Print("Input received at " + eventMouseButton.Position);
            float ratio = (Position.y + size - eventMouseButton.Position.y)/size/2;
            // controller.songPlayer.Seek(controller.getSongLength() * ratio);
            controller.updateTime(controller.getSongLength() * ratio); // temp
        }

        GetTree().SetInputAsHandled();
    }
}
