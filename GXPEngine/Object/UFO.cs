using GXPEngine;
using GXPEngine.Core;
using System;
using TiledMapParser;

public class UFO : AnimationSprite
{
    bool keySpawned = false;
    KeyCollectible key;
    AnimationSprite overlay;
    public UFO(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        overlay = new AnimationSprite(filename, cols, rows);
        overlay.SetFrame(1);
        overlay.SetOrigin(width / 2, height / 2);
        AddChild(overlay);
        SetOrigin(width / 2, height / 2);
        key = new KeyCollectible("data/map/key0.png", 1, 1, new TiledObject());
        key.locked = true;
        MyGame.GetGame().AddChild(key);
        key.gCollider._collider._position = new Vec2(obj.X+20,obj.Y-140);
    }

    void Update()
    {
        Player player = MyGame.GetGame().player;
        if (player == null) { return; }
        Vec2 direction = player.gCollider._collider._position - new Vec2(x, y);
        float distance = direction.Length();

        bool correct = true;
        if (MyGame.UFOButtons.Count == 3)
            foreach (UFOButton b in MyGame.UFOButtons)
            {
                correct = correct && b.isCorrect();
            }
        if (correct && !keySpawned)
        {
            overlay.visible = false;
            key.locked = false;
            Console.WriteLine("- Puzzle key unlocked");
            keySpawned = true;
        }
    }
}

