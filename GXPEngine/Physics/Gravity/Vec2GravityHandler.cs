using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Vec2GravityHandler
{
    public static List<Vec2GravityCollider> gravityObjects = new List<Vec2GravityCollider>();

    public static void handleGravity()
    {
        foreach (Vec2GravityCollider atractor in gravityObjects)
        {
            foreach (Vec2GravityCollider obj in gravityObjects)
            {
                if (obj == atractor || obj._collider.IsStationary()) { continue; }
                Vec2 direction = atractor._collider._position - obj._collider._position;
                float distance = direction.Length();
                float forceMagnitude = (Settings.gravitationalConstant * atractor._collider.mass * obj._collider.mass) / Mathf.Pow(distance, 2);
                Vec2 force = direction.Normalized() * forceMagnitude;
                obj._collider._velocity += force/obj._collider.mass;

            }
        }
    }
}

