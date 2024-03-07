using Godot;
using System;

public class HoldNote : GenericNote
{
    //public bool holdStarted = false;
    public Vector2 mouseStartPos;
    private bool frame1 = true;
    private int minFrames = 0;
    private bool hasNext = false;
    private Vector2 nextNote;
    private NoteSlot nextNoteSlot;
    public bool isDone = false;

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
                nextNoteSlot = controller.noteSlots[(int)nextNote.x, (int)nextNote.y];
            }
        }
        if(minFrames<=20){ // minimum length to be able to fail (discards a little bit of weirdness)
            minFrames++;
        }
        if(isDone&&active){
            isDone=false;
        }

        if(isDone||(hasNext==false&&active==false)){
            Update();
        }
        if(holdStarted){
            active = false;
            //Visible = false;
        }
        if(hasNext && holdStarted && Input.IsActionPressed("press") && mouseInLine()){
            // increment score
            Vector2 mousePos = GetGlobalMousePosition();
            float nextNoteY = controller.scrollPos + nextNoteSlot.Position.y;
            GD.Print("holding");
            if((nextNoteY+80>=mousePos.y)&&(nextNoteY-80<=mousePos.y)){
                holdStarted = false;
                isDone = true;
                GD.Print("at next note");
                controller.noteSlots[(int)nextNote.x, (int)nextNote.y].note.holdStarted=true;
                //Visible = false;
                
            }
            Update();
        } else if(hasNext && holdStarted && Input.IsActionPressed("press") && !mouseInLine()&&minFrames>20){
            GD.Print("mouse out of line");
            holdStarted = false;
        }
        else if(holdStarted && Input.IsActionPressed("press")==false){
            holdStarted = false;
            GD.Print("hold released");
        }
        
    }

    public override void _Draw()
    {
        base._Draw();
        if(hasNext==false&&active==false){
            DrawCircle(new Vector2(0,0), 40, controller.editor.getColor(slot.noteX, slot.noteY));
        }
        if(isDone){
            Vector2 slot1 = new Vector2(slot.noteX*controller.space+controller.space, controller.scrollPos + slot.Position.y);
            Vector2 slot2 = new Vector2(nextNote.x*controller.space+controller.space, controller.scrollPos + nextNoteSlot.Position.y);
            slot2.x -= slot1.x;
            slot2.y -= slot1.y;
            DrawLine(new Vector2(0,0), slot2, controller.editor.getColor(slot.noteX, slot.noteY), 50, true);
            DrawCircle(new Vector2(0,0), 40, controller.editor.getColor(slot.noteX, slot.noteY));
        }
        else if(hasNext && holdStarted && Input.IsActionPressed("press") && mouseInLine()){
            Vector2 slot1 = new Vector2(slot.noteX*controller.space+controller.space, controller.scrollPos + slot.Position.y);
            Vector2 slot2 = getIdealCoord();
            slot2.x -= slot1.x;
            slot2.y -= slot1.y;
            DrawLine(new Vector2(0,0), slot2, controller.editor.getColor(slot.noteX, slot.noteY), 50, true);
            DrawCircle(new Vector2(0,0), 40, controller.editor.getColor(slot.noteX, slot.noteY));
        }
    }

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
        float idealX = getIdealCoord().x;
        if(mousePos.y>controller.playRegion.y && mousePos.y<controller.playRegion.x){
            return false;
        }
        if((idealX+120>=mousePos.x)&&(idealX-120<=mousePos.x)){
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

    public Vector2 getIdealCoord(){
        float slope = getSlope();
        Vector2 mousePos = GetGlobalMousePosition();
        float slotX = slot.noteX*controller.space+controller.space;
        float slotY = controller.scrollPos + slot.Position.y;
        float idealX = (slotY-mousePos.y)/slope*-1 + slotX;
        return new Vector2(idealX,mousePos.y);
    }
}
