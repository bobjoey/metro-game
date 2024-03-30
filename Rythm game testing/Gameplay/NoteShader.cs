using Godot;
using System;

public class NoteShader : Node2D
{
    public MapController controller;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        controller = GetParent<MapController>();
        Update();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public override void _Draw()
    {
        base._Draw();
        drawShader();
    }
    public void drawShader(){
        // Color color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        // Rect2 rect2 = new Rect2(new Vector2(0,controller.playRegion.y), new Vector2(controller.displaySize.x,controller.displaySize.y));
        // DrawRect(rect2, color, true);
        Color color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        Rect2 rect2 = new Rect2(new Vector2(0,controller.playRegion.x), new Vector2(controller.displaySize.x,controller.playRegion.y-controller.playRegion.x-4));
        DrawRect(rect2, color, true);
    }
}
