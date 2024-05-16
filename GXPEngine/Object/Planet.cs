using GXPEngine;
using System;
using TiledMapParser;

public class Planet : GravityObject
{
    
    public float poisonRadius;
    public float poisonDamage;
    public float touchDamage;

    public Planet(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlanetDensity);
        MyGame.planets.Add(this);

        poisonRadius = obj.GetFloatProperty("poisonRadius", 0);
        poisonDamage = obj.GetFloatProperty("poisonDamage", 1);
        touchDamage = obj.GetFloatProperty("touchDamage", 0);

        Console.WriteLine("Planet created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary()+ ", mass:" + gCollider._collider.mass + ", density:" + obj.GetFloatProperty("density", Settings.defauldPlanetDensity));
    }
    public Planet(string filename, int cols, int rows, TiledObject obj, bool announceCreation) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlanetDensity);
        MyGame.planets.Add(this);

        if(announceCreation) Console.WriteLine("Planet created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary()+ ", mass:"+gCollider._collider.mass +", density:"+ obj.GetFloatProperty("density", Settings.defauldPlanetDensity));
    }
}
