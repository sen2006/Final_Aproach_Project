using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class PlayButton : AnimationSprite
{
    public PlayButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && HitTestPoint(Input.mouseX, Input.mouseY))
        {
            MyGame.GetGame().currentLevelFile = "data/map/GameMap.tmx";
            LevelHandler.Reload();
        }
    }
}

