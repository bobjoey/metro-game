using Godot;
using System;
using System.Collections.Generic;

public class TouchController : Area2D
{

    public Vector2[] touchPositions = new Vector2[20]; // no shot we're getting more than 20 touches

    // Called when the node enters the scene tree for the first time.
    // public override void _Ready()
    // {

    // }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
//   public override void _Process(float delta)
//   {

//   }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventScreenTouch eventScreenTouch){
            if(eventScreenTouch.IsPressed()==true){
                GD.Print("pos"+eventScreenTouch.Position+" index: "+eventScreenTouch.Position);
                touchPositions[eventScreenTouch.Index] = eventScreenTouch.Position;
            } else{
                touchPositions[eventScreenTouch.Index] = new Vector2(-1,-1);
            }
        }

        if (@event is InputEventScreenDrag eventScreenDrag){
            touchPositions[eventScreenDrag.Index] = eventScreenDrag.Position;
        }
    }
}
