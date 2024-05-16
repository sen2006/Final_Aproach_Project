public static class Settings
{
    // gravity
    internal static float gravitationalConstant = .1f;
    internal static float maxVelocity = 150f;
    internal static float minGravityToRotate = .5f;
    internal static float defauldPlanetDensity = 1f;
    internal static float defauldPlayerDensity = .8f;
    internal static float defauldBlackHoleDensity = 3.5f;
    internal static float minGravityForce = 10f;


    // player
    internal static float fuelUsage = .1f;
    internal static float maxFuel = 6f;
    internal static int jumpDelay = 880;//MS
    internal static int jumpBoostDelay = 150;//MS
    internal static float initialJumpDistance = 10;
    internal static float jumpPower = 50;
    internal static float boosterPower = 20;
    internal static float walkSpeed = 6;
    internal static bool cameraRotation = false; // warning motion sickness
    internal static float maxHealth = 3;
    internal static int immunityTime = 1000; //MS

    // player animations
    internal static float moveAnimationDeltaFrameTime = .18f;
    internal static float idleAnimationDeltaFrameTime = .15f;
    internal static float jumpAnimationDeltaFrameTime = .15f;
    internal static float boostAnimationDeltaFrameTime = .15f;
    internal static float landingAnimationDeltaFrameTime = .15f;


    internal static float backgroundAnimationDeltaFrameTime = .01f;

    // border
    internal static Vec2 upperLeftBorder = new Vec2(-1000, -1000);
    internal static Vec2 lowerRightBorder = new Vec2(5000, 5000);
}

