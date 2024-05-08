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
    public static Player player;

    UI UI;

    public MyGame() : base(1920, 1080, false, true)
    {
        planets = new List<Planet>();

        /*
        planet = new Planet(100, 1);
        new Planet(70+(float)random.NextDouble()*150, 1);
        new Planet(70 + (float)random.NextDouble() * 150, 1);
        new Planet(70 + (float)random.NextDouble() * 150, 1);
        */
        //foreach (Planet p in planets) { AddChild(p); }

        new Planet("data/map/Large Planet.png", 1, 1, new TiledMapParser.TiledObject());

        foreach (Planet planet in planets) { AddChild(planet); }

        UI = new UI();
        AddChild(UI);
    }

    void Update()
    {
        if (!LevelHandler.levelLoaded)
        {
            //LevelHandler.LoadScene("data/map/TestPrototype.tmx");
            //LevelHandler.LoadScene("data/map/TiledTest.tmx");
        }

        if (player == null)
        {
            new Player("data/map/1 tile objects_character_small_astroid_key.png", 1,1,new TiledMapParser.TiledObject());
        }

        Vec2CollisionManager.UpdateOldPositions();

        //planet.gCollider._collider._position = new Vec2(Input.mouseX, Input.mouseY);

        Vec2GravityHandler.handleGravity();

        // requests the collision manager to resolve all collisions
        Vec2CollisionManager.HandleCollisions();

        player.Step();

        foreach (Planet p in planets) { p.UpdateScreenPosition(); }

        player.UpdateScreenPosition();
        //objDraw.Ellipse(objDraw.width/2, objDraw.height/2, objDraw.width, objDraw.height);

        UI.Draw();

    }
}