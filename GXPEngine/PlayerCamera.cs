using GXPEngine;

public class PlayerCamera { }/* : Camera
{
    HoverCraft followCraft;

    public Vec2 _position;
    public Vec2 vecRotation;
    public PlayerCamera(int windowX, int windowY, int windowWidth, int windowHeight, HoverCraft pFollowCraft) : base (windowX, windowY, windowWidth, windowHeight, true)
    {
        followCraft = pFollowCraft;
    }

    void UpdateScreenPosition()
    {
        x = _position.x; y = _position.y;
        rotation = vecRotation.GetAngleDegrees();
    }

    public void Step(bool rotation = true)
    {
        _position.Lerp(followCraft._collider._position+(followCraft._collider._velocity *10), 0.2f);
        if (rotation) vecRotation.Lerp(followCraft.vecRotation, 0.06f);
        else vecRotation = Vec2.GetUnitVectorDeg(0);

        UpdateScreenPosition();
    }
}*/
