using Godot;
using System;

public class GenericNote : Area2D
{
    public MapController controller;

    public Vector2 playRegion; // y1, y2
    public float speed;
    public bool active;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        controller = GetParent<MapController>();
        speed = controller.noteSpeed;
        playRegion = controller.playRegion;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Translate(new Vector2(0, delta * speed));
        if (Position.y > playRegion.x && Position.y < playRegion.y) active = true;
        else active = false; // miss if tp.y > pr.y
        InputPickable = active;
    }
}
