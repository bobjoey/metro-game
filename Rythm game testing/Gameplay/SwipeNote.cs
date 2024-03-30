using Godot;
using System;

public class SwipeNote : GenericNote
{
    public bool swipeStarted = false;
    public Vector2 mouseStartPos;
    public Vector2 touchStartPos;

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

        if(swipeStarted && Input.IsActionPressed("press")){
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
            } else if(checkTouches()){
                active = false;
                Visible = false;
                swipeStarted = false;
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
            GD.Print("mouse: "+mouseStartPos);
            GD.Print("swipe started?");
            }
        }

        if (@event is InputEventScreenTouch eventScreenTouch){
            if(eventScreenTouch.IsPressed()==true){
                swipeStarted = true;
                touchStartPos = eventScreenTouch.Position;
                GD.Print("swipe started? at:"+touchStartPos);
            }
        }

        // GetTree().SetInputAsHandled();
    }

    public bool checkTouches(){
        Vector2[] touchPositions = controller.touchController.touchPositions;
        for(int i=0; i<touchPositions.Length;i++){
            if(touchPositions[i].y>0){
                Vector2 pos = touchPositions[i];
                if(slot.noteType==33 && pos.x >= touchStartPos.x+100){
                    return true;
                }
                if(slot.noteType==44 && pos.x <= touchStartPos.x-100){
                    GD.Print("done at:" + touchPositions[i]);
                    return true;
                }
            }
        }
        return false;
    }
}
