using Godot;
using System;
using System.Runtime.InteropServices;

public class songBanner : Sprite
{
//	public int selected = 0;
	public override void _Ready()
	{
		this.Texture = ResourceLoader.Load("res://gameSprites/SongSelect/sbMed.png") as Texture;
	}

	public void _on_Path2D_update_bannerEventHandler(songCard newSongCard){
		//GD.Print("Name = " + newSongCard.songTitle + "     Path = " + newSongCard.diffBanner);
		this.Texture = ResourceLoader.Load(newSongCard.diffBanner) as Texture;
	}
}
