using GXPEngine;
using System;
using TiledMapParser;

public class BlackHole : Planet
{
    public BlackHole(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj, false)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldBlackHoleDensity);
        Console.WriteLine("BlackHole created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary() + ", mass:" + gCollider._collider.mass + ", density:" + obj.GetFloatProperty("density", Settings.defauldPlanetDensity));
    }

    public override void Step()
    {
        this.Animate(Settings.blackHoleAnimationDeltaFrameTime);
        Player player = MyGame.GetGame().player;
        base.Step();
        Vec2 direction = player.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();

        if (distance < radius+player.radius) { player.Damage(10000); }

        SoundHandler.BlackHoleAmbiance.SetDefaultVolume(0);
    }
}