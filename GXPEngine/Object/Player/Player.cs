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
        Idle,
        Float,
        Jump,
        Boost,
        Landing
    }

    State state = State.Idle;

    public Vec2 vecRotation = new Vec2();
    Vec2 targetRotation = new Vec2(0, 0);

    float nearestPlanetForce = 0;
    Planet nearestPlanet = null;

    EasyDraw easyDraw;

    bool wasJump = false;

    float fuel = 0;
    float health = Settings.maxHealth;

    AnimationSprite moveAnimationSprite;
    AnimationSprite idleAnimationSprite;
    AnimationSprite idleFloatAnimationSprite;
    AnimationSprite jumpAnimationSprite;
    AnimationSprite boostAnimationSprite;
    AnimationSprite landingAnimationSprite;

    AnimationSprite currentAnimation;
    float animationDeltaFrameTime;

    float boostTimer = 0;

    float immunityTimer;

    bool footstepPlayed = false;

    public Player(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows, obj)
    {
        gCollider._collider.mass = Mathf.PI * Mathf.Pow(radius, 2) * obj.GetFloatProperty("density", Settings.defauldPlayerDensity);
        gCollider._collider.SetStationary(obj.GetBoolProperty("stationary", false));

        easyDraw = new EasyDraw(Mathf.Ceiling(radius) * 2, Mathf.Ceiling(radius) * 2);
        easyDraw.SetOrigin(radius, radius);

        MyGame.GetGame().player = this;


        moveAnimationSprite = new AnimationSprite("data/player/walking.png", 5, 5, 22, false, false);
        moveAnimationSprite.visible = false;
        AddChild(moveAnimationSprite);

        idleAnimationSprite = new AnimationSprite("data/player/idle.png", 4, 4, -1, false, false);
        idleAnimationSprite.visible = false;
        AddChild(idleAnimationSprite);

        idleFloatAnimationSprite = new AnimationSprite("data/player/floating.png", 4, 4, 14, false, false);
        idleFloatAnimationSprite.visible = false;
        AddChild(idleFloatAnimationSprite);

        jumpAnimationSprite = new AnimationSprite("data/player/jump-with-fire.png", 3, 3, 8, false, false);
        jumpAnimationSprite.visible = false;
        AddChild(jumpAnimationSprite);

        boostAnimationSprite = new AnimationSprite("data/player/boost.png", 2, 2, 4, false, false);
        boostAnimationSprite.visible = false;
        AddChild(boostAnimationSprite);

        landingAnimationSprite = new AnimationSprite("data/player/landing.png", 3, 2, 6, false, false); ;
        landingAnimationSprite.visible = false;
        AddChild(landingAnimationSprite);

        Console.WriteLine("Player created at (" + x + "," + y + ") stationary:" + gCollider._collider.IsStationary() + ", mass:" + gCollider._collider.mass + ", density:" + obj.GetFloatProperty("density", Settings.defauldPlayerDensity));

    }

    public override void Step()
    {
        nearestPlanet = FindNearestPlanet();
        RefillFuel();
        HandleInputs();
        rotateToNearestPlanet();
        checkBoundries();
        checkPoisonPlanetOrDamage();
        if (checkHealth()) return;
        immunityTimer = Math.Max(immunityTimer - Time.deltaTime, 0);
        base.Step();
    }

    void checkPoisonPlanetOrDamage()
    {

        if (nearestPlanet != null)
        {
            Vec2 direction = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
            float distance = direction.Length();
            if (distance - (nearestPlanet.poisonRadius+radius) <= 0)
            {
                Damage(nearestPlanet.poisonDamage);
            }
            if (distance - (nearestPlanet.radius + radius) <= .5f)
            {
                Damage(nearestPlanet.touchDamage);
                if (nearestPlanet.touchDamage > 0)
                {
                    gCollider._collider._velocity = direction.Normalized() * -.5f;
                    gCollider._collider._position = nearestPlanet.gCollider._collider._position + direction.Normalized() * -(nearestPlanet.radius + radius + 2.5f);
                }
            }
        }
    }

    bool checkHealth()
    {
        if (health <= 0)
        {
            MyGame.GetGame().GameOver();
            return true;
        }
        if (immunityTimer <= 0)
            health = Mathf.Min(health + Settings.healthRegen, Settings.maxHealth);
        return false;
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
        if (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f &&
            nearestPlanet.gCollider._collider.mass > 0)
        {
            fuel = Mathf.Min(fuel + .3f ,Settings.maxFuel);
        }
    }

    public void HandleInputs()
    {
        state = State.Idle;
        if (nearestPlanet != null && !(gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f))
        {
            boostTimer = Mathf.Min(boostTimer + Time.deltaTime, Settings.jumpBoostDelay);
            state = State.Float;
        }
        else boostTimer = 0;
        if (Input.GetKey(Key.W) || Input.GetKey(Key.SPACE))
        {
            bool allowJump = (jumpAnimationSprite.currentFrame >= Settings.jumpFrame);
            bool allowBoost = (boostTimer >= Settings.jumpBoostDelay);
            // JUMP
            if (fuel > 0)
            {
                if (!wasJump &&
                    (nearestPlanet != null && gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f) &&
                    nearestPlanet.gCollider._collider.mass > 0)
                {
                    Vec2 planetAngle = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
                    if (allowJump)
                    {
                        gCollider._collider._position = nearestPlanet.gCollider._collider._position + (planetAngle.Normalized() * -(Settings.initialJumpDistance + radius + nearestPlanet.radius));
                        gCollider._collider._velocity = (vecRotation * Settings.jumpPower * -1);
                        SoundHandler.Jump.Play();
                    }
                    wasJump = allowJump;
                }
                if (!allowBoost) state = State.Jump;
                else state = State.Boost;
                if (allowJump || allowBoost) gCollider._collider._velocity.Lerp(vecRotation * Settings.boosterPower * -1, .02f);
            }
            if (allowBoost)
            {
                fuel = Mathf.Max(fuel - Settings.fuelUsage, 0);
            }
            return;
        }
        else
        {
            wasJump = false;
        }
        if (nearestPlanet != null &&
            gCollider._collider._position.distance(nearestPlanet.gCollider._collider._position) - (radius + nearestPlanet.radius) <= .5f &&
            !wasJump &&
            nearestPlanet.gCollider._collider.mass > 0 &&
            !(nearestPlanet is BlackHole))
        {
            bool walk = false;
            // move LEFT and RIGHT if player is on planet
            if (Input.GetKey(Key.D))
            {
                walk = true;
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(-90);
                gCollider._collider._position += targetMovament;

                _mirrorX = false;
                state = State.Move;
            }
            else if (Input.GetKey(Key.A))
            {
                walk = true;
                Vec2 targetMovament = targetRotation.Normalized() * Settings.walkSpeed;
                targetMovament.RotateDegrees(90);
                gCollider._collider._position += targetMovament;

                _mirrorX = true;
                state = State.Move;
            }
            if (walk)
            {
                if (moveAnimationSprite.currentFrame == 10 || moveAnimationSprite.currentFrame == 21)
                {
                    if (!footstepPlayed)
                    {
                        switch (MyGame.random.Next(3))
                        {
                            case 0:
                                SoundHandler.Footstep1.Play();
                                break;
                            case 1:
                                SoundHandler.Footstep2.Play();
                                break;
                            case 2:
                                SoundHandler.Footstep3.Play();
                                break;
                        }
                        footstepPlayed = true;
                    }
                }
                else footstepPlayed = false;
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
        if (nearestPlanet == null) return;
        Vec2 direction = nearestPlanet.gCollider._collider._position - gCollider._collider._position;
        float distance = direction.Length();
        float forceMagnitude = (Settings.gravitationalConstant * nearestPlanet.gCollider._collider.mass * this.gCollider._collider.mass) / Mathf.Pow(distance, 2);

        nearestPlanetForce = 0;
        if (forceMagnitude >= Settings.minGravityToRotate)
        {
            nearestPlanetForce = forceMagnitude;
            targetRotation = (nearestPlanet.gCollider._collider._position - gCollider._collider._position).Normalized();
            vecRotation.Lerp(targetRotation, (distance - (radius + nearestPlanet.radius) <= 0.5f) ? .6f : .0025f + .005f * (forceMagnitude / 50));
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
                animationDeltaFrameTime = Settings.moveAnimationDeltaFrameTime;
                break;
            case State.Idle:
                currentAnimation = idleAnimationSprite;
                animationDeltaFrameTime = Settings.idleAnimationDeltaFrameTime;
                break;
            case State.Float:
                currentAnimation = idleFloatAnimationSprite;
                break;
            case State.Jump:
                currentAnimation = jumpAnimationSprite;
                animationDeltaFrameTime = Settings.jumpAnimationDeltaFrameTime;
                break;
            case State.Boost:
                currentAnimation = boostAnimationSprite;
                animationDeltaFrameTime = Settings.boostAnimationDeltaFrameTime;
                break;
            case State.Landing:
                currentAnimation = landingAnimationSprite;
                animationDeltaFrameTime = Settings.landingAnimationDeltaFrameTime;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (currentAnimation != prevAnimation && currentAnimation != null)
        {
            if (prevAnimation != null)
            {
                prevAnimation.SetFrame(0);
                prevAnimation.visible = false;
            }

            currentAnimation.visible = true;
            currentAnimation.width = width;
            currentAnimation.height = height;
            currentAnimation.SetXY(-radius, -radius);
        }
        currentAnimation.Animate(animationDeltaFrameTime);
        currentAnimation.Mirror(_mirrorX, _mirrorY);
    }

    public float GetFuel() { return fuel; }

    public float GetHealth() { return health; }

    public void SetHealth(float health) { this.health = Mathf.Min(health, Settings.maxHealth); }

    public void Damage(float damage)
    {
        if (immunityTimer <= 0 && damage > 0)
        {
            health = Mathf.Max(Mathf.Min(health - damage, Settings.maxHealth), 0);
            immunityTimer = Settings.immunityTime;
            Console.WriteLine("Damage dealt to player:" + damage);
            MyGame.GetGame().UI.damageScreenShow();
        }
    }
}
