using GXPEngine;
using TiledMapParser;

public class QuitButton : AnimationSprite
{
    public QuitButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && HitTestPoint(Input.mouseX, Input.mouseY))
        {
            MyGame.GetGame().Destroy();
        }
    }
}

