using Godot;
using System;
using System.Runtime.InteropServices;

public class songBanner : AnimatedSprite
{
	public override void _Ready()
	{
		this.Animation = songsManager.songsList[0].diffBanner;
	}
	public string ToTime(string length){
		return length[0] + ":" + length[1]+length[2];
	}
	public void _on_Path2D_update_bannerEventHandler(songCard newSongCard){
		//GD.Print("Name = " + newSongCard.songTitle + "     Path = " + newSongCard.diffBanner);
		//this.Texture = ResourceLoader.Load(newSongCard.diffBanner) as Texture;
		this.Animation = newSongCard.diffBanner;
		GetNode<RichTextLabel>("TitleLabel").BbcodeText = "[center]" + newSongCard.songTitle + "[/center]";
		GetNode<RichTextLabel>("AuthorLabel").BbcodeText = "[center]" + newSongCard.songAuthor + "[/center]";
		GetNode<RichTextLabel>("LengthLabel").BbcodeText = "[center]" + ToTime(newSongCard.songLength.ToString()) + "[/center]";
	}
}
