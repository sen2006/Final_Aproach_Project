using GXPEngine;
using System;
using TiledMapParser;

public class QuitButton : AnimationSprite
{
    bool hover = false;

    public QuitButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        Console.WriteLine("Menu quit button created at (" + x + "," + y + ")");
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
                MyGame.GetGame().Destroy();
            }
        }
        else hover = false;
    }
}

