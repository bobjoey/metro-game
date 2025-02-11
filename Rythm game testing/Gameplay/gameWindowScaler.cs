using Godot;
using System;

public class gameWindowScaler : Control
{
	//int[] screenSize = new int[2];
	int curHeight;
	int curWidth;
	public override void _Ready()
	{
		Vector2 screenSize = OS.GetScreenSize();
		int curHeight = (int)ProjectSettings.GetSetting("display/window/size/height");
		int curWidth = (int)ProjectSettings.GetSetting("display/window/size/width");
		float scaleFac = screenSize.y/curHeight/1.38f;
		curHeight = (int)(curHeight*scaleFac);
		curWidth = (int)(curWidth*scaleFac);
		
		// GD.Print("Screen dimensions = " + screenSize.x + ", " + screenSize.y);
		// GD.Print("Game dimensions = " + curWidth + ", " + curHeight);
		// GD.Print("Scale factor = " + scaleFac);
		// GD.Print("New dimensions = " + (int)(curWidth*scaleFac) + ", " + (int)(curHeight*scaleFac));
		Vector2 newSize = new Vector2((float)curWidth, (float)curHeight);
		OS.WindowSize = newSize;
		OS.WindowPosition = new Vector2((screenSize.x-curWidth)/2, 250);
		//ProjectSettings.SetSetting("display/window/size/height",curHeight);
		//ProjectSettings.SetSetting("display/window/size/width",curWidth);
	//	GD.Print(ProjectSettings.GetSetting("display/window/size/height"));
	//	GD.Print(ProjectSettings.GetSetting("display/window/size/width"));
	//	GD.Print(curHeight);
	//	GD.Print(curWidth);
	}

	private void play_button_pressed()
	{
		//GD.Print("Play button pressed");
		//GetTree().ChangeScene("res://game.tscn");
		GetTree().ChangeScene("res://songSelect/songSelect.tscn");
	}
	
	private void settings_button_pressed()
{
	//GD.Print("Settings button pressed");
	GetTree().ChangeScene("res://settings.tscn");
}


}





