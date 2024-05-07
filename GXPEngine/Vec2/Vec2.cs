using GXPEngine;
using System;

public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float pX = 0, float pY = 0)
    {
        this.x = pX;
        this.y = pY;
    }

    // --------operators
    public static Vec2 operator *(Vec2 left, Vec2 right) => new Vec2(left.x * right.x, left.y * right.y);
    public static Vec2 operator *(Vec2 left, float right) => left * new Vec2(right, right);
    public static Vec2 operator *(float left, Vec2 right) => right*left;
    public static Vec2 operator /(Vec2 left, Vec2 right) => new Vec2(left.x / right.x, left.y / right.y);
    public static Vec2 operator /(Vec2 left, float right) => new Vec2(left.x / right, left.y / right);
    public static Vec2 operator -(Vec2 left, Vec2 right) => new Vec2(left.x - right.x, left.y - right.y);
    public static Vec2 operator +(Vec2 left, Vec2 right) => new Vec2(left.x + right.x, left.y + right.y);
    public static bool operator ==(Vec2 left, Vec2 right) => left.x==right.x && left.y==right.y;
    public static bool operator !=(Vec2 left, Vec2 right) => !(left==right);


    /// <summary>
    /// Returns a string version of the vector.
    /// </summary>
    /// <returns>Format "(x,y)".</returns>
    public override string ToString() { return String.Format("({0},{1})", x, y); }

    // --------return functions

    /// <summary>
    /// Returns the current length.
    ///  </summary>
    public float Length() { return Mathf.Sqrt(x * x + y * y); }

    /// <summary>
    /// Returns a normalized version of this vector whithout changing the original.
    /// </summary>
    public Vec2 Normalized()
    {
        Vec2 toReturn = this;
        toReturn.Normalize();
        return toReturn;
    }

    /// <summary>
    /// Liniar interpolation.
    /// </summary>
    /// <param name="pOtherVec">The other vector to lerp to.</param>
    /// <param name="pFactor">How much of the original vector gets replaced.</param>
    /// <returns>Interpolated vector.</returns>
    public Vec2 Lerped(Vec2 pOtherVec, float pFactor)
    {
        Vec2 toReturn = this;
        toReturn.Lerp(pOtherVec, pFactor);
        return toReturn;
    }

    /// <summary>
    /// Checks if a point is neatby the curent vector.
    /// </summary>
    /// <param name="pOtherVec">The other point to check.</param>
    /// <param name="pRange">The radius to check.</param>
    /// <returns>Returns true if the distance between this and otherVec is less than or equal to range.</returns>
    public bool InRange(Vec2 pOtherVec, float pRange)
    {
        Vec2 Delta = pOtherVec - this;
        return Delta.Length() <= pRange;
    }

    /// <summary>
    /// Gets the direction of a vector
    /// </summary>
    /// <returns>Angle in radians</returns>
    public float GetAngleRadians() { return Mathf.Atan2(y,x); }

    /// <summary>
    /// Gets the direction of a vector
    /// </summary>
    /// <returns>Angle in degrees</returns>
    public float GetAngleDegrees() { return Rad2Deg(GetAngleRadians()); }

    /// <summary>
    /// Gets the dot product of the current vector and another
    /// </summary>
    /// <param name="pOtherVec">The other point to check.</param>
    /// <returns>Dot product</returns>
    public float Dot(Vec2 pOtherVec)
    {
        return (x * pOtherVec.x + y * pOtherVec.y);
    }

    /// <summary>
    /// Gets the unit normal vector of the given vector
    /// </summary>
    /// <returns>Unit normal vector</returns>
    public Vec2 Normal()
    {
        return new Vec2(-y, x).Normalized();
    }

    /// <summary>
    /// Returns the distance from this vector to another
    /// </summary>
    /// <param name="pOtherVec">The other point to check.</param>
    public float distance(Vec2 pOtherVec)
    {
        return (pOtherVec - this).Length();
    }


    // --------void functions

    /// <summary>
    /// sets new X and Y.
    /// </summary>
    public void SetXY(float x, float y) { this.x = x; this.y = y; }

    /// <summary>
    /// Sets the length to the new value.
    /// </summary>
    /// <param name="pLength">New length.</param>
    public void SetLength(float pLength) {
        //if (this.Length() == 0) throw new ArgumentException("Cant set length on a Vec2 with a length of 0");
        this = this.Normalized() * pLength; 
    } 

    /// <summary>
    /// sets the length to 1.
    /// </summary>
    public void Normalize()
    {
        if (this.Length() == 0) return;
        this = this / this.Length(); 
    }

    /// <summary>
    /// Sets the length to a maximum value if it is over.
    /// (only gets run once)
    /// </summary>
    /// <param name="pMaxLength">The maximum</param>
    public void MaxLength(float pMaxLength)
    {
        if (pMaxLength < 0) { throw new ArgumentException("maxLengt should not be negative"); }
        this.SetLength(Mathf.Min(pMaxLength, this.Length()));
    }

    /// <summary>
    /// Liniar interpolation on the current vector.
    /// </summary>
    /// <param name="pTargetVec">The other vector to lerp to.</param>
    /// <param name="pFactor">How much of the original vector gets replaced.</param>
    public void Lerp(Vec2 pTargetVec, float pFactor)
    {
        this = this + (pTargetVec - this) * pFactor;
    }

    /// <summary>
    /// Rotates the vectors direction clockwise
    /// </summary>
    /// /// <param name="pRad">The amount to rotate in radians.</param>
    public void RotateRadians(float pRad)
    {
        this = new Vec2(Mathf.Cos(pRad) * x - Mathf.Sin(pRad) * y, Mathf.Cos(pRad) * y + Mathf.Sin(pRad) * x);

    }

    /// <summary>
    /// Rotates the vectors direction clockwise
    /// </summary>
    /// /// <param name="pDeg">The amount to rotate in degrees.</param>
    public void RotateDegrees(float pDeg)
    {
        RotateRadians(Deg2Rad(pDeg));
    }

    /// <summary>
    /// Rotates the vectors direction clockwise around another vector
    /// </summary>
    /// /// <param name="pRad">The amount to rotate in radians.</param>
    public void RotateAroundRadians(Vec2 pOtherVec, float pRad) 
    {
        this -= pOtherVec;
        this.RotateRadians(pRad);
        this += pOtherVec;
    }

    /// <summary>
    /// Rotates the vectors direction clockwise around another vector
    /// </summary>
    /// /// <param name="pDeg">The amount to rotate in degrees.</param>
    public void RotateAroundDegrees(Vec2 pOtherVec, float pDeg) 
    {
        this -= pOtherVec;
        this.RotateDegrees(pDeg);
        this += pOtherVec;
    }

    /// <summary>
    /// Set vector angle to the given direction in degrees.
    /// </summary>
    public void SetAngleDegrees(float pDeg)
    {
        SetAngleRadians(Deg2Rad(pDeg));
    }

    /// <summary>
    /// Set vector angle to the given direction in radians.
    /// </summary>
    public void SetAngleRadians(float pRad)
    {
        this = new Vec2(this.Length(),0);
        this.RotateRadians(pRad);
    }

    /// <summary>
    /// Reflects the curent vector using a normal
    /// </summary>
    /// <param name="pNormal">The normal of the surface bounced against</param>
    /// <param name="pBounciness">The bouniness of the vector</param>
    public void Reflect(Vec2 pNormal, float pBounciness = 1)
    {
        this = this - (1 + pBounciness) * (this.Dot(pNormal) * pNormal);
    }


    // ------------Static functions

    /// <summary>
    /// converts the given degrees to radians
    /// </summary>
    private static float Deg2Rad(float pRad) { return pRad * (Mathf.PI / 180); }

    /// <summary>
    /// converts the given radians to degrees
    /// </summary>
    private static float Rad2Deg(float pRad) { return pRad * (180 / Mathf.PI); }

    /// <summary>
    /// Returns a new unit vector pointing in a random direction
    /// </summary>
    public static Vec2 RandomUnitVector() {
        Random random = new Random();
        return GetUnitVectorDeg(random.Next(360)); 
    }

    /// <summary>
    /// returns a new vector pointing in the given direction in degrees.
    /// </summary>
    public static Vec2 GetUnitVectorDeg(float pRad)
    {
        return GetUnitVectorRad(Deg2Rad(pRad));
    }

    /// <summary>
    /// returns a new vector pointing in the given direction in radians.
    /// </summary>
    public static Vec2 GetUnitVectorRad(float pRad)
    {
        Vec2 toReturn = new Vec2(1,0);
        toReturn.SetAngleRadians(pRad); 
        toReturn.SetLength(1);
        return toReturn;
    }

    
    
    public static void UnitTest()
    {
        Console.WriteLine("180 deg in rad:" + Vec2.Deg2Rad(180));
        Console.WriteLine("1 rad in deg:" + Vec2.Rad2Deg(1));

        Vec2 testVec = Vec2.GetUnitVectorDeg(270);
        Vec2 testVec2 = Vec2.GetUnitVectorRad(2);
        Vec2 testVec3 = Vec2.RandomUnitVector();

        Console.WriteLine("The unit vector for deg angle 270 :" + testVec + "-length:" + testVec.Length() + "-deg:" + testVec.GetAngleDegrees());
        Console.WriteLine("The unit vector for rad angle 2 :" + testVec2 + "-length:" + testVec2.Length() + "-rad:" + testVec2.GetAngleRadians());
        Console.WriteLine("A random unit vector:" + testVec3 + "-length:" + testVec3.Length() + "-deg:" + testVec3.GetAngleDegrees());

        testVec = new Vec2(5, 5);
        testVec2 = testVec;
        testVec2.RotateDegrees(45);
        testVec3 = testVec;
        testVec3.RotateRadians(1);

        Console.WriteLine("the vector " + testVec + "-length:" + testVec.Length() + "-deg:" + testVec.GetAngleDegrees() + " rotated by 45 deg :" + testVec2 + "-length:" + testVec2.Length() + "-deg:" + testVec2.GetAngleDegrees());
        Console.WriteLine("the vector " + testVec + "-length:" + testVec.Length() + "-rad:" + testVec.GetAngleRadians() + " rotated by 1 rad :" + testVec3 + "-length:" + testVec3.Length() + "-deg:" + testVec3.GetAngleRadians());

        testVec2 = testVec;
        testVec2.SetAngleDegrees(70);
        testVec3 = testVec;
        testVec3.SetAngleRadians(2);

        Console.WriteLine("the vector " + testVec + "-length:" + testVec.Length() + " set to 70 deg :" + testVec2 + "-length:" + testVec2.Length() + "-deg:" + testVec2.GetAngleDegrees());
        Console.WriteLine("the vector " + testVec + "-length:" + testVec.Length() + " set to 2 rad :" + testVec3 + "-length:" + testVec3.Length() + "-rad:" + testVec3.GetAngleRadians());

        Vec2 rotatePoint = new Vec2(2, 6);
        testVec2 = testVec;
        testVec2.RotateAroundDegrees(rotatePoint, 90);
        testVec3 = testVec;
        testVec3.RotateAroundRadians(rotatePoint, 2);

        Console.WriteLine("the vector " + testVec + " rotated around :" + rotatePoint + " by 90 deg" + testVec2);
        Console.WriteLine("the vector " + testVec + " rotated around :" + rotatePoint + " by 2 rad" + testVec3);

        Console.WriteLine("the normalized version of vector " + testVec + "is :" + testVec.Normalized());
        Console.WriteLine("the unit normal of vector " + testVec + "is :" + testVec.Normal());

        Vec2 reflectionNormal = Vec2.GetUnitVectorDeg(30);

        Console.WriteLine("the vector " + testVec + "reflected off of the normal:" + reflectionNormal + "is :"+ testVec2);

    }
}

