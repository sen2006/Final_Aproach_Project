using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Settings
{
    // gravity
    internal readonly static float gravitationalConstant = .03f;
    internal readonly static float maxVelocity = 100f;
    internal readonly static float minGravityToRotate = 1f;
    internal readonly static float defauldPlanetDensity = 1f;
    internal readonly static float defauldPlayerDensity = 1.1f;


    // player
    internal readonly static float fuelUsage = .1f;
    internal readonly static float maxFuel = 6f;
    internal readonly static float boosterPower = 4;
    internal readonly static float walkSpeed = 3;
    internal readonly static bool cameraRotation = false; // warning motion sickness
}

