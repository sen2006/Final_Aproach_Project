using GXPEngine;
using System;
public class SoundHandler
{


    public static SoundHandler track = new SoundHandler("data/sound/Soundtrack.wav", 1f, 0, true);


    Sound storedSound;
    float defaultVolume;
    uint defaultChanel;

    public SoundHandler(string fileName, float defaultVolume, uint defaultChanel, bool loop = false)
    {
        storedSound = new Sound(fileName, loop);
        this.defaultVolume = defaultVolume;
        this.defaultChanel = defaultChanel;
    }

    public void play()
    {
        play(defaultVolume);
    }

    public void play(float volume)
    {
        play(volume, defaultChanel);
    }

    public void play(float volume, uint chanel)
    {
        storedSound.Play(false, chanel, volume, 0);
        Console.WriteLine("- Started playing sound:" + storedSound);
    }
}
