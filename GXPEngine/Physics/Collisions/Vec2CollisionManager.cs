using System.Collections.Generic;

public static class Vec2CollisionManager
{
    // TODO create a collision order

    public static List<Vec2Collider> colliders = new List<Vec2Collider>();
    // settings

    static float bounciness = 0f;

    public static void UpdateOldPositions()
    {
        foreach (Vec2Collider c in colliders)
        {
            c.UpdateOldPosition();
        }
    }


    public static void HandleCollisions()
    {
        foreach (Vec2Collider c in colliders)
        {
            c.Step();
        }
        HandleEarliestCollisions(getCollisionList());
        HandleExtraCollisions();
    }

    static List<Vec2Collision> getCollisionList()
    {
        List<Vec2Collision> collisions = new List<Vec2Collision>();
        foreach (Vec2Collider c in colliders)
        {
            if (c.IsStationary() || !c.IsSolid()) { continue; }
            foreach (Vec2Collider c2 in colliders)
            {
                if (c2 == c) { continue; }
                if (c is Vec2BallCollider cB)
                {
                    float collisionTime = 1;
                    // ball on :
                    if (c2 is Vec2BallCollider c2B) { collisionTime = Vec2PhysicsCalculations.TimeOfImpactBall(c._velocity, c._position, c.GetOldPosition(), c2._position, cB.radius, c2B.radius); } // ball
                    if (c2 is Vec2LineCollider c2L) {   float distance = c._position.distance(c2._position);
                                                        if (distance >= 0) collisionTime = Vec2PhysicsCalculations.TimeOfImpactLine(cB._position, cB.GetOldPosition(), c2L.startPos, c2L.endPos, cB.radius);
                                                        else collisionTime = Vec2PhysicsCalculations.TimeOfImpactLine(cB._position, cB.GetOldPosition(), c2L.endPos, c2L.startPos, cB.radius);
                                                    } // line
                    if (collisionTime > -1 && collisionTime < 1)
                    {
                        collisions.Add(new Vec2Collision(c,c2,collisionTime));
                    }
                }
            }
        }

        return collisions;
    }

    static void HandleEarliestCollisions(List<Vec2Collision> collisions)
    {
        while (true)
        {
            float earliestCollisionTOI = 1;
            Vec2Collision earliestCollision = null;

            foreach (Vec2Collision collision in collisions)
            {
                float collisionTime = collision.TOI;
                if (collisionTime < earliestCollisionTOI)
                {
                    earliestCollisionTOI = collisionTime;
                    earliestCollision = collision;
                }
            }

            if (earliestCollisionTOI < 1 && earliestCollision != null)
            {
                handleCollision(earliestCollision.c, earliestCollision.c2);
                collisions.Remove(earliestCollision);
            }
            if (collisions.Count == 0) { return; }
        }
    }

    static void handleCollision(Vec2Collider c, Vec2Collider c2)
    {
        if (c is Vec2BallCollider)
        {
            // ball on :
            if (c2 is Vec2BallCollider) { HandleCollisionBallBall((Vec2BallCollider)c, (Vec2BallCollider)c2); } // ball
            if (c2 is Vec2LineCollider) { HandleCollisionBallLine((Vec2BallCollider)c, (Vec2LineCollider)c2); } // line
        }
    }

    public static void HandleExtraCollisions()
    {
        foreach (Vec2Collider c in colliders)
        {
            if (c.IsStationary() || !c.IsSolid()) { continue; }
            foreach (Vec2Collider c2 in colliders)
            {
                if (c2 == c) { continue; }
                if (c is Vec2BallCollider)
                {
                    // ball on :
                    if (c2 is Vec2BallCollider) { HandleCollisionBallBall((Vec2BallCollider)c, (Vec2BallCollider)c2); } // ball
                    if (c2 is Vec2LineCollider) { HandleCollisionBallLine((Vec2BallCollider)c, (Vec2LineCollider)c2); } // line
                }
            }
        }
    }

