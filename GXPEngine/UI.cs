using GXPEngine;
using System;

public class UI : GameObject
{
    EasyDraw coordinates;

    EasyDraw fuelBar;
    EasyDraw healthBar;
    Sprite fuelBarOverlay;

    public UI()
    {
        coordinates = new EasyDraw(500, 50);
        AddChild(coordinates);

        fuelBar = new EasyDraw(1920, 1080, false);
        AddChild(fuelBar);

        fuelBarOverlay = new Sprite("data/UI/fuel_bar_overlay.png");
        AddChild(fuelBarOverlay);
        fuelBarOverlay.SetOrigin(fuelBarOverlay.width / 2, fuelBarOverlay.height);
        fuelBarOverlay.SetXY(1920 / 2, 1080);

        healthBar = new EasyDraw(1920, 1080, false);
        AddChild(healthBar);
    }

    public void Draw()
    {
        Player player = MyGame.GetGame().player;

        coordinates.Clear(0, 0, 0, 0);
        coordinates.Text("coordinates "+ Mathf.Round(player.x) + "," + Mathf.Round(player.y));

        fuelBar.Clear(0, 0, 0, 0);
        float fuelBarWidth = 850;
        float fuelBarHeight = 115;

        float redFuelBarWidth = fuelBarWidth * (1 - player.GetFuel() / Settings.maxFuel);

        fuelBar.Stroke(0, 0);

        fuelBar.Fill(191, 233, 255);
        fuelBar.Rect(1920 / 2, 1080 - 140 + fuelBarHeight / 2, fuelBarWidth, fuelBarHeight);

        fuelBar.Fill(185, 73, 75);
        fuelBar.Rect(1920 / 2 + -fuelBarWidth / 2 + (fuelBarWidth - redFuelBarWidth / 2), 1080 - 140 + fuelBarHeight / 2, redFuelBarWidth, fuelBarHeight);

        healthBar.Clear(0, 0, 0, 0);
        healthBar.Text("health:"+MyGame.GetGame().player.GetHealth()+"/"+Settings.maxHealth, 0, 20);
    }
}

