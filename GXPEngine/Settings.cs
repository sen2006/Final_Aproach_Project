public static class Settings
{
    // gravity
    internal static float gravitationalConstant = .03f;
    internal static float maxVelocity = 100f;
    internal static float minGravityToRotate = .5f;
    internal static float defauldPlanetDensity = 1f;
    internal static float defauldPlayerDensity = 1.1f;
    internal static float defauldBlackHoleDensity = 10f;
    internal static float minGravityForce = 10f;


    // player
    internal static float fuelUsage = .1f;
    internal static float maxFuel = 4f;
    internal static float jumpDelay = 700;//MS
    internal static float jumpBoostDelay = 150;//MS
    internal static float initialJumpDistance = 5;
    internal static float jumpPower = 200;
    internal static float boosterPower = 4;
    internal static float walkSpeed = 5;
    internal static bool cameraRotation = false; // warning motion sickness
    internal static float maxHealth = 3;

    // player animations
    internal static float moveAnimationDeltaFrameTime = .18f;
    internal static float idleAnimationDeltaFrameTime = .15f;
    internal static float jumpAnimationDeltaFrameTime = .15f;
    internal static float boostAnimationDeltaFrameTime = .15f;
    internal static float landingAnimationDeltaFrameTime = .15f;


    // border
    internal static Vec2 upperLeftBorder = new Vec2(-1000, -1000);
    internal static Vec2 lowerRightBorder = new Vec2(5000, 5000);
}

