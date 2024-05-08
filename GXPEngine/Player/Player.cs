using GXPEngine;
using GXPEngine.Core;
using System;
using TiledMapParser;
public class Player : GravityObject
{
    Vec2 vecRotation = new Vec2();
    Vec2 targetRotation = new Vec2(0,0);

    float nearestPlanetForce = 0;
    Planet nearestPlanet = null;

    EasyDraw easyDraw;

    public static float fuel = Settings.maxFuel;
    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        easyDraw = new EasyDraw(Mathf.Ceiling(radius) * 2, Mathf.Ceiling(radius) * 2);
        easyDraw.SetOrigin(radius, radius);

        MyGame.player = this;
        MyGame.GetGame().AddChild(this);

        gCollider._collider.SetStationary(obj.GetBoolProperty("stationary", false));
        ((Vec2BallCollider)gCollider._collider).radius = 20;
    }

    public override void Step()
    {
        refillFuel();
        handleInputs();
        base.Step();
    }

    void refillFuel()
    {
        if (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f)
        {
            fuel = Settings.maxFuel;
        }
    }

        public void handleInputs()
    {
        if (Input.GetKey(Key.SPACE))
        {
            if (fuel > 0)
            {
                // not run more than once
                if (Input.GetKeyDown(Key.SPACE) && (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f))
                {

                    Vec2 planetAngle = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
                    gCollider._collider._position = nearestPlanet.gCollider._collider._position + (planetAngle.Normalized() * -(2.5f + radius + nearestPlanet.radius));
                    gCollider._collider._velocity += planetAngle.Normalized() * -Settings.maxVelocity;
                    if (Input.GetKey(Key.D))
                    {
                        gCollider._collider._velocity.RotateDegrees(60);
                    }
                    if (Input.GetKey(Key.A))
                    {
                        gCollider._collider._velocity.RotateDegrees(-60);
                    }
                }
                gCollider._collider._velocity.Lerp(vecRotation * Settings.boosterPower * (1 + (nearestPlanetForce * .03f)) * -1, .01f);
            }
            fuel = Mathf.Max(fuel - Settings.fuelUsage, 0);
        }
        if (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f) {
            if (Input.GetKey(Key.D))
            {
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(-90);
                gCollider._collider._position += targetMovament;
                //gCollider._collider._velocity += targetMovament;
            }

            if (Input.GetKey(Key.A))
            {
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(90);
                gCollider._collider._position += targetMovament;
                //gCollider._collider._velocity += targetMovament;
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
        nearestPlanet = FindNearestPlanet();
        if (nearestPlanet == null) return;
        Vec2 direction = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();
        float forceMagnitude = (Settings.gravitationalConstant * nearestPlanet.gCollider._collider.mass * this.gCollider._collider.mass) / Mathf.Pow(distance, 2);

        nearestPlanetForce = 0;
        if (forceMagnitude >= Settings.minGravityToRotate)
        {
            nearestPlanetForce = forceMagnitude;
            targetRotation = (nearestPlanet.gCollider._collider._position - gCollider._collider._position).Normalized();
            vecRotation.Lerp(targetRotation, (distance-(radius+nearestPlanet.radius) <= 0.0000001f) ? .1f : .0025f + .025f * (forceMagnitude / 150));

            easyDraw.rotation = vecRotation.GetAngleDegrees();
        }
    }
}
