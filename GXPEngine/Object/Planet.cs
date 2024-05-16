using GXPEngine;
using System;
using TiledMapParser;

public class Planet : GravityObject
{
    EasyDraw easyDraw;

    public Planet(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlanetDensity);

        easyDraw = new EasyDraw(Mathf.Ceiling(radius * 2) * 2, Mathf.Ceiling(radius * 2) * 2);
        easyDraw.SetOrigin(radius * 2, radius * 2);
        MyGame.planets.Add(this);

        Console.WriteLine("Planet created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary()+ ", mass:" + gCollider._collider.mass + ", density:" + obj.GetFloatProperty("density", Settings.defauldPlanetDensity));
    }
    public Planet(string filename, int cols, int rows, TiledObject obj, bool announceCreation) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlanetDensity);

        easyDraw = new EasyDraw(Mathf.Ceiling(radius * 2) * 2, Mathf.Ceiling(radius * 2) * 2);
        easyDraw.SetOrigin(radius * 2, radius * 2);
        MyGame.planets.Add(this);

        if(announceCreation) Console.WriteLine("Planet created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary()+ ", mass:"+gCollider._collider.mass +", density:"+ obj.GetFloatProperty("density", Settings.defauldPlanetDensity));
    }

    public override void Draw()
    {
        easyDraw.Clear(0, 0, 0, 0);
        //if (!HasChild(easyDraw)) AddChild(easyDraw);
        easyDraw.Fill(255, 255, 255, 100);
        easyDraw.StrokeWeight(1);
        easyDraw.Stroke(255, 0, 0);
        easyDraw.Ellipse(radius * 2, radius * 2, easyDraw.width, easyDraw.height);
    }
}
