using GXPEngine;

public class Vec2Collider : GameObject // a master Class, not for use in the collision manager
{
    bool solid;
    bool stationary;

    public Vec2 _position = new Vec2(0, 0);
    public Vec2 _velocity = new Vec2(0, 0);
    Vec2 _oldPosition = new Vec2(0, 0);

    public float mass;

    public Vec2Collider(float Mass, bool stationary = false, bool solid = true, bool addToManager = true) : base()
    {
        this.solid = solid;
        this.mass = Mass;
        this.stationary = stationary;

        if (addToManager) Vec2CollisionManager.colliders.Add(this);
    }
    public bool IsSolid() { return solid; }
    public bool IsStationary() {  return stationary; }

    public void SetStationary(bool newVal) { stationary = newVal; }

    public void UpdateOldPosition ()
    {
        _oldPosition = _position;
    }

    public Vec2 GetOldPosition() { return _oldPosition; }

    public void Step()
    {
        if (stationary) { return; }
        _position += _velocity;

        _velocity.MaxLength(Settings.maxVelocity);
    }


    public virtual void Trigger(Vec2Collider otherCollider)
    {
        //override this method
    }
}

