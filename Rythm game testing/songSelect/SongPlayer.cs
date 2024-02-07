using Godot;
using System;

public class SongPlayer : Control
{
    public AudioStreamPlayer musicPlayer;
	public Timer timer;
	public AudioEffectSpectrumAnalyzerInstance spectrum;
	public const double vuCount = 16;
	public const double freqMax = 11050.0;
	public override void _Ready()
	{
		musicPlayer = GetNode<AudioStreamPlayer>("prevPlayer");
		timer = GetNode<Timer>("prevTimer");
		//musicPlayer = GetNode<AudioStreamPlayer>("/root/songSelect/scene/SongPlayer/prevPlayer");
		//timer = GetNode<Timer>("/root/songSelect/scene/SongPlayer/prevTimer");
	}

	public void songChange(songCard newSong){ // idk man just call this when u change the song thing
	//GD.Print(newSong.songAudio);
		//GD.Print(newSong.songAudio);
		musicPlayer.Stream = newSong.songAudio; // setting song into musicPlayer
		
		float startTime = newSong.previewTime; // getting the start time for the preview
		
		float volumeConversion = ((float) 50) * 0.01F; // setting volume by getting bus index & converting volume variable to Db
		float volume = (float)Math.Log(volumeConversion) * 8.685889f;
		int busIndex = AudioServer.GetBusIndex("Master");
		AudioServer.SetBusVolumeDb(busIndex, volume);
		//spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(busIndex, (int)volume);
		
		musicPlayer.Play(startTime);
		
		timer.Start(10); // starts timer for audio preview to stop 
	}
	
	public override void _Process(float delta){
		//float curVol = AudioServer.GetBusVolumeDb(AudioServer.GetBusIndex("Master"));
		//GD.Print(curVol + " " + delta);
		//bounce();
	}
	private void stopPlaying(){ // stops any song if it is played for 10 seconds
		musicPlayer.Stop();
	}
	public void _on_Path2D_songChangeEventHandler(songCard newSong){
		//GD.Print(newSong.songTitle);
		songChange(newSong);
	}

	public void bounce(){
		for (int i = 1; i < vuCount+1; i++){
			float hz = (float)(i * freqMax / vuCount);
			float magnitude = spectrum.GetMagnitudeForFrequencyRange(0, hz).Length();
			GD.Print(magnitude);
		}
	}
}
