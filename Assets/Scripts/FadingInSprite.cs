using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadingInSprite : MonoBehaviour
{
    public float minimum = 0.0f;
    public float maximum = 1f;
    public float duration = 5.0f;
    private float startTime;
    private SpriteRenderer sprite;
    private bool FadeIn = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    
        startTime = Time.time;
        FadeIn = true;
    }
    void Update()
    {
        if (FadeIn)
        {
            float t = (Time.time - startTime) / duration;
            sprite.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(minimum, maximum, t));

        }
    }
}
