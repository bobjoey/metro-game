using Godot;
using System;

public class HoldNote : GenericNote
{
    //public bool holdStarted = false;
    public Vector2 mouseStartPos;
    public int holdCount = 0; // num of frames the hold lasts // should be irrelevent
    private bool frame1 = true;
    private bool hasNext = false;
    private Vector2 nextNote;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        base._Process(delta);
        if(frame1){
            nextNote = controller.editor.findNextHoldNote(slot, slot.noteY);
            frame1 = false;
            if(nextNote.x == -1){
                hasNext = false;
            } else{
                hasNext = true;
            }
        }
        // if(Input.IsActionPressed("press") && mouseInLine()){
        //     holdStarted = true;
        // }
        if(holdStarted){
            active = false;
            Visible = false;
        }
        if(hasNext && holdStarted && Input.IsActionPressed("press") && mouseInLine()){
           // increment score
            NoteSlot nextNoteSlot = controller.noteSlots[(int)nextNote.x, (int)nextNote.y]; 
            Vector2 mousePos = GetGlobalMousePosition();
            float nextNoteY = controller.scrollPos + nextNoteSlot.Position.y;
            GD.Print("holding");
            if((nextNoteY+50>=mousePos.y)&&(nextNoteY-50<=mousePos.y)){
                holdStarted = false;
                GD.Print("at next note");
                controller.noteSlots[(int)nextNote.x, (int)nextNote.y].note.holdStarted=true;
            }
        } else if(hasNext && holdStarted && Input.IsActionPressed("press") && !mouseInLine()){
            GD.Print("mouse out of line");
            holdStarted = false;
        }
        if(holdStarted && Input.IsActionPressed("press")==false){
            holdStarted = false;
            GD.Print("hold released");
        }
        
    }

    /*public override void _Draw()
    {
        base._Draw();
        if(hasNext && holdStarted && Input.IsActionPressed("press") && mouseInLine()){
            Vector2 slot1 = new Vector2(slot.noteX*controller.space+controller.space, controller.scrollPos + slot.Position.y);
            Vector2 slot2 = controller.editor.findNextHoldNote(slot, slot.noteY);
            DrawLine(slot1)
        }
    }*/

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (@event is InputEventMouseButton eventMouseButton) // starts hold
        {
            if(eventMouseButton.IsPressed()==true){
            holdStarted = true;
            mouseStartPos = GetGlobalMousePosition();
            GD.Print("hold started?");
            if(hasNext == false){
                active = false;
                Visible = false;
                holdStarted = false;
                GD.Print("no next note, removing this one");
            }
            }
        }

        // GetTree().SetInputAsHandled();
    }

    public bool mouseInLine(){ // returns true if mouse is in the path of the line connecting hold notes
        float slope = getSlope();
        Vector2 mousePos = GetGlobalMousePosition();
        float slotX = slot.noteX*controller.space+controller.space;
        float slotY = controller.scrollPos + slot.Position.y;
        float idealX = (slotY-mousePos.y)/slope*-1 + slotX;
        if((idealX+50>=mousePos.x)&&(idealX-50<=mousePos.x)){
            GD.Print("mouse in line");
            return true;
        }
        GD.Print("ideal:"+idealX + " but is: "+mousePos.x + ", note is at: "+slotX+","+slotY + " slope: "+slope+" mousey: "+mousePos.y);
        if(hasNext==false){GD.Print("no next, but out of line :(");}
        return false;
    }

    public float getSlope(){ // returns the slope of this note to the next note
        float slot2x = (float) (nextNote.x*controller.space+controller.space);
        float slot2y = (float) (controller.scrollPos + controller.noteSlots[(int)nextNote.x,(int)nextNote.y].Position.y);
        float slot1x = slot.noteX*controller.space+controller.space;
        float slot1y = controller.scrollPos + slot.Position.y;
        float slope = (slot2y - slot1y) / (slot2x - slot1x);
        return slope;
    }

    public void setHoldStarted(bool started){
        holdStarted = started;
    }
}
