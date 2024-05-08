using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;

public class MyGame : Game
{
    public static MyGame myGame;
    public static MyGame GetGame() { return myGame;}
    static void Main()
    {
        myGame = new MyGame();
        myGame.Start();
    }

    public static List<Planet> planets;


    Planet planet;
    Player obj;

    public MyGame() : base(1920, 1080, false, true)
    {
        planets = new List<Planet>();

        planet = new Planet(new Vec2(600, 400), 100, 1);
        new Planet(new Vec2(200, 600), 120, 1);
        new Planet(new Vec2(1300, 300), 120, 1);
        new Planet(new Vec2(600, 800), 120, 1);
        new Planet(new Vec2(400, 1040), 120, 1);


        obj = new Player(new Vec2(200, 400), 20);
        AddChild(obj);

        foreach (Planet p in planets) { AddChild(p); }
    }

    void Update()
    {

        Vec2CollisionManager.UpdateOldPositions();

        planet.gCollider._collider._position = new Vec2(Input.mouseX, Input.mouseY);

        Vec2GravityHandler.handleGravity();

        // requests the collision manager to resolve all collisions
        Vec2CollisionManager.HandleCollisions();

        obj.Step();

        foreach (Planet p in planets) { p.UpdateScreenPosition(); }

        obj.UpdateScreenPosition();
        //objDraw.Ellipse(objDraw.width/2, objDraw.height/2, objDraw.width, objDraw.height);

        

    }
}