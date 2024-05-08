using GXPEngine;
using System;
using System.Collections.Generic;
using TiledMapParser;

public static class LevelHandler
{
    public static bool levelLoaded = false;

    public static void LoadScene(string filename, bool loadLights = false, bool loadVignette = false, bool setColors = true)
    {
        TiledLoader loader = new TiledLoader(filename);
        loader.autoInstance = true;

        // ------------- Convention (render order):

        // image layer 1 = background, lit
        // tile layer 0: normal tiles, lit (=main layer)
        // object layer 0: normal objects (e.g. player), rendered with main layer

        // object layer 1: lights. BlendMode.LIGHTING

        // image layer 0 = background, unlit: BlendMode.FILLEMPTY (+rendered after normal lights!)

        // object layer 2: "volumetric" lights. BlendMode.ADDITIVE

        // tile layer 1: foreground tiles, unlit (=rendered after lights)

        // object layer 3: darkening (vignette). BlendMode.MULTIPLY

        // background, lit:
        loader.addColliders = false;
        Pivot background = new Pivot();
        MyGame.GetGame().AddChild(background);
        loader.rootObject = background;
        loader.LoadImageLayers(1);
        if (setColors)
        {
            ((Sprite)background.GetChildren()[0]).SetColor(0.5f, 0.5f, 0.5f);
        }

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
            loader.LoadObjectGroups(1); // contains normal lights
        }

        // background, unlit:
        Pivot unlit = new Pivot();
        MyGame.GetGame().AddChild(unlit);
        loader.rootObject = unlit;
        loader.LoadImageLayers(0);
        ((Sprite)unlit.GetChildren()[0]).blendMode = BlendMode.FILLEMPTY;

        loader.rootObject = MyGame.GetGame();
        if (loadLights)
        {
            loader.LoadObjectGroups(2); // "volumetric" lighting (additive)
        }

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
            loader.LoadObjectGroups(3); // vignette (multiply)
        }
        levelLoaded = true;
    }

    public static void UnloadScene()
    {
        foreach (GameObject child in MyGame.GetGame().GetChildren())
        {
            child.LateDestroy();
        }
        levelLoaded = false;

    }
}

