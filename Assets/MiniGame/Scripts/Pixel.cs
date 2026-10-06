using UnityEngine;
using System.Collections;

public class Pixel : MonoBehaviour
{
    public Color[] colors = new Color[] { Color.grey, Color.blue, Color.green, Color.cyan, Color.yellow};
    PIXELSTYLE style;
    SpriteRenderer sRenderer;

	void Start () {
	}

    public void InitPixel(PIXELSTYLE v)
    {
        sRenderer = GetComponent<SpriteRenderer>();
        style = v;
        Draw(v);
    }

    public void Draw(PIXELSTYLE v)
    {
        switch (v)
        {
            case PIXELSTYLE.PATH: SetColor(colors[1]); break;
            case PIXELSTYLE.FILL: SetColor(colors[4]); break;
            case PIXELSTYLE.TYPE1: SetColor(colors[2]); break;
            case PIXELSTYLE.TYPE2: SetColor(colors[3]); break;
            default: SetColor(colors[0]); break;
        }
    }
    public void SetColor(Color c)
    {
        sRenderer.color = c;
    }
    public void SafeSave(PIXELSTYLE v)
    {
        if (style == PIXELSTYLE.FILL) return;
        Save(v);
    }
    public void Save(PIXELSTYLE v)
    {
        style = v;
        Draw(v);
    }
    public PIXELSTYLE Load()
    {
        return style;
    }
}
