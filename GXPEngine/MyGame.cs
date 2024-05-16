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
    public static List<int> collectedKeys;
    public static List<UFOButton> UFOButtons;
    public Player player;
    public PlayerCamera camera;
    public Camera UICamera;
    public Camera backgroundCamera;
    public AnimationSprite background;
    public UI UI;

    public string currentLevelFile = "data/map/MainMenu.tmx";

    public MyGame() : base(1920, 1080, false, true)
    {
        //Initiate the MyGame
        planets = new List<Planet>();
        collectedKeys = new List<int>();
        UFOButtons = new List<UFOButton>();
        SoundHandler.BlackHoleAmbiance.SetDefaultVolume(0);
        SoundHandler.MusicTrack.Play();
        SoundHandler.BlackHoleAmbiance.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(Key.R))
        {
            LevelHandler.UnloadScene();
        }

        if (camera != null)
        {
            if (Input.GetKey(Key.X))
            {
                camera.scale = 4f;
            } else if (Input.GetKey(Key.Z))
            {
                camera.scale = .2f;
            }
            else camera.scale = .9f;
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
        if(player!=null)player.Step();
        if(camera!=null)camera.Step();

        // Update all screen positions
        foreach (Planet p in planets)
        {
            p.UpdateScreenPosition();
        }
        if (player != null) player.UpdateScreenPosition();
        if (camera != null) camera.UpdateScreenPosition();
        if (background!= null) background.Animate(Settings.backgroundAnimationDeltaFrameTime);

        if (UI != null) UI.Draw();
    }

    void CheckNulls()
    {
        if (!LevelHandler.levelLoaded)
        {
            Console.WriteLine("- Found Level is not be loaded");
            LevelHandler.LoadScene(currentLevelFile);
        }

        if (background == null && player != null)
        {
            Console.WriteLine("- Found background to be NULL");
            background = new AnimationSprite("data/map/background.png", 7,1);
            background.SetXY(50000, 50000);
            Console.WriteLine("Created background");
            AddChild(background);
        }

        if (backgroundCamera == null && player != null)
        {
            Console.WriteLine("- Found background camera to be NULL");
            backgroundCamera = new Camera(0, 0, 1920, 1080, false);
            backgroundCamera.SetXY(50000 + (1920 / 2), 50000 + (1080 / 2));
            Console.WriteLine("Created background camera");
            AddChild(backgroundCamera);
        }

        if (camera == null && player!=null)
        {
            // create the camera that follows the player
            Console.WriteLine("- Found camera to be NULL");
            camera = new PlayerCamera(0, 0, 1920, 1080, player);
            AddChild(camera);
            Console.WriteLine("Created camera");
        }

        if (UI == null && player != null)
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

    public void GameOver()
    {
        SoundHandler.Death.Play();
        currentLevelFile = "data/map/gameOverMenu.tmx";
        LevelHandler.Reload();
    }
}