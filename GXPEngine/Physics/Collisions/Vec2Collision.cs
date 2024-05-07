

public class Vec2Collision
{
    public Vec2Collider c;
    public Vec2Collider c2;
    public float TOI;
    public Vec2Collision (Vec2Collider c, Vec2Collider c2, float TOI)
    {
        this.c = c;
        this.c2 = c2;
        this.TOI = TOI;
    }
}
