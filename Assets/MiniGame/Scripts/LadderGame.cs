using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

struct Point
{
    public int x, y;
    public Point(int px, int py)
    {
        x = px;
        y = py;
    }
}

public class LadderGame : MonoBehaviour {
    int cols = 11, rows = 22;
    public int bet = 0;
    float width = 120f, height = 20f;
    public GameObject linePrefab;
    public Transform cursor, cover;
    public AudioSource soundSpin, soundStop, soundCash;

    bool[,] grid;

    bool isInput = true;

    List<Point> tlist;
    Button[] buttons;
    Button takeButton;
    Text betLabel;
    Text[] labels;
    List<Image> lines;
    int lineNo;

    int[] bonusList = new int[6] {10, 50, 100, 200, 500, 2000};

	void Start () {
        cursor.gameObject.SetActive(false);
        InitLadder();
        InitButtons();
	}

    void SetLane(int no)
    {
        if (!isInput) return;
        isInput = false;
        for (int i = 0; i < cols; i += 2)
        {
            if (i != no)
                buttons[i / 2].interactable = false;
            //buttons[i / 2].collider.enabled = false;
        }
        InitPath(no, 0);
        cursor.gameObject.SetActive(true);
        Point p = tlist[0];
        Vector3 pos = new Vector3((p.x / 2f - cols / 4f) * width, (rows / 2f - p.y - 0.5f) * height, 0f);
        cursor.localPosition = pos;
        StartCoroutine(DelayAction(0.1f, () =>
        {
            MoveCursor();
        }));
        soundSpin.Play();
        TweenParms parms = new TweenParms().Prop("localScale", Vector3.zero).Ease(EaseType.EaseInQuad);
        HOTween.To(cover, 0.5f, parms);
    }

    void MoveCursor()
    {
        SequenceParms sparams = new SequenceParms().OnComplete(DoneMoveCursor);
        Sequence mySequence = new Sequence(sparams);
        TweenParms parms;

        Vector3 pOld, pos;
        Point p = tlist[0];
        pos = new Vector3((p.x / 2f - cols / 4f) * width, (rows / 2f - p.y - 0.5f) * height, 0f);
        pOld = pos;
        cursor.localPosition = pos;
        lineNo = 0;
        lines[lineNo].enabled = true;
        lines[lineNo].rectTransform.pivot = new Vector2(0.5f, 1f); // UIWidget.Pivot.Top;
        lines[lineNo].rectTransform.sizeDelta = new Vector2(4, 4);
        for (int i = 1; i < tlist.Count; i++)
        {
            p = tlist[i];
            pos = new Vector3((p.x / 2f - cols / 4f) * width, (rows / 2f - p.y - 0.5f) * height, 0f);
            parms = new TweenParms().Prop("localPosition", pos).Ease(EaseType.Linear).OnComplete(DoneMoveCursorBit).OnUpdate(OnMoveCoursorBit);
            mySequence.Append(HOTween.To(cursor, 0.005f * Vector3.Distance(pos, pOld), parms));
            pOld = pos;
        }

        mySequence.Play();
 
    }

    void OnMoveCoursorBit()
    {
        Point p1 = tlist[lineNo];
        Point p2 = tlist[lineNo+1];
        Vector3 pos1 = new Vector3((p1.x / 2f - cols / 4f) * width, (rows / 2f - p1.y - 0.5f) * height, 0f);
        Vector3 pos2 = cursor.localPosition;
        int dist = Mathf.FloorToInt(Vector3.Distance(pos1, pos2));
        if (p1.x == p2.x) lines[lineNo].rectTransform.sizeDelta = new Vector2(4, dist);
        else lines[lineNo].rectTransform.sizeDelta = new Vector2(dist, 4);
    }

    void DoneMoveCursorBit()
    {
        lineNo++;
        if (lineNo >= lines.Count) return;
        Point p1 = tlist[lineNo];
        Point p2 = tlist[lineNo + 1];
        
        if (p1.x > p2.x) lines[lineNo].rectTransform.pivot = new Vector2(1f, 0.5f); // UIWidget.Pivot.Right;
        else if (p1.x < p2.x) lines[lineNo].rectTransform.pivot = new Vector2(0f, 0.5f); // UIWidget.Pivot.Left;
        else lines[lineNo].rectTransform.pivot = new Vector2(0.5f, 1f); // UIWidget.Pivot.Top;
        
        lines[lineNo].enabled = true;
        lines[lineNo].rectTransform.sizeDelta = new Vector2(4, 4);
    }

    void DoneMoveCursor()
    {
        cursor.gameObject.SetActive(false);
        soundSpin.Stop();
        soundStop.Play();
        soundCash.Play();
        takeButton.interactable = true;
        TweenBet(bonusList[tlist[tlist.Count - 1].x / 2]);
    }

