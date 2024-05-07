public class Vec2GravityCollider
{
    public Vec2Collider _collider;

    public Vec2GravityCollider (Vec2Collider collider)
    {
        _collider = collider;
        Vec2GravityHandler.gravityObjects.Add(this);
    }
}
