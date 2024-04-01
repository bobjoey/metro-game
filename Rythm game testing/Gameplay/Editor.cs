using Godot;
using System;
using System.Diagnostics;

public class Editor : Node2D
{
    public MapController controller;
    // menu, grid, etc (timebar?)
    public Vector2 grid;

    public bool active;
    public int noteOption; // noteOption 0 none 1 tap 2 hold 3 swipeL 4 swipeR ? also maybe an option for color/line series
    public float space, songLengthPx;

    public int noteType = 11;
    public string noteColor = "g";

    // Lists to help organize and access notes
    // one for lanes
    // one for color/chaining together

    // Called when the node enters the scene tree for the first time.
    // public override void _Ready() { }

    public void init()
    {
        controller = GetParent<MapController>();
        space = controller.space;
        songLengthPx = controller.songLengthPx;
        grid = new Vector2(controller.keyCount + 1, songLengthPx / space);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        active = controller.gameState == 2;

        // Visible = active; // there should be a better way to do this, maybe have some kinda popup/down animation for the menu and stuff and/or have the editor ver be a separate scene
        // if (!active) return;

        Update();
    }

    public override void _Draw()
    {
        Vector2 displaySize = controller.displaySize; // 480, 800
        float offset = controller.scrollPos;
        Color red = new Color(0.9f, 0.2f, 0.2f);
        float width = 4;

        if(controller.gameState==2){
            // vertical
            for (int i = 1; i < grid.x; i++)
            {
                DrawLine(new Vector2(i * space, 0), new Vector2(i * space, displaySize.y), red, width);
            }

            // horizontal
            for (int i = 0; i <= grid.y; i++)
            {
                DrawLine(new Vector2(0, -i * space + offset), new Vector2(displaySize.x, -i * space + offset), red, width);
            }
        }
        

        Vector2 ar = controller.playRegion;
        DrawLine(new Vector2(0, ar.x), new Vector2(displaySize.x, ar.x), new Color(0.2f, 0.9f, 0.2f), width);
        DrawLine(new Vector2(0, ar.y), new Vector2(displaySize.x, ar.y), new Color(0.2f, 0.9f, 0.2f), width);

        drawNoteConnections();

        drawHoldNotes(0); // outer colored line

        drawNotes();

        drawHoldNotes(1); // inner white line
    }


    // placing notes: use note/lane options, snap to grid, add to relevant lists and create connecting track
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (!active) return;
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed == true)
        {
            if (eventMouseButton.Position.x - space / 2 < 0) return;
            int x = (int)((eventMouseButton.Position.x - space/2) / space);
            if (x >= controller.keyCount) return;
            int y = (int)((-eventMouseButton.Position.y + controller.scrollPos + space/2) / space);
            if (y < 0) y = 0; else if (y >= controller.noteSlots.GetLength(1)) y = controller.noteSlots.GetLength(1)-1;
            placeNote(x, y, noteColor, noteType);
        }
    }

