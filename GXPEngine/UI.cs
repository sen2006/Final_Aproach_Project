using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UI : GameObject
{
    EasyDraw fuelBar;
    public UI() 
    { 
        fuelBar = new EasyDraw(200, 200);
        AddChild(fuelBar);
    }

    public void Draw()
    {
        fuelBar.Clear(0,0,0,0);
        fuelBar.Text("fuel;"+Player.fuel+"/"+Settings.maxFuel);
    }

}

