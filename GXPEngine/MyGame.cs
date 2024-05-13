using GXPEngine;
using System;
using System.Collections.Generic;

public class MyGame : Game
{
    static MyGame myGame;
    public static Random random = new Random();
    public static MyGame GetGame() { return myGame; }
    static void Main()
    {
        // calling the initiation the MyGame and save it as a static
        myGame = new MyGame();
        Console.WriteLine("Finished creating MyGame");
        Console.WriteLine("--- Starting MyGame");
        myGame.Start();
    }

    public static List<Planet> planets;
    public Player player;
    public PlayerCamera camera;
    public Camera UICamera;
    public UI UI;

    public MyGame() : base(1920, 1080, false, true)
    {
        //Initiate the MyGame
        planets = new List<Planet>();
    }

    void Update()
    {
        if (Input.GetKeyDown(Key.R))
        {
            LevelHandler.UnloadScene();
        }

        CheckNulls();

        // save all old positions in the colliders
        Vec2CollisionManager.UpdateOldPositions();

        // request the GravityHandler to manage all gravity
        Vec2GravityHandler.handleGravity();

        // requests the collision manager to resolve all collisions
        Vec2CollisionManager.HandleCollisions();

        // Step all objects
        foreach (Planet p in planets)
        {
            p.Step();
        }
        player.Step();
        camera.Step();

        // Update all screen positions
        foreach (Planet p in planets)
        {
            p.UpdateScreenPosition();
        }
        player.UpdateScreenPosition();
        camera.UpdateScreenPosition();

        UI.Draw();
    }

    void CheckNulls()
    {
        if (!LevelHandler.levelLoaded)
        {
            Console.WriteLine("- Found Level is not be loaded");
            LevelHandler.LoadScene("data/map/TestPrototype.tmx");
        }

        if (camera == null)
        {
            // create the camera that follows the player
            Console.WriteLine("- Found camera to be NULL");
            camera = new PlayerCamera(0, 0, 1920, 1080, player);
            AddChild(camera);
            Console.WriteLine("Created camera");
        }

        if (UI == null)
        {
            Console.WriteLine("- Found UI to be NULL");
            UI = new UI();
            UI.SetXY(-20000, -20000);
            AddChild(UI);
            Console.WriteLine("Created UI");
        }

        if (UICamera == null)
        {
            // create the camera that shows the UI
            Console.WriteLine("- Found UI camera to be NULL");
            UICamera = new Camera(0, 0, 1920, 1080, false);
            UICamera.SetXY(-20000 + (1920 / 2), -20000 + (1080 / 2));
            AddChild(UICamera);
            Console.WriteLine("Created UI camera");
        }
    }
}