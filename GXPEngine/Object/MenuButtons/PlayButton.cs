using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class PlayButton : AnimationSprite
{
    bool hover = false;

    public PlayButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        Console.WriteLine("Menu play button created at (" + x + "," + y + ")");
    }

    void Update()
    {
        if (HitTestPoint(Input.mouseX, Input.mouseY))
        {
            if (!hover)
            {
                SoundHandler.MenuHover.Play();
                hover = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                SoundHandler.MenuClick.Play();
                MyGame.GetGame().currentLevelFile = "data/map/GameMap.tmx";
                LevelHandler.Reload();
            }
        } else hover = false;
    }
}

