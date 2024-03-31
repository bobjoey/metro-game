using Godot;
using System;
using System.Drawing.Text;
using System.Linq.Expressions;


public class OpenTutorial : Button
{
    private AnimatedSprite menu;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        menu = GetNode<AnimatedSprite>("TutorialSheet");
    }

    public void _on_OpenTutorial_pressed(){
        GetNode<Sprite>("Holder").Visible = false;
        menu.Play();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(menu.Frame == 3){
            menu.Stop();
            menu.Visible = false;
            GetNode<Sprite>("TutorialMenu").Visible = true;
        }
    }

    public void _on_CloseTutorial_pressed(){
        GetNode<Sprite>("Holder").Visible = true;
        GetNode<Sprite>("TutorialMenu").Visible = false;
        menu.Frame = 0;
        menu.Visible = true;
    }
}
