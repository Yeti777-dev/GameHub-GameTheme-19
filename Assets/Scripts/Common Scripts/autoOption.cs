using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoOption : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;
    public int InputParameter;
    private Vector3 InitialScale;
    public GameObject highlightPanel;
    public static bool OnceClicked;
    public Color highlight_Color = Color.green;
    private TextMesh spRenderer;
    private Color OriginalColor;
    private void Start()
    {
        spRenderer = GetComponent<TextMesh>();
        InitialScale = transform.localScale;
        OriginalColor = spRenderer.color;
    }
    private void OnEnable()
    {
        OnceClicked = false;
    }
    private void OnMouseEnter()
    {
        spRenderer.color = highlight_Color;
    }
    private void OnMouseExit()
    {
        spRenderer.color = OriginalColor;
    }

    private void OnMouseDown()
    {
        if (OnceClicked)
            return;
        if (transform.name == "Spin Button")
        {
            transform.localScale = new Vector3(InitialScale.x + (InitialScale.x / 2), InitialScale.y + (InitialScale.y / 2), 1f);
            GetComponent<AudioSource>().Play();

        }
        else
        {
            transform.localScale = new Vector3(InitialScale.x + (InitialScale.x / 8), InitialScale.y + (InitialScale.y / 8), 1f);

        }

    }
    void OnMouseUp()
    {
        if (OnceClicked)
            return;
        transform.localScale = InitialScale;
        GUIManager.instance.totalAutoSpins = GUIManager.instance.SpinNumbers + InputParameter;
        GUIManager.instance.SpinNumbers = GUIManager.instance.totalAutoSpins;
        if (UIManagerScript.isVideoPannelShowing)
            return;
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, .1f);
        OnceClicked = true;    }
}
