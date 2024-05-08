using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;

public class MyGame : Game
{
    public static MyGame myGame;
    public static Random random = new Random();
    public static MyGame GetGame() { return myGame;}
    static void Main()
    {
        myGame = new MyGame();
        myGame.Start();
    }

    public static List<Planet> planets;


    Planet planet;
    Player obj;

    UI UI;

    public MyGame() : base(1920, 1080, false, true)
    {
        planets = new List<Planet>();


        planet = new Planet(100, 1);
        new Planet(70+(float)random.NextDouble()*150, 1);
        new Planet(70 + (float)random.NextDouble() * 150, 1);
        new Planet(70 + (float)random.NextDouble() * 150, 1);
        new Planet(70 + (float)random.NextDouble() * 150, 1);


        obj = new Player(20);
        AddChild(obj);

        foreach (Planet p in planets) { AddChild(p); }

        UI = new UI();
        AddChild(UI);
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

        UI.Draw();

    }
}