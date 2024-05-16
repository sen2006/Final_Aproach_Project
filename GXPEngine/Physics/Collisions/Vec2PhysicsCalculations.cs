using GXPEngine;

public static class Vec2PhysicsCalculations
{
    //TODO: comments

    /// <summary>
    /// Calculates the the time of impact with a line
    /// </summary>
    /// <param name="pNewPos">The position at the end of the frame</param>
    /// <param name="pOldPos">The position at the start of the frame</param>
    /// <param name="pLineSegmentStart">The start of the line colliding with</param>
    /// <param name="pLineSegmentEnd">The end of the line colliding with</param>
    /// <param name="radius">For a circle hitbox</param>
    /// <returns>0 = begining of frame, 1 = end of frame</returns>
    public static float TimeOfImpactLine(Vec2 pNewPos, Vec2 pOldPos, Vec2 pLineSegmentStart, Vec2 pLineSegmentEnd, float radius = 0)
    {
        Vec2 lineVec = pLineSegmentEnd - pLineSegmentStart;
        Vec2 LineNormal = lineVec.Normal();
        Vec2 oldDifferenceVec = pOldPos - pLineSegmentStart;
        Vec2 newDifferenceVec = pNewPos - pLineSegmentStart;

        float oldDist = oldDifferenceVec.Dot(LineNormal) - radius;
        float newDist = newDifferenceVec.Dot(LineNormal);

        return oldDist / newDist;
    }

    /// <summary>
    /// Calculates the the point of impact with a line
    /// </summary>
    /// <param name="pNewPos">The position at the end of the frame</param>
    /// <param name="pOldPos">The position at the start of the frame</param>
    /// <param name="pLineSegmentStart">The start of the line colliding with</param>
    /// <param name="pLineSegmentEnd">The end of the line colliding with</param>
    /// <param name="radius">For a circle hitbox</param>
    /// <returns>Vector position of the point of impact</returns>
    public static Vec2 PointOfImpactLine(Vec2 pNewPos, Vec2 pOldPos, Vec2 pLineSegmentStart, Vec2 pLineSegmentEnd, float radius = 0)
    {
        return pOldPos + TimeOfImpactLine(pNewPos, pOldPos, pLineSegmentStart, pLineSegmentEnd, radius) * (pNewPos - pOldPos);
    }

    /// <summary>
    /// Calculates the the time of impact with a ball
    /// </summary>
    /// <param name="pNewPos1">The position at the end of the frame</param>
    /// <param name="pOldPos1">The position at the Beginning of the frame</param>
    /// <param name="pPos2">The position of the ball</param>
    /// <param name="pRadius1">The radius</param>
    /// <param name="pRadius2">The other Radius</param>
    /// <returns>0 = begining of frame, 1 = end of frame</returns>
    public static float TimeOfImpactBall(Vec2 velocity1, Vec2 pNewPos1, Vec2 pOldPos1, Vec2 pPos2, float pRadius1, float pRadius2)
    {
        Vec2 relativePos = pPos2-pOldPos1;

        // quadratic formula
        float a = Mathf.Pow(velocity1.Length(),2);
        float b = (2 * relativePos).Dot(velocity1);
        float c = Mathf.Pow(relativePos.Length(),2) - Mathf.Pow((pRadius1 + pRadius2),2);

        float D = Mathf.Pow(b, 2) - 4 * a * c;

        if (a < 0.00000001 ) return 1; // no collision
        if (D < 0) return 1; // no collision
        if (c < 0 ) return 0; // collision error : no collision

        float TOI = (-b - Mathf.Sqrt(D)) / (2*a);

        if (TOI<=0 || TOI > 1) return 1; // no collision

        return TOI;
    }

    /// <summary>
    /// Calculates the the point of impact with a ball
    /// </summary>
    /// <param name="pNewPos1">The position at the end of the frame</param>
    /// <param name="pOldPos1">The position at the Beginning of the frame</param>
    /// <param name="pPos2">The position of the ball</param>
    /// <param name="pRadius1">The radius</param>
    /// <param name="pRadius2">The other Radius</param>
    /// <returns>Vector position of the point of impact</returns>
    public static Vec2 PointOfImpactBall(Vec2 velocity1, Vec2 pNewPos1, Vec2 pOldPos1, Vec2 pPos2, float pRadius1, float pRadius2)
    {
        return pOldPos1 + TimeOfImpactBall(velocity1, pNewPos1, pOldPos1, pPos2, pRadius1, pRadius2) * (pNewPos1 - pOldPos1);
    }

    public static float LineDistance(Vec2 pPosition, Vec2 pLineSegmentStart, Vec2 pLineSegmentEnd)
    {
        Vec2 lineVec = pLineSegmentEnd - pLineSegmentStart;
        Vec2 lineNormal = lineVec.Normal();

        Vec2 differanceVecStart = pPosition - pLineSegmentStart;

        float ballDistance = differanceVecStart.Dot(lineNormal);

        return ballDistance;
    }
}

