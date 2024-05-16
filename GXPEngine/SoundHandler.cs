using GXPEngine;
using GXPEngine.Core;
using System;
using System.Media;
public class SoundHandler
{
    public static SoundHandler MusicTrack = new SoundHandler("data/sound/Soundtrack.wav", 1f, 0, true);
    public static SoundHandler BlackHoleAmbiance = new SoundHandler("data/sound/Blackhole_001.wav", 1f, 0, true);
    public static SoundHandler ButtonPress = new SoundHandler("data/sound/Button_001.wav", 1f, 0, false);
    public static SoundHandler Death = new SoundHandler("data/sound/Death_001.wav", 1f, 0, false);
    public static SoundHandler UFOOpen = new SoundHandler("data/sound/Door-Opening_001.wav", 1f, 0, false);
    public static SoundHandler Footstep1 = new SoundHandler("data/sound/Footstep_01_001.wav", .8f, 0, false);
    public static SoundHandler Footstep2 = new SoundHandler("data/sound/Footstep_02_001.wav", .8f, 0, false);
    public static SoundHandler Footstep3 = new SoundHandler("data/sound/Footstep_03_001.wav", .8f, 0, false);
    public static SoundHandler Pickup = new SoundHandler("data/sound/Item-Pickup_001.wav", 1f, 0, false);
    public static SoundHandler Jump = new SoundHandler("data/sound/Jump_001.wav", 1f, 0, false);
    public static SoundHandler Boost = new SoundHandler("data/sound/Boost_001.wav", .6f, 0, true);
    public static SoundHandler Landing = new SoundHandler("data/sound/Landing_001.wav", .7f, 0, false);
    public static SoundHandler MenuClick = new SoundHandler("data/sound/Menu-Click_001.wav", 1f, 0, false);
    public static SoundHandler MenuHover = new SoundHandler("data/sound/Menu-Hover_001.wav", 1f, 0, false);
    public static SoundHandler RocketFlyaway = new SoundHandler("data/sound/Rocket_001.wav", 1f, 0, false);


    Sound storedSound;
    float defaultVolume;
    uint defaultChanel;

    public SoundHandler(string fileName, float defaultVolume, uint defaultChanel, bool loop = false)
    {
        storedSound = new Sound(fileName, loop);
        this.defaultVolume = defaultVolume;
        this.defaultChanel = defaultChanel;
    }

    public void Play()
    {
        Play(defaultVolume);
    }

    public void Play(float volume)
    {
        Play(volume, defaultChanel);
    }

    public void Play(float volume, uint chanel)
    {
        storedSound.Play(false, chanel, volume, 0);
        //Console.WriteLine("- Started playing sound:" + storedSound);
    }

    public void SetDefaultVolume(float defaultVolume) { this.defaultVolume = defaultVolume;  }
}
