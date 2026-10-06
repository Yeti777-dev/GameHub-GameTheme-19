using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {
    public int x, y;
    public Transform tr;
    public float scale = 8f;
    public int cols = 100;
    public int rows = 70;

    void Start()
    {
        tr = transform;
    }

    public void MoveCursor(int px, int py)
    {
        x = px;
        y = py;
        tr.localPosition = new Vector3((x - cols / 2f) * scale, (y - rows / 2f) * scale, 0f);
    }
}
