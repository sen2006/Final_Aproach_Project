using GXPEngine;
using System;

public class UI : GameObject
{
    EasyDraw coordinates;

    EasyDraw text;

    EasyDraw fuelBar;
    Sprite fuelBarOverlay;
    Sprite shieldOverlay;
    Sprite keyOverlay;

    Sprite key1;
    Sprite key2;
    Sprite key3;
    Sprite key0;

    Sprite damageScreen;
    int damageScreenTimer=0;

    public UI()
    {
        coordinates = new EasyDraw(500, 50);
        AddChild(coordinates);

        fuelBar = new EasyDraw(1920, 1080, false);
        AddChild(fuelBar);
        fuelBar.SetOrigin(fuelBar.width / 2, fuelBar.height);
        fuelBar.SetXY(1920 / 2, 1050);

        fuelBar.scale = .5f;

        fuelBarOverlay = new Sprite("data/UI/fuel_bar_overlay.png");
        AddChild(fuelBarOverlay);
        fuelBarOverlay.SetOrigin(fuelBarOverlay.width / 2, fuelBarOverlay.height);
        fuelBarOverlay.SetXY(1920 / 2, 1050);

        fuelBarOverlay.scale = .5f;

        text = new EasyDraw(1920, 1080, false);
        AddChild(text);

        shieldOverlay = new Sprite("data/UI/shield.png");
        AddChild(shieldOverlay);
        shieldOverlay.SetOrigin(shieldOverlay.width/2, 0);
        shieldOverlay.SetXY(1800,-20);
        shieldOverlay.scale = .2f;

        keyOverlay = new Sprite("data/UI/keys.png");
        AddChild(keyOverlay);
        keyOverlay.SetOrigin(keyOverlay.width/2, 0);
        keyOverlay.SetXY(1800, 180);
        keyOverlay.scale = .2f;

        key1 = new Sprite("data/UI/key1.png");
        AddChild(key1);
        key1.SetOrigin(key1.width / 2, 0);
        key1.SetXY(1800, 155);
        key1.scale = .2f;

        key2 = new Sprite("data/UI/key2.png");
        AddChild(key2);
        key2.SetOrigin(key2.width / 2, 0);
        key2.SetXY(1800, 155);
        key2.scale = .2f;

        key3 = new Sprite("data/UI/key3.png");
        AddChild(key3);
        key3.SetOrigin(key3.width / 2, 0);
        key3.SetXY(1800, 155);
        key3.scale = .2f;

        key0 = new Sprite("data/UI/key0.png");
        AddChild(key0);
        key0.SetOrigin(key0.width / 2, 0);
        key0.SetXY(1800, 155);
        key0.scale = .2f;

        damageScreen = new Sprite("data/UI/damage_screen.png");
        AddChild(damageScreen);
        damageScreen.visible = false;
        damageScreen.alpha = .4f;
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

        text.Clear(0, 0, 0, 0);
        text.TextSize(20);
        text.Text(Mathf.Round(MyGame.GetGame().player.GetHealth()*10)/10d+"/"+Settings.maxHealth, 1650, 100);
        //text.Text(MyGame.collectedKeys.Count+"/4", 1650, 250);

        key0.visible = MyGame.collectedKeys.Contains(0);
        key1.visible = MyGame.collectedKeys.Contains(1);
        key2.visible = MyGame.collectedKeys.Contains(2);
        key3.visible = MyGame.collectedKeys.Contains(3);

        if (damageScreenTimer>0)
        {
            damageScreen.visible=true;
            damageScreenTimer-=Time.deltaTime;
        } else { damageScreen.visible = false; }
    }

    public void damageScreenShow()
    {
        damageScreenTimer = Settings.damageUIOnScreenTime;
        damageScreen.visible = true;
    }
}

