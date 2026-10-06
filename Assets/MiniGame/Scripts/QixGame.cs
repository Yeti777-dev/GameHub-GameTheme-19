using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public enum PIXELSTYLE { EMPTY, PATH, TYPE1, TYPE2, FILL };

public class QixGame : MonoBehaviour {
    public Color borderColor = Color.white, cursorColor = Color.red;
    public GameObject dotPrefab;
    public Material emptyMaterial, FILLMaterial, cursorMaterial;
    int cols = 100;
    int rows = 70;
    float scale = 8f;

    Pixel[,] grid;

    public Cursor cursor;

	void Start () {
        InitGrid();
	}

    void InitGrid()
    {
	    grid = new Pixel[cols, rows];
        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
            {
                GameObject go = Instantiate(dotPrefab) as GameObject;
                go.name = "Dot";
                Transform tr = go.transform;
                tr.parent = transform;
                tr.localScale = Vector3.one * scale * 10f;
                tr.localPosition = new Vector3((i - cols / 2f) * scale, (j - rows / 2f) * scale, 0f);
                Pixel px = go.GetComponent<Pixel>();
                grid[i, j] = px;
                if (i == 0 || j == 0 || i == cols - 1 || j == rows - 1)
                {
                    px.InitPixel(PIXELSTYLE.FILL);
                    px.SetColor(borderColor);
                }
                else
                {
                    px.InitPixel(PIXELSTYLE.EMPTY);
                }
            }
        {
            cursor = GetComponentInChildren<Cursor>();
            cursor.cols = cols;
            cursor.rows = rows;
            cursor.scale = scale;
            cursor.MoveCursor(cols / 2, 0);
        }
    }

    bool isFlooding = false;

    int countFill = 0;
    int countEmpty = 0;
    public int countType1 = 0;
    public int countType2 = 0;

    Point oldPoint;

    int CountArea(PIXELSTYLE ps)
    {
        int cnt = 0;
        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
                if (grid[i,j].Load() == ps) cnt++;
        return cnt;
    }

    void FillArea(PIXELSTYLE ps1, PIXELSTYLE ps2)
    {
        for (int i = 0; i < cols; i++)
            for (int j = 0; j < rows; j++)
                if (grid[i, j].Load() == ps1) grid[i, j].SafeSave(ps2);
    }

    void DoneFloodFill()
    {
        Debug.Log("DoneFloodFill");
        if (countType1 < countType2)
        {
            FillArea(PIXELSTYLE.TYPE1, PIXELSTYLE.FILL);
            FillArea(PIXELSTYLE.TYPE2, PIXELSTYLE.EMPTY);
        }
        else
        {
            FillArea(PIXELSTYLE.TYPE2, PIXELSTYLE.FILL);
            FillArea(PIXELSTYLE.TYPE1, PIXELSTYLE.EMPTY);
        }
        FillArea(PIXELSTYLE.PATH, PIXELSTYLE.FILL);
    }

    void FloodFill(Point p, PIXELSTYLE ps)
    {
        if (!isFlooding) return;
        if (grid[p.x, p.y].Load() != PIXELSTYLE.EMPTY)
        {
            if (countEmpty == countType1 + countType2)
            {
                isFlooding = false;
                DoneFloodFill();
            }
            return;
        }
        grid[p.x, p.y].SafeSave(ps);
        if (ps == PIXELSTYLE.TYPE1) countType1++;
        else countType2++;
        FloodFill(new Point(p.x - 1, p.y), ps);
        FloodFill(new Point(p.x + 1, p.y), ps);
        FloodFill(new Point(p.x, p.y - 1), ps);
        FloodFill(new Point(p.x, p.y + 1), ps);
    }

    void Paint(Point p, KeyCode k)
    {
        if (isFlooding) return;
        isFlooding = true;
        countType1 = 0;
        countType2 = 0;
        countEmpty = CountArea(PIXELSTYLE.EMPTY);
        if (k == KeyCode.UpArrow || k == KeyCode.DownArrow)
        {
            FloodFill(new Point(p.x-1, p.y), PIXELSTYLE.TYPE1);
            FloodFill(new Point(p.x+1, p.y), PIXELSTYLE.TYPE2);
        }
        else
        {
            FloodFill(new Point(p.x, p.y-1), PIXELSTYLE.TYPE1);
            FloodFill(new Point(p.x, p.y+1), PIXELSTYLE.TYPE2);
        }
    }

    void MoveCursor(Point p, KeyCode k)
    {
        Pixel px = grid[p.x, p.y];
        PIXELSTYLE ps = px.Load();
        if (ps == PIXELSTYLE.EMPTY)
        {
            cursor.MoveCursor(p.x, p.y);
            grid[p.x, p.y].SafeSave(PIXELSTYLE.PATH);
        }
        else if (ps == PIXELSTYLE.FILL)
        {
            cursor.MoveCursor(p.x, p.y);
            if (grid[oldPoint.x, oldPoint.y].Load() != PIXELSTYLE.FILL)
                Paint(oldPoint, k);
        }
        else if (ps == PIXELSTYLE.PATH)
        {
            // ERROR
        }
        else
        {
            // ERROR
        }
    }

    void Update()
    {
        Point p = new Point(cursor.x, cursor.y);
        if (Input.GetKey(KeyCode.UpArrow) && p.y < rows-1)
        {
            oldPoint = p;
            p.y++;
            MoveCursor(p, KeyCode.UpArrow);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && p.y > 0)
        {
            oldPoint = p;
            p.y--;
            MoveCursor(p, KeyCode.DownArrow);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && p.x < cols-1)
        {
            oldPoint = p;
            p.x++;
            MoveCursor(p, KeyCode.RightArrow);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && p.x > 0)
        {
            oldPoint = p;
            p.x--;
            MoveCursor(p, KeyCode.LeftArrow);
        }
    }
}
