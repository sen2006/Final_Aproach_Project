using GXPEngine;
using System;
using TiledMapParser;

public class Planet : GravityObject
{
    EasyDraw easyDraw;
    public Planet (string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        //x = (float)MyGame.random.NextDouble() * 1920;
        //y = (float)MyGame.random.NextDouble() * 1080;


        gCollider._collider._position = new Vec2(x-radius,y-radius);
        easyDraw = new EasyDraw(Mathf.Ceiling(radius*2) * 2, Mathf.Ceiling(radius*2) * 2);
        easyDraw.SetOrigin(radius*2, radius*2);
        MyGame.planets.Add(this);

        Console.WriteLine("planet loaded at " + x +"-"+y);
    }

    public override void Draw()
    {
        easyDraw.Clear(0,0,0,0);
        //if (!HasChild(easyDraw)) AddChild(easyDraw);
        easyDraw.Fill(255, 255, 255, 100);
        easyDraw.StrokeWeight(1);
        easyDraw.Stroke(255, 0, 0);
        easyDraw.Ellipse(radius*2, radius*2, easyDraw.width, easyDraw.height);
    }
}