    static bool HandleCollisionBallBall(Vec2BallCollider c, Vec2BallCollider c2)
    {
        bool hasCollided = false;

        float distance = c._position.distance(c2._position);
        float oldDistance = c.GetOldPosition().distance(c2._position);
        Vec2 collisionNormal = (c2._position - c._position).Normalized();
        /*if (distance - (c.radius + c2.radius) < 0.000001f && oldDistance - (c.radius + c2.radius) < 0.000001f)
        {
            //was already close to other ball
            if (c.IsSolid() && c2.IsSolid())
            {
                float overlap = c.radius + c2.radius - distance;
                c._position += collisionNormal * -overlap;
            }
            c.Trigger(c2);
            c2.Trigger(c);
            hasCollided = true;
        }
        else */if (Vec2PhysicsCalculations.TimeOfImpactBall(c._velocity, c._position, c.GetOldPosition(), c2._position, c.radius, c2.radius) != 1)
        {
            //collision detected with other ball

            if (c.IsSolid() && c2.IsSolid())
            {
                Vec2 POI = Vec2PhysicsCalculations.PointOfImpactBall(c._velocity, c._position, c.GetOldPosition(), c2._position, c.radius, c2.radius);
                c._position = POI;
                Vec2 COMVelocity = c2.IsStationary() ? new Vec2(0, 0) : (c.mass * c._velocity + c2.mass * c2._velocity) / (c.mass + c2.mass);

                c._velocity = c._velocity - (1 + bounciness) * ((c._velocity - COMVelocity).Dot(collisionNormal)) * collisionNormal;
                if (!c2.IsStationary()) c2._velocity = c2._velocity - (1 + bounciness) * ((c2._velocity - COMVelocity).Dot(collisionNormal)) * collisionNormal;
            }
            c.Trigger(c2);
            c2.Trigger(c);
            hasCollided = true;
        }

        return hasCollided;
    }

    static bool HandleCollisionBallLine(Vec2BallCollider cB, Vec2LineCollider cL)
    {
        bool hasCollided = false;

        float distance = Vec2PhysicsCalculations.LineDistance(cB._position, cL.startPos, cL.endPos);
        float oldDistance = Vec2PhysicsCalculations.LineDistance(cB.GetOldPosition(), cL.startPos, cL.endPos);

        Vec2 differanceVecStart = cB._position - cL.startPos;
        Vec2 differanceVecEnd = cB._position - cL.endPos;

        Vec2 lineVec = cL.endPos - cL.startPos;
        Vec2 lineNormal = lineVec.Normal();

        float scalarVecStart = differanceVecStart.Dot(lineVec.Normalized());
        float scalarVecEnd = differanceVecEnd.Dot(lineVec.Normalized() * -1);
        
        if ((distance < cB.radius && distance > -cB.radius) && 
                (scalarVecStart > 0 && scalarVecEnd > 0))
        { // check for collision
            if (cB.IsSolid() && cL.IsSolid())
            {
                // calculate the point of impact
                Vec2 POI;
                if (distance >= 0) POI = Vec2PhysicsCalculations.PointOfImpactLine(cB._position, cB.GetOldPosition(), cL.startPos, cL.endPos, cB.radius);
                else POI = Vec2PhysicsCalculations.PointOfImpactLine(cB._position, cB.GetOldPosition(), cL.endPos, cL.startPos, cB.radius);
                // return the ball to the point of impact
                cB._position = POI;
                // reflect the velocity on the line normal
                cB._velocity.Reflect(lineNormal, bounciness);
            }
            cB.Trigger(cL);
            cL.Trigger(cB);
            hasCollided = true;
        }
        else
        { // if the previous collision failes try colliding with the caps at either end
            if (HandleCollisionBallBall(cB, cL.beginCap) ||
                HandleCollisionBallBall(cB, cL.endCap))
            {
                cB.Trigger(cL);
                cL.Trigger(cB);
                hasCollided = true;
            }
        }

        return hasCollided;
    }
}