    void InitPath(int px, int py)
    {
        tlist = new List<Point>();
        int x = px, y = py;
        tlist.Add(new Point(x, y));
        while (y < rows)
        {
            if (x - 1 > 0 && grid[x - 1, y]) 
            {
                x--;
                tlist.Add(new Point(x, y));
                x--;
                tlist.Add(new Point(x, y));
                y++;
                tlist.Add(new Point(x, y));
                continue;
            }
            if (x + 1 < cols - 1 && grid[x + 1, y])
            {
                x++;
                tlist.Add(new Point(x, y));
                x++;
                tlist.Add(new Point(x, y));
                y++;
                tlist.Add(new Point(x, y));
                continue;
            }
            y++;
            tlist.Add(new Point(x, y));
        }

        lines = new List<Image>(); 
        Point pOld = tlist[0], pos;
        foreach (Point p in tlist)
        {
            if (pOld.x == p.x && pOld.y == p.y) continue;
            bool isV = false;
            if (pOld.x == p.x) isV = true;
            GameObject go = Instantiate(linePrefab) as GameObject;
            go.name = "path" + p.x + "_" + p.y;
            Transform tr = go.transform;
            tr.SetParent(transform);
            tr.localScale = Vector3.one;
            tr.localPosition = Vector3.zero;
            if (pOld.x < p.x) pos = p;
            else pos = pOld;
            Image sprite = go.GetComponent<Image>();
            lines.Add(sprite);
            sprite.color = Color.red;
            if (isV)
            {
                tr.localPosition = new Vector3((pos.x / 2f - cols / 4f) * width, (rows / 2f - pos.y - 0.5f) * height, 0f);
                sprite.rectTransform.sizeDelta = new Vector2(4, Mathf.FloorToInt(height) + 4);
            }
            else
            {
                tr.localPosition = new Vector3((pos.x / 2f - cols / 4f - 0.25f - 0.25f * Mathf.Sign(p.x - pOld.x)) * width, (rows / 2f - pos.y - 0.5f) * height, 0f);
                sprite.rectTransform.sizeDelta = new Vector2(Mathf.FloorToInt(width / 2f) + 4, 4);
            }
            sprite.enabled = false;
            pOld = p;
        }
    }

    void InitLadder()
    {
        grid = new bool[cols, rows];
        for (int i = 0; i < cols; i += 2)
            for (int j = 0; j < rows; j++)
                grid[i, j] = true;
        for (int j = 1; j < rows-1; j++)
            for (int i = 1; i < cols; i += 2)
            {
                bool flag = Random.Range(0f, 2f) < 1f ? true : false;
                if (i - 2 > 0 && grid[i - 2, j]) flag = false;
                grid[i, j] = flag;
            }
        for (int i = 0; i < cols; i += 2)
        {
            GameObject go = Instantiate(linePrefab) as GameObject;
            go.name = "vLine" + i;
            Transform tr = go.transform;
            tr.SetParent(transform);
            tr.localScale = Vector3.one;
            tr.localPosition = new Vector3((i / 2f - cols / 4f) * width, 0f, 0f);
            Image sprite = go.GetComponent<Image>();
            sprite.rectTransform.sizeDelta = new Vector2(4, Mathf.FloorToInt(height * (rows - 1)));
        }
        for (int i = 1; i < cols; i += 2)
            for (int j = 1; j < rows - 1; j++)
            {
                if (!grid[i, j]) continue;
                GameObject go = Instantiate(linePrefab) as GameObject;
                go.name = "hLine" + i + "_" + j;
                Transform tr = go.transform;
                tr.SetParent(transform);
                tr.localScale = Vector3.one;
                tr.localPosition = new Vector3((i / 2f - cols / 4f) * width, (rows / 2f - j - 0.5f) * height, 0f);
                Image sprite = go.GetComponent<Image>();
                sprite.rectTransform.sizeDelta = new Vector2(Mathf.FloorToInt(width) + 4, 4);
            }
    }

    void InitButtons()
    {
        betLabel = transform.parent.Find("Win/Label").GetComponent<Text>();
        buttons = transform.parent.Find("Buttons/Choice").GetComponentsInChildren<Button>();
        takeButton = transform.parent.Find("Buttons/Take").GetComponentInChildren<Button>();
        takeButton.interactable = false;
        labels = transform.parent.Find("Results").GetComponentsInChildren<Text>();
        for (int i = 0; i < 6; i++)
        {
            labels[i].text = bonusList[i].ToString("#,##0");
            //labels[i].transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }

    void OnUpdateBet()
    {
        betLabel.text = bet.ToString("#,##0");
    }

    void TweenBet(int val)
    {
        TweenParms parms = new TweenParms().Prop("bet", val).Ease(EaseType.Linear).OnUpdate(OnUpdateBet);
        HOTween.To(this, 0.5f, parms);
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

	void Update () {
	
	}
    
    IEnumerator DelayAction(float dTime, System.Action callback)
    {
        yield return new WaitForSeconds(dTime);
        callback();
    }
}
