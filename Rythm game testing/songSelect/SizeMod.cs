using Godot;
using System;

public class SizeMod : PathFollow2D
{
	public override void _Process(float delta)
	{
		if(this.Scale > new Vector2(1,1)){
			this.Scale = new Vector2(this.Scale.x*0.99f, this.Scale.y*0.99f);
		}
	}
}
