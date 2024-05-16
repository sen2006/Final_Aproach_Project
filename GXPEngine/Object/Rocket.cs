using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Rocket : AnimationSprite
{
    public Rocket(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        SetOrigin(width/2, height/2);
    }

    void Update()
    {
        Player player = MyGame.GetGame().player;
        if (player == null) return;
        Vec2 direction = player.gCollider._collider._position - new Vec2(x,y);
        float distance = direction.Length();

        if (distance <= width/2 + player.radius &&
            MyGame.collectedKeys.Contains(1) && 
            MyGame.collectedKeys.Contains(2) && 
            MyGame.collectedKeys.Contains(3) && 
            MyGame.collectedKeys.Contains(0))
        {
            SoundHandler.RocketFlyaway.Play();
            MyGame.GetGame().currentLevelFile = "data/map/WinMenu.tmx";
            LevelHandler.Reload();
        }
    }
}

