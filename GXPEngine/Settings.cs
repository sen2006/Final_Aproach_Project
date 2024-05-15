using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Settings
{
    // gravity
    internal static float gravitationalConstant = .03f;
    internal static float maxVelocity = 100f;
    internal static float minGravityToRotate = 1f;
    internal static float defauldPlanetDensity = 1f;
    internal static float defauldPlayerDensity = 1.1f;
    internal static float defauldBlackHoleDensity = 10f;
    internal static float minGravityForce = 10f;


    // player
    internal static float fuelUsage = .1f;
    internal static float maxFuel = 3f;
    internal static float boosterPower = 4;
    internal static float walkSpeed = 3;
    internal static bool cameraRotation = false; // warning motion sickness
    internal static float maxHealth = 3;
    internal static float animationDeltaFrameTime=.15f;

    // border
    internal static Vec2 upperLeftBorder = new Vec2(-1000, -1000);
    internal static Vec2 lowerRightBorder = new Vec2(5000, 5000);
}

