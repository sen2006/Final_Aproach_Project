using GXPEngine;
using System;
using System.Diagnostics;
using System.Reflection;
using TiledMapParser;
public class Player : GravityObject
{
    enum State
    {
        Move,
        Idle
    }

    State state = State.Idle;

    public Vec2 vecRotation = new Vec2();
    Vec2 targetRotation = new Vec2(0, 0);

    float nearestPlanetForce = 0;
    Planet nearestPlanet = null;

    EasyDraw easyDraw;

    bool wasJump = false;

    float fuel = Settings.maxFuel;
    float health = Settings.maxHealth;

    AnimationSprite moveAnimationSprite;
    AnimationSprite idleAnimationSprite;

    int animationFrame;
    int animationCounter;
    AnimationSprite currentAnimation;

    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlayerDensity);
        gCollider._collider.SetStationary(obj.GetBoolProperty("stationary", false));

        easyDraw = new EasyDraw(Mathf.Ceiling(radius) * 2, Mathf.Ceiling(radius) * 2);
        easyDraw.SetOrigin(radius, radius);

        MyGame.GetGame().player = this;

        Console.WriteLine("Player created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary());

        moveAnimationSprite = new AnimationSprite("data/player/running-animation-sprite-sheet.png", 5,5, 22, false, false);
        moveAnimationSprite.visible = false;
        AddChild(moveAnimationSprite);

        idleAnimationSprite = new AnimationSprite("data/player/running-animation-sprite-sheet.png", 5, 5, 1, false, false);
        idleAnimationSprite.visible = false;
        AddChild(idleAnimationSprite);
    }

    public override void Step()
    {
        RefillFuel();
        HandleInputs();
        rotateToNearestPlanet();
        checkBoundries();
        checkHealth();
        base.Step();
    }

    void checkHealth()
    {
        if (health <= 0) MyGame.GetGame().GameOver();
    }

    void checkBoundries()
    {
        Vec2 pos = gCollider._collider._position;
        Vec2 UL = Settings.upperLeftBorder;
        Vec2 LR = Settings.lowerRightBorder;
        if (pos.x < UL.x || pos.x > LR.x || pos.y < UL.y || pos.y > LR.y)
        {
            MyGame.GetGame().GameOver();
        }
    }

    void RefillFuel()
    {
        if (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f)
        {
            fuel = Settings.maxFuel;
        }
    }

    public void HandleInputs()
    {
        state = State.Idle;
        if (Input.GetKey(Key.W) || Input.GetKey(Key.SPACE))
        {
            // JUMP
            if (fuel > 0)
            {
                if (!wasJump && (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f))
                {
                    Vec2 planetAngle = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
                    gCollider._collider._position = nearestPlanet.gCollider._collider._position + (planetAngle.Normalized() * -(2.5f + radius + nearestPlanet.radius));
                    wasJump = true;
                }
                gCollider._collider._velocity.Lerp(vecRotation * Settings.boosterPower * (1 + (nearestPlanetForce * .03f)) * -1, .01f);
            }
            fuel = Mathf.Max(fuel - Settings.fuelUsage, 0);
        }
        else wasJump = false;
        if (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f)
        {
            // move LEFT and RIGHT if player is on planet
            if (Input.GetKey(Key.D))
            {
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(-90);
                gCollider._collider._position += targetMovament;

                _mirrorX = false;
                state = State.Move;
            } else if (Input.GetKey(Key.A))
            {
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(90);
                gCollider._collider._position += targetMovament;
                
                _mirrorX = true;
                state = State.Move;
            }
        }
    }

    public override void UpdateScreenPosition()
    {
        base.UpdateScreenPosition();
        rotation = vecRotation.GetAngleDegrees() - 90;

        //Console.WriteLine("player V:"+gCollider._collider._velocity);
        //Console.WriteLine("player P:"+gCollider._collider._position);
    }

    public override void Draw()
    {
        playAnimation();
        /*if (!HasChild(easyDraw)) AddChild(easyDraw);
        easyDraw.Fill(255, 255, 255);
        easyDraw.StrokeWeight(1);
        easyDraw.Stroke(255, 0, 0);
        easyDraw.Rect(radius, radius, radius * 2, radius * 2);*/
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
            vecRotation.Lerp(targetRotation, (distance - (radius + nearestPlanet.radius) <= 0.0000001f) ? .1f : .0025f + .025f * (forceMagnitude / 150));
        }
    }

    public virtual void playAnimation()
    {
        //Console.WriteLine("animation"+"."+state);
        AnimationSprite prevAnimation = currentAnimation;
        switch (state)
        {
            case State.Move:
                currentAnimation = moveAnimationSprite;
                break;
            case State.Idle:
                currentAnimation = idleAnimationSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (currentAnimation != prevAnimation && currentAnimation!= null)
        {
            if (prevAnimation != null)
            {
                prevAnimation.visible = false;
            }

            currentAnimation.visible = true;
            currentAnimation.width = width;
            currentAnimation.height = height;
            currentAnimation.SetXY(-radius, -radius);
            animationCounter = 0;
            animationFrame = 0;
        }
        currentAnimation.Animate(Settings.animationDeltaFrameTime);
        currentAnimation.Mirror(_mirrorX, _mirrorY);

        /*
        if (animationCounter > Settings.framesPerAnimationFrame)
        {
            animationCounter = 0;
            if (animationFrame == currentAnimation.frameCount)
            {
                animationFrame = 0;
            }
            currentAnimation.SetFrame(animationFrame);
            animationFrame++;
        }
        animationCounter++;*/
    }

    public float GetFuel() { return fuel; }

    public float GetHealth() { return health; }

    public void SetHealth(float health) { this.health = Mathf.Min(health, Settings.maxHealth);}

    public void Damage(float damage) { health = Mathf.Max(Mathf.Min(health - damage, Settings.maxHealth),0); }
}
