using GXPEngine;
using System;

public class GravityObject : GameObject
{
    public float radius;
    public Vec2GravityCollider gCollider;
    public GravityObject(Vec2 position, float radius, float density = 1, bool stationary = false) : base()
    {
        gCollider = new Vec2GravityCollider(new Vec2BallCollider(radius, density, stationary));
        gCollider._collider._position = position;
        this.radius = radius;
    }

    public virtual void Draw() { }

    public virtual void UpdateScreenPosition()
    {
        x = gCollider._collider._position.x;
        y = gCollider._collider._position.y;
        Draw();
    }

    public void Step()
    {
        Planet nearestPlanet = FindNearestPlanet();
        Vec2Collider colider = gCollider._collider;
        Vec2Collider planetColider = nearestPlanet.gCollider._collider;
        if (Vec2PhysicsCalculations.TimeOfImpactBall(colider._velocity, colider._position, colider.GetOldPosition(), planetColider._position, radius, nearestPlanet.radius) < 0.0000001f)
        {
            float aproachSpeed = colider._velocity.Normalized().Dot((planetColider._position - colider._position).Normalized());
            Console.WriteLine(aproachSpeed);

            /*
            float planetAngle = (planetColider._position - colider._position).GetAngleDegrees();
            float velocityAngle = colider._velocity.GetAngleDegrees();

            if (planetAngle - velocityAngle > 90) colider._velocity = new Vec2(0,0);

            Console.WriteLine("planet C" + (planetAngle-velocityAngle));
            */

            colider._velocity = new Vec2(0, 0);
        }
    }

    public Planet FindNearestPlanet()
    {
        float nearestdistance = float.PositiveInfinity;
        Planet nearestPlanet = null;
        foreach (Planet p in MyGame.planets) 
        {
            float distance = (gCollider._collider._position - p.gCollider._collider._position).Length() - (radius+p.radius);

            if (distance < nearestdistance)
            {
                nearestPlanet = p;
                nearestdistance = distance;
            }
        }

        return nearestPlanet;
    }
}
