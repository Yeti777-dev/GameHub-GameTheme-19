using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[InitializeOnLoad]
public class uGUITools
{
    [MenuItem("uGUI/Anchors to Corners %`")]
    static void AnchorsToCorners()
    {
        Image[] CanvasChildren = Canvas.FindObjectsOfType<Image>();
        foreach (Image trans in CanvasChildren)
        {
            RectTransform t = trans.transform.GetComponent<RectTransform>();
            RectTransform pt = trans.transform.parent.transform.GetComponent<RectTransform>();
            // RectTransform t = Selection.activeTransform as RectTransform;
            //RectTransform pt = Selection.activeTransform.parent as RectTransform;


            if (t == null || pt == null) return;

            t.localScale = new Vector3(1, 1, 1);
            pt.localScale = new Vector3(1, 1, 1);
            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }

    [MenuItem("uGUI/Transform to Corners %`")]
    static void TransformToCorners()
    {
        Text[] CanvasChildren = Canvas.FindObjectsOfType<Text>();
        foreach (Text trans in CanvasChildren)
        {
            RectTransform t = trans.transform.GetComponent<RectTransform>() ;
            RectTransform pt = trans.transform.parent.transform.GetComponent<RectTransform>();
            // RectTransform t = Selection.activeTransform as RectTransform;
            //RectTransform pt = Selection.activeTransform.parent as RectTransform;


            if (t == null || pt == null) return;

            t.localScale = new Vector3(1, 1, 1);
            pt.localScale = new Vector3(1, 1, 1);
            Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                                t.anchorMax.y + t.offsetMax.y / pt.rect.height);

            t.anchorMin = newAnchorsMin;
            t.anchorMax = newAnchorsMax;
            t.offsetMin = t.offsetMax = new Vector2(0, 0);
        }
    }

    //[MenuItem("uGUI/Corners to Anchors %]")]
    //static void CornersToAnchors()
    //{
    //    RectTransform t = Selection.activeTransform as RectTransform;

    //    if (t == null) return;

    //    t.offsetMin = t.offsetMax = new Vector2(0, 0);
    //}
}