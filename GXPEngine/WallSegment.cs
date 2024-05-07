using GXPEngine;
using System.Drawing.Printing;

public class WallSegment : Vec2LineCollider
{
    EasyDraw easyDraw;
    int R;
    int G;
    int B;
    int A;
    float strokeWeight;

    public WallSegment(Vec2 startPos, Vec2 endPos, int R = 255, int G = 255, int B =255, int A = 255, float strokeWeight = 1) : base(startPos, endPos)
    {
        
        this.R = R;
        this.G = G;
        this.B = B;
        this.A = A;
        this.strokeWeight = strokeWeight;

        UpdateScreenPos();
    }

    public void UpdateScreenPos()
    {
        float xSmall = Mathf.Min(startPos.x, endPos.x);
        float ySmall = Mathf.Min(startPos.y, endPos.y);
        float xLarge = Mathf.Max(startPos.x, endPos.x);
        float yLarge = Mathf.Max(startPos.y, endPos.y);

        easyDraw = new EasyDraw(Mathf.Ceiling(xLarge - xSmall) + 10, Mathf.Ceiling(yLarge - ySmall) + 10, false);
        easyDraw.SetXY(xSmall - 5, ySmall - 5);
        easyDraw.Stroke(R, G, B, A);
        easyDraw.StrokeWeight(strokeWeight);
        easyDraw.Line((startPos.x - xSmall) + 5, (startPos.y - ySmall) + 5, (endPos.x - xSmall) + 5, (endPos.y - ySmall) + 5);

        RemoveChild(easyDraw);
        AddChild(easyDraw);
    }
}

