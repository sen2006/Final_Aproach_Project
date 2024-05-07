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
    EasyDraw objDraw;

    public MyGame() : base(1920, 1080, false, true)
    {
        planets = new List<Planet>();

        planet = new Planet(new Vec2(600, 400), 100, 1);
        AddChild(planet);

        obj = new Player(new Vec2(200, 400), 20);
        AddChild(obj);
        objDraw = new EasyDraw(60,60);
        objDraw.SetOrigin(30, 30);
        AddChild(objDraw);
    }

    void Update()
    {

        Vec2CollisionManager.UpdateOldPositions();

        planet.gCollider._collider._position = new Vec2(Input.mouseX, Input.mouseY);

        Vec2GravityHandler.handleGravity();

        // requests the collision manager to resolve all collisions
        Vec2CollisionManager.HandleCollisions();

        obj.Step();

        planet.UpdateScreenPosition();
        obj.UpdateScreenPosition();

        objDraw.SetXY(obj.x, obj.y);
        //objDraw.Ellipse(objDraw.width/2, objDraw.height/2, objDraw.width, objDraw.height);

        

    }
}