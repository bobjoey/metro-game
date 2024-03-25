using Godot;
using System;

public class SwipeNote : GenericNote
{
    public bool swipeStarted = false;
    public Vector2 mouseStartPos;
    public Vector2 touchStartPos;
    public int holdCount = 0; // num of frames the hold lasts

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        pointValue = 200;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(controller.editor.isOnScreen(slot.noteX, slot.noteY)!=1){
            return;
        }

        if(swipeStarted && Input.IsActionPressed("press") && holdCount<5){ // 5 is minimum time for swipe, 5 frames
            holdCount++;
            //GD.Print(holdCount + ": frames");
        }
        if(swipeStarted && Input.IsActionPressed("press") && holdCount>=5){
            //GD.Print("mouse: "+GetGlobalMousePosition());
            if(slot.noteType==33 && GetGlobalMousePosition().x >= mouseStartPos.x+100){ // 100 is minimum swipe distance (idk units)
                active = false;
                Visible = false;
                swipeStarted = false; 
            }
            else if(slot.noteType==44 && GetGlobalMousePosition().x <= mouseStartPos.x-100){
                active = false;
                Visible = false;
                swipeStarted = false;
                //increase score here
                controller.increaseScore(200);
            }
        }
        if(swipeStarted && Input.IsActionPressed("press")==false){
            swipeStarted = false;
        }
    }
    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (@event is InputEventMouseButton eventMouseButton) // starts hold
        {
            if(eventMouseButton.IsPressed()==true){
            swipeStarted = true;
            mouseStartPos = GetGlobalMousePosition();
            holdCount = 0;
            GD.Print("mouse: "+mouseStartPos);
            GD.Print("swipe started?");
            }
        }

        // GetTree().SetInputAsHandled();
    }
}
