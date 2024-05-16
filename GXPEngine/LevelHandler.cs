using GXPEngine;
using System;
using TiledMapParser;

public static class LevelHandler
{
    public static bool levelLoaded = false;

    public static void LoadScene(string filename, bool loadLights = false, bool loadVignette = false, bool setColors = false, bool loadUnlit = false)
    {
        Console.WriteLine("-- Starting Level loading (" + filename + ")");
        TiledLoader loader = new TiledLoader(filename);
        loader.autoInstance = true;

        Console.WriteLine("- Loading Background");
        loader.addColliders = false;
        Pivot background = new Pivot();
        MyGame.GetGame().AddChild(background);
        loader.rootObject = background;
        loader.LoadImageLayers(1);
        if (setColors)
        {
            ((Sprite)background.GetChildren()[0]).SetColor(0.5f, 0.5f, 0.5f);
        }

        Console.WriteLine("- Loading main layer");
        // main layer, lit:
        Pivot mainlayer = new Pivot();
        MyGame.GetGame().AddChild(mainlayer);
        loader.rootObject = mainlayer;
        loader.addColliders = true;
        loader.LoadTileLayers(0);
        loader.LoadObjectGroups(0); // contains the player and other objects
        loader.addColliders = false;

        // lights:
        if (loadLights)
        {
            Console.WriteLine("- Loading lights layer");
            loader.LoadObjectGroups(1); // contains normal lights
        }

        Console.WriteLine("- Loading background unlit");
        // background, unlit:
        if (loadUnlit)
        {
            Pivot unlit = new Pivot();
            MyGame.GetGame().AddChild(unlit);
            loader.rootObject = unlit;
            loader.LoadImageLayers(0);
            ((Sprite)unlit.GetChildren()[0]).blendMode = BlendMode.FILLEMPTY;
        }

        loader.rootObject = MyGame.GetGame();
        if (loadLights)
        {
            Console.WriteLine("- Loading volumetriv lights");
            loader.LoadObjectGroups(2); // "volumetric" lighting (additive)
        }

        Console.WriteLine("- Loading foreground");
        // foreground:
        // (A SpriteBatch is used to easily set the color of all sprites with one command - see SpriteBatch.cs for more info)
        SpriteBatch foreground = new SpriteBatch();
        MyGame.GetGame().AddChild(foreground);
        loader.rootObject = foreground;
        loader.LoadTileLayers(1);
        foreground.Freeze();
        if (setColors)
        {
            foreground.SetColor(0.4f, 0.4f, 0.4f);
        }

        loader.rootObject = MyGame.GetGame();
        if (loadVignette)
        {
            Console.WriteLine("- Loading vignette");
            loader.LoadObjectGroups(3); // vignette (multiply)
        }
        levelLoaded = true;
        Console.WriteLine("-- Finished loading level");
    }

    public static void UnloadScene()
    {
        Console.WriteLine("-- Destroying level");

        Console.WriteLine("- Destroying children of MyGame");
        foreach (GameObject child in MyGame.GetGame().GetChildren())
        {
            Console.WriteLine("Destroying: "+child);
            child.LateDestroy();
        }

        MyGame game = MyGame.GetGame();

        Console.WriteLine("- Clearing planet list");
        MyGame.planets.Clear();

        Console.WriteLine("- Clearing collected key list");
        MyGame.collectedKeys.Clear();

        Console.WriteLine("- Clearing collicer list");
        Vec2CollisionManager.colliders.Clear();

        Console.WriteLine("- Clearing gravity collicer list");
        Vec2GravityHandler.gravityObjects.Clear();

        Console.WriteLine("- Clearing UFO-buttons list");
        MyGame.UFOButtons.Clear();

        Console.WriteLine("- Setting player to NULL");
        game.player = null;

        Console.WriteLine("- Setting camera to NULL");
        game.camera = null;

        Console.WriteLine("- Setting UI to NULL");
        game.UI = null;

        Console.WriteLine("- Setting UI-camera to NULL");
        game.UICamera = null;

        Console.WriteLine("- Setting background to NULL");
        game.background = null;

        Console.WriteLine("- Setting background-camera to NULL");
        game.backgroundCamera = null;

        

        levelLoaded = false;
        Console.WriteLine("-- Finished destroying level");
    }

    internal static void Reload()
    {
        UnloadScene();
        LoadScene(MyGame.GetGame().currentLevelFile);
    }
}