/*
    public void placeNote(int x, int y)
    {
        NoteSlot slot = controller.noteSlots[x, y];

        if (slot.full) {
            slot.removeNote();
        }
        else
        {
            GenericNote note = controller.tapNote.Instance<TapNote>(); // add a switch for diff note types based on noteOption
            controller.noteSlots[x, y].addNote(note);
        }
    }
*/

    public void placeNote(int x, int y, string color, int type)
    {
        NoteSlot slot = controller.noteSlots[x, y];
        slot.noteX = x;
        slot.noteY = y;

        if (slot.full) {
            slot.removeNote();
        }
        else
        {
            if(type==11){
                GenericNote note = controller.tapNote.Instance<TapNote>();
                controller.noteSlots[x, y].addNote(note, color, type);
            } else if (type==33||type==44){
                GenericNote note = controller.swipeNote.Instance<SwipeNote>();
                controller.noteSlots[x, y].addNote(note, color, type);
            } else if (type==21||type==22){
                GenericNote note = controller.holdNote.Instance<HoldNote>();
                controller.noteSlots[x, y].addNote(note, color, type);
                //GD.Print("hold note moment");
            } else{
                GD.Print("bro u messed up the note type");
            }
            
        }
    }

    // choosing note and lane

    public void setNotes(string songCode){
        string path = "res://songs/"+songCode+"/"+songCode+"Notes.txt";
		GD.Print(path);
		File file = new File();
		if(file.FileExists(path)){
			file.Open(path, File.ModeFlags.Read);
			while(!file.EofReached()){
				string line = file.GetLine();
                if (line != ""){
                    int x = Convert.ToInt32(line.Substring(0,1));
                    int y = Convert.ToInt32(line.Substring(2,3));
                    string color = line.Substring(6, 1);
                    int type = Convert.ToInt32(line.Substring(8,2));
                    //GD.Print("x: " + x + " y: " + y + " color: " + color + " type" + type);
                    //placeNote(x, y);
                    placeNote(x, y, color, type); 
                }
			}
			file.Close();
		} else {
			GD.Print("not found: " + path);
		}
    }

    // drawing notes starts from here

    public int isOnScreen(int x, int y){ // returns 0 if below, 1 if on/near, 2 if above
        float pos = controller.scrollPos + controller.noteSlots[x,y].Position.y;
        if(pos<-300){
            return 2; // above screen
        } else if (pos>controller.displaySize.y+900){
            return 0;
        }
        return 1;
    }

    public void drawNotes(){
        var noteSlots = controller.noteSlots;
        for(int y=0;y<grid.y;y++){
            for(int x=0; x<controller.keyCount;x++){
                if(noteSlots[x,y].full && noteSlots[x,y].note.active == true && isOnScreen(x, y)==1){
                    if(noteSlots[x,y].noteType < 30){ // tap notes & hold notes
                        drawTapNote(x, y, noteSlots);
                    } else if(noteSlots[x,y].noteType == 33){ // right swipe
                        drawRSwipeNote(x, y, noteSlots);
                    } else if(noteSlots[x,y].noteType == 44){ // left swipe
                        drawLSwipeNote(x, y, noteSlots);
                    }
                }
                else if(noteSlots[x,y].full && noteSlots[x,y].note.active == false && isOnScreen(x, y)==1){
                    drawDeadNote(x, y, noteSlots);
                } else if (isOnScreen(x, y)==2){
                    return;
                }
            }
        }
        
    }

    public void drawTapNote(int x, int y, NoteSlot[,] noteSlots){
        float space = controller.editor.space;
        Vector2 position = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
        Color color = getColor(x, y);
        DrawCircle(position, 70, color);
        DrawCircle(position, 50, new Color(0.9f, 0.9f, 0.9f));
    }

    public void drawRSwipeNote(int x, int y, NoteSlot[,] noteSlots){
        float space = controller.editor.space;
        Vector2 position = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
        Color color = getColor(x, y);
        Vector2[] points = new  Vector2[3];

        points[0] = new Vector2(position.x+77, position.y); // right vertex
        points[1] = new Vector2(position.x-57, position.y+83); // top left vertex
        points[2] = new Vector2(position.x-57, position.y-83); // top left vertex
        DrawColoredPolygon(points, color, null, null, null, true); // outer triangle

        points[0] = new Vector2(position.x+50, position.y); // right vertex
        points[1] = new Vector2(position.x-45, position.y+60); // top left vertex
        points[2] = new Vector2(position.x-45, position.y-60); // top left vertex
        DrawColoredPolygon(points, new Color(0.9f, 0.9f, 0.9f), null, null, null, true); // smol triangle
    }

    public void drawLSwipeNote(int x, int y, NoteSlot[,] noteSlots){
        float space = controller.editor.space;
        Vector2 position = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
        Color color = getColor(x, y);
        Vector2[] points = new  Vector2[3];

        points[0] = new Vector2(position.x-78, position.y); // left vertex
        points[1] = new Vector2(position.x+57, position.y+83); // top right vertex
        points[2] = new Vector2(position.x+57, position.y-83); // top right vertex
        DrawColoredPolygon(points, color, null, null, null, true); // outer triangle

        points[0] = new Vector2(position.x-50, position.y); // left vertex
        points[1] = new Vector2(position.x+45, position.y+60); // top right vertex
        points[2] = new Vector2(position.x+45, position.y-60); // top right vertex
        DrawColoredPolygon(points, new Color(0.9f, 0.9f, 0.9f), null, null, null, true); // smol triangle
    }

    public void drawDeadNote(int x, int y, NoteSlot[,] noteSlots){
        float space = controller.editor.space;
        Vector2 position = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
        Color color = getColor(x, y);
        DrawCircle(position, 40, color);
        DrawCircle(position, 20, new Color(0.9f, 0.9f, 0.9f));
    }

    public Color getColor(int x, int y){
		switch(controller.noteSlots[x, y].color){
			case "g":
				return new Color(0.1f, 0.6f,0.1f);
			case "p":
				return new Color(0.8f, 0.1f,0.9f);
			case "r":
				return new Color(0.9f, 0.1f,0.1f);
			case "y":
				return new Color(0.77f, 0.77f,0.1f);
		}
        return new Color(0.1f, 0.1f,0.1f);
	}

    public void drawNoteConnections(){
        var noteSlots = controller.noteSlots;
        for(int y=0;y<grid.y;y++){
            for(int x=0;x<controller.keyCount;x++){
                if(noteSlots[x, y].noteType!=0 && isOnScreen(x, y)==1){
                    Vector2 nextSlot = findNextNote(noteSlots[x,y], y);
                    if (nextSlot.x!=-1){
                        Vector2 slot1Pos = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
                        int slot2x = (int) (nextSlot.x*space+space);
                        int slot2y = (int) (controller.scrollPos + noteSlots[(int)nextSlot.x,(int)nextSlot.y].Position.y);
                        Vector2 slot2Pos = new Vector2(slot2x, slot2y);
                        DrawLine(slot1Pos, slot2Pos, new Color(0.9f, 0.9f, 0.9f), 40f, true);
                    }
                }
            }
        }
    }

    public Vector2 findNextNote(NoteSlot slot,  int y){
        var noteSlots = controller.noteSlots;
        for(int y1=y+1;y1<y+5&&y1<controller.vSlotCnt;y1++){
            for(int x1=0;x1<4;x1++){
                if(slot.color==noteSlots[x1,y1].color){
                    return new Vector2(x1, y1);
                }
            }
        }
        return new Vector2(-1, -1);
    }

    public void drawHoldNotes(int inout){
        var noteSlots = controller.noteSlots;
        for(int y=0;y<grid.y;y++){
            for(int x=0;x<controller.keyCount;x++){
                if((noteSlots[x, y].noteType==22 || noteSlots[x, y].noteType==21) && isOnScreen(x, y)==1){ // hold notes is 22/21
                    Vector2 nextSlot = findNextHoldNote(noteSlots[x,y], y);
                    if (nextSlot.x!=-1){
                        Vector2 slot1Pos = new Vector2(x*space+space, controller.scrollPos + noteSlots[x,y].Position.y);
                        int slot2x = (int) (nextSlot.x*space+space);
                        int slot2y = (int) (controller.scrollPos + noteSlots[(int)nextSlot.x,(int)nextSlot.y].Position.y);
                        Vector2 slot2Pos = new Vector2(slot2x, slot2y);
                        if(inout == 1){ // draw white line, after colored one
                            DrawLine(slot1Pos, slot2Pos, new Color(0.9f, 0.9f, 0.9f), 40f, true);
                        } else {
                            DrawLine(slot1Pos, slot2Pos, getColor(x,y), 80f, true);
                        }
                    }
                } else if(isOnScreen(x, y)==2){
                    return;
                }
            }
        }
    }

    public Vector2 findNextHoldNote(NoteSlot slot, int y){
        var noteSlots = controller.noteSlots;
        for(int y1=y+1;y1<y+5&&y1<controller.vSlotCnt;y1++){
            for(int x1=0;x1<4;x1++){
                if(noteSlots[x1, y1].noteType==slot.noteType && slot.color==noteSlots[x1,y1].color){ // hold notes is 22/21
                    return new Vector2(x1, y1);
                }
            }
        }
        return new Vector2(-1, -1);
    }



}
