using GXPEngine;
using System;

public class PlayerCamera : Camera
{
    Player player;

    public Vec2 _position;
    public Vec2 vecRotation;
    public PlayerCamera(int windowX, int windowY, int windowWidth, int windowHeight, Player pPlayer) : base (windowX, windowY, windowWidth, windowHeight, false)
    {
        player = pPlayer;
    }

    public void UpdateScreenPosition()
    {
        x = _position.x; y = _position.y;
        rotation = Settings.cameraRotation ? vecRotation.GetAngleDegrees()-90 : 0;
    }

    public void Step()
    {
        _position.Lerp(player.gCollider._collider._position+(player.gCollider._collider._velocity *10), 0.2f);
        if (Settings.cameraRotation) vecRotation.Lerp(player.vecRotation, 0.06f);
        else vecRotation = Vec2.GetUnitVectorDeg(0);
    }
}
