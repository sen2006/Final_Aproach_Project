using GXPEngine;
using TiledMapParser;

public class Rocket : AnimationSprite
{
    public Rocket(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
    }

    void Update()
    {
        if (MyGame.collectedKeys.Contains(1) && 
            MyGame.collectedKeys.Contains(2) && 
            MyGame.collectedKeys.Contains(3) && 
            MyGame.collectedKeys.Contains(4) &&
            Input.GetMouseButtonDown(0) && HitTestPoint(MyGame.GetGame().player.x, MyGame.GetGame().player.y))
        {
            MyGame.GetGame().currentLevelFile = "data/map/TestPrototype.tmx";
            LevelHandler.Reload();
        }
    }
}

