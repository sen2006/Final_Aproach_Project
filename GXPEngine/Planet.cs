using GXPEngine;
using System;

public class Planet : GravityObject
{
    EasyDraw easyDraw;
    public Planet (Vec2 position, float radius, float density = 1, bool stationary = true) : base(position, radius, density, stationary)
    {
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
