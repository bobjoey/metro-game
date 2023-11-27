using Godot;
using System;

public class TimeBar : Area2D
{
    public MapController controller;

    public Vector2 size = new Vector2(8, 320);
    public Node2D scroll;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        InputPickable = true;
        controller = GetParent<MapController>();
        scroll = GetNode<Node2D>("Scroll");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        scroll.Position = new Vector2(0, size.y - controller.getPositionRatio() * size.y);
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);
        if (@event is InputEventMouseButton emb) GD.Print("Input received at " + emb.Position);
        if (controller.gameState != 2) return;

        if (@event is InputEventMouseButton eventMouseButton)
        {
            Vector2 mpos = eventMouseButton.Position - Position + size;
            controller.songPlayer.Seek(controller.getSongLength() * mpos.y / size.y / 2);
        }
    }
}
