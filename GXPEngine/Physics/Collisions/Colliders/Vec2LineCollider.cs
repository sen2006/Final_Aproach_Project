
public class Vec2LineCollider : Vec2Collider
{
    public Vec2 startPos;
    public Vec2 endPos;

    public Vec2BallCollider beginCap;
    public Vec2BallCollider endCap;
    public Vec2LineCollider(Vec2 startPos, Vec2 endPos, bool solid = true, bool addToManager = true) : base(0, true, solid, addToManager)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        _position = startPos + (endPos-startPos)/2;
        beginCap = new Vec2BallCollider(0, 1, true, solid, false);
        endCap = new Vec2BallCollider(0, 1, true, solid, false);

        beginCap._position = startPos;
        endCap._position = endPos;
    }


}
