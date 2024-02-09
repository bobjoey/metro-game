using Godot;
using System;

public class SongPlayer : Control
{
	[Signal]
	public delegate void soundBounceEventHandler(float magnitude);
	[Export]
	private int previewLength = 10;
    public AudioStreamPlayer musicPlayer;
	public songCard song;
	public Timer previewTimer;
	public Timer fadeTimer;
	private int fadeAudio = 0;
	private const float fadeInTime = 0.5f;
	private const float fadeOutTime = 0.7f;
	public float currentVolume = 50;
	public int busIndex = AudioServer.GetBusIndex("Master");
	public AudioEffectSpectrumAnalyzerInstance spectrum;
	public const double vuCount = 16;
	public const double freqMax = 11050.0;
	private float timeAvg;
	private float metroNome;
	private float timePassed = 0;
	public override void _Ready()
	{
		//song = songsManager.songsList[0];
		musicPlayer = GetNode<AudioStreamPlayer>("prevPlayer");
		previewTimer = GetNode<Timer>("prevTimer");
		fadeTimer = GetNode<Timer>("fadeTimer");
	}	

	public void songChange(songCard newSong){ // idk man just call this when u change the song thing
		//musicPlayer.Stop();
		song = newSong;
		fadeAudio = 1;
		fadeTimer.Start(fadeInTime);
	}
	
	public override void _Process(float delta){
		timePassed+=delta;
		if(song != null)metroNome = 1/((float)song.bpm/60f);
		if(fadeAudio!=0){
			fade();
		}
		//metroNome/=8;
		//if(timePassed >= metroNome){
			spectrum = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(AudioServer.GetBusIndex("Master"), 0);
			bounce();
			timePassed = 0;
		//}
	}

	private void fade(){
		float amount = (fadeTimer.TimeLeft)/fadeInTime;
		if(fadeAudio==2){
			amount = (fadeOutTime-fadeTimer.TimeLeft)/fadeOutTime;
		}
		// setting volume by getting bus index & converting volume variable to Db
		float volume = (float)Math.Log((currentVolume * 0.01f * amount)) * 8.685889f;
		AudioServer.SetBusVolumeDb(busIndex, volume);
	}

	private void fadeTime(){
		if(fadeAudio==1){
			fadeAudio=2;
			musicPlayer.Stream = song.songAudio;
			musicPlayer.Play(song.previewTime);
			fadeTimer.Start(fadeOutTime);
		} else if (fadeAudio==2){
			//setting volume by getting bus index & converting volume variable to Db
			float volume = (float)Math.Log(currentVolume * 0.01f) * 8.685889f;
			AudioServer.SetBusVolumeDb(busIndex, volume);
			previewTimer.Start(previewLength-fadeOutTime);
			fadeAudio = 0;
		}
	}

	private void stopPlaying(){ // stops any song if it is played for 10 seconds
		fadeAudio = 1;
		fadeTimer.Start(fadeInTime);
	}
	public void _on_Path2D_songChangeEventHandler(songCard newSong){
		//GD.Print(newSong.songTitle);
		songChange(newSong);
	}

	public void bounce(){
		for (int i = 1; i < vuCount+1; i++){
			float hz = (float)(i * freqMax / vuCount);
			float magnitude = spectrum.GetMagnitudeForFrequencyRange(0, 150).Length();
			this.EmitSignal("soundBounceEventHandler", magnitude);
		}
	}
}
