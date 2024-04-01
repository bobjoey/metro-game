using Godot;
using System;

public class PunchCard : Sprite
{
    int decBy = 20;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Position = new Vector2(595, -700);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//   public override void _Process(float delta)
//   {
//     while(this.Position.y < 1000){
//         this.Position = new Vector2(this.Position.x, this.Position.y-decBy);
//     }
//     if(decBy > 1){
//         decBy--;
//     }
//   }
}
