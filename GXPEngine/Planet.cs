using GXPEngine;
using System;

public class Planet : GravityObject
{
    EasyDraw easyDraw;
    public Planet (float radius, float density = 1, bool stationary = true) : base(new Vec2(0, 0), radius, density, stationary)
    {
        x = (float)MyGame.random.NextDouble() * 1920;
        y = (float)MyGame.random.NextDouble() * 1080;



        gCollider._collider._position = new Vec2(x,y);
        easyDraw = new EasyDraw(Mathf.Ceiling(radius) * 2, Mathf.Ceiling(radius) * 2);
        easyDraw.SetOrigin(radius, radius);
        MyGame.planets.Add(this);
    }

    public override void Draw()
    {
        if (!HasChild(easyDraw)) AddChild(easyDraw);
        easyDraw.Fill(255, 255, 255);
        easyDraw.StrokeWeight(1);
        easyDraw.Stroke(255, 0, 0);
        easyDraw.Ellipse(radius, radius, easyDraw.width, easyDraw.height);
    }
}
