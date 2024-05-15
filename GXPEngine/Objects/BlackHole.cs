using GXPEngine;
using System;
using TiledMapParser;

public class BlackHole : Planet
{
    public BlackHole(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj, false)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldBlackHoleDensity);
        Console.WriteLine("BlackHole created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary());
    }

    public override void Step()
    {
        Player player = MyGame.GetGame().player;
        base.Step();
        Vec2 direction = player.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();

        if (distance < radius+player.radius) { MyGame.GetGame().Die(); }
    }
}
