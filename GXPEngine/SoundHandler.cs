using GXPEngine;
public class SoundHandler
{


    //public static SoundHandler shooting_sound = new SoundHandler("assets/sound/Shotgun firing.wav", 0.4f, 0);


    Sound storedSound;
    float defaultVolume;
    uint defaultChanel;

    public SoundHandler(string fileName, float defaultVolume, uint defaultChanel)
    {
        storedSound = new Sound(fileName);
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
    }
}
