using Godot;
using System;

public class Background : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public int StartingOffest;

    public MapController controller;
    public float pos;
    public int size = 16300;
    public int times = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        controller = (MapController)GetParent();
        pos = StartingOffest;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        pos = controller.scrollPos+StartingOffest-controller.playRegion.x-size*times;
        if(pos-size/2>controller.displaySize.y){
            times+=2;
        }
        Position = new Vector2(Position.x, pos);
    }
}
