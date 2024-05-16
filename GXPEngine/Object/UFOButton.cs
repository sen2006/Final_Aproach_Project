using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class UFOButton : AnimationSprite
{
    int ID;
    //1=red
    //2=green
    //3=blue
    int state = 0;
    EasyDraw easyDraw;
    public UFOButton(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        easyDraw = new EasyDraw(Mathf.Ceiling(obj.Width * 2), Mathf.Ceiling(obj.Height * 2), true);
        easyDraw.SetXY(-height / 2, -width / 2);
        AddChild(easyDraw);
        ID = obj.GetIntProperty("ID", 0);
        MyGame.UFOButtons.Add(this);
    }

    public void Update()
    {
        Camera cam = MyGame.GetGame().camera;
        if (cam == null) return;
        Vector2 mouse = cam.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
        if (Input.GetMouseButtonDown(0) && easyDraw.HitTestPoint(mouse.x, mouse.y))
        {
            state++;
            if (state > 3) state = 1;
            SoundHandler.ButtonPress.Play();
        }
        Draw();
    }

    void Draw()
    {
        switch (state)
        {
            case 1:
                easyDraw.Clear(250, 53, 53);
                break;
            case 2:
                easyDraw.Clear(112, 214, 98);
                break;
            case 3:
                easyDraw.Clear(84, 196, 252);
                break;
            default:
                switch ((Time.time/500) % 3)
                {
                    case 0:
                        easyDraw.Clear(250, 53, 53);
                        break;
                    case 1:
                        easyDraw.Clear(112, 214, 98);
                        break;
                    case 2:
                        easyDraw.Clear(84, 196, 252);
                        break;
                }
                break;
        }
    }

    public bool isCorrect()
    {
        return (ID == state);
    }
}

