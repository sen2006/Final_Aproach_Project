using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.WebSockets;

public class MyGame : Game
{
    public static MyGame myGame;
    public static Random random = new Random();
    public static MyGame GetGame() { return myGame; }
    static void Main()
    {
        myGame = new MyGame();
        myGame.Start();
    }

    public static List<Planet> planets;

    public Player player;
    public PlayerCamera camera;
    Camera UICamera;

    UI UI;

    public MyGame() : base(1920, 1080, false, true)
    {
        planets = new List<Planet>();

        UI = new UI();
        UI.SetXY(-2000, -2000);
        AddChild(UI);

        UICamera = new Camera(0,0,1920,1080,false);
        UICamera.SetXY(-2000, -2000);
        AddChild(UICamera);
    }

    void Update()
    {
        if (!LevelHandler.levelLoaded)
        {
            LevelHandler.LoadScene("data/map/TestPrototype.tmx");
        }

        if (camera == null)
        {
            camera = new PlayerCamera(0,0,1920,1080,player);
            AddChild(camera);
        }
        SetChildIndex(UICamera, 0);

        Vec2CollisionManager.UpdateOldPositions();

        Vec2GravityHandler.handleGravity();

        // requests the collision manager to resolve all collisions
        Vec2CollisionManager.HandleCollisions();

        player.Step();
        camera.Step();

        foreach (Planet p in planets)
        {
            p.UpdateScreenPosition();
        }
        player.UpdateScreenPosition();

        UI.Draw();

    }
}