using GXPEngine;
using System;
public class Player : GravityObject
{
    Vec2 vecRotation = new Vec2();
    Vec2 targetRotation = new Vec2(0,0);

    float nearestPlanetForce = 0;

    EasyDraw easyDraw;
    public Player(Vec2 position, float radius, float density = 1) : base(position, radius, density)
    {
        easyDraw = new EasyDraw(Mathf.Ceiling(radius) * 2, Mathf.Ceiling(radius) * 2);
        easyDraw.SetOrigin(radius, radius);
    }

    public void handleInputs()
    {
        if (Input.GetKey(Key.SPACE)) {
            gCollider._collider._velocity.Lerp(vecRotation * Settings.boosterPower * (1+(nearestPlanetForce * .03f)) * -1, .01f);
            if (gCollider._collider._velocity.Length() <= 0.000001)
            {
                gCollider._collider._position += vecRotation * -0.5f;
                Console.WriteLine("YIPPEE");
            } 
        }
    }

    public override void UpdateScreenPosition()
    {

        base.UpdateScreenPosition();
        rotateToNearestPlanet();
    }

    public override void Draw()
    {
        if (!HasChild(easyDraw)) AddChild(easyDraw);
        easyDraw.Fill(255, 255, 255);
        easyDraw.StrokeWeight(1);
        easyDraw.Stroke(255, 0, 0);
        easyDraw.Rect(radius, radius, radius * 2, radius * 2);
    }

    public void rotateToNearestPlanet()
    {
        Planet nearestPlanet = FindNearestPlanet();
        Vec2 direction = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();
        float forceMagnitude = (Settings.gravitationalConstant * nearestPlanet.gCollider._collider.mass * this.gCollider._collider.mass) / Mathf.Pow(distance, 2);

        Console.WriteLine(forceMagnitude);

        nearestPlanetForce = 0;
        if (forceMagnitude >= Settings.minGravityToRotate)
        {
            nearestPlanetForce = forceMagnitude;
            targetRotation = (nearestPlanet.gCollider._collider._position - gCollider._collider._position).Normalized();
            vecRotation.Lerp(targetRotation, .05f + .2f * (forceMagnitude / (Settings.minGravityToRotate * 10)));

            easyDraw.rotation = vecRotation.GetAngleDegrees();
        }
    }
}
