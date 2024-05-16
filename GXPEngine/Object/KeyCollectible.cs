using GXPEngine;
using System;
using TiledMapParser;

public class KeyCollectible : GravityObject
{
    int ID;
    public bool locked = false;
    public KeyCollectible(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        ID = obj.GetIntProperty("ID", 0);
        Vec2CollisionManager.colliders.Remove(this.gCollider._collider);
        Vec2GravityHandler.gravityObjects.Remove(this.gCollider);
        gCollider = new Vec2GravityCollider(new Vec2BallCollider(radius, obj.GetFloatProperty("density", 0), obj.GetBoolProperty("stationary", false), false));

        gCollider._collider._position = new Vec2(x,y);

        Console.WriteLine("Key created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary() + ", ID:" + ID);
    }

    public void Update()
    {
        base.Step();
        base.UpdateScreenPosition();
        Player player = MyGame.GetGame().player;
        if (player == null) return;
        Vec2 direction = player.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();

        if (distance <= radius + player.radius && !locked)
        {
            Console.WriteLine("- Collected Key ID:" + ID);
            MyGame.collectedKeys.Add(ID);

            string collectedKeys = "[";

            for (int i=0; i<MyGame.collectedKeys.Count; i++)
            {
                collectedKeys += MyGame.collectedKeys[i] + (i < MyGame.collectedKeys.Count-1?",":"");
            }
            collectedKeys += "]";

            Console.WriteLine("Currently collected keys:" + collectedKeys); ;
            Vec2CollisionManager.colliders.Remove(this.gCollider._collider);
            Vec2GravityHandler.gravityObjects.Remove(this.gCollider);
            this.LateDestroy();
            Console.WriteLine("Destroyed Key");
        }
    }
}
