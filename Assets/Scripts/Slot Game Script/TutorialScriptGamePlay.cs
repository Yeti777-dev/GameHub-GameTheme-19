using UnityEngine;
using System.Collections;

public class TutorialScriptGamePlay : MonoBehaviour
{
    public static TutorialScriptGamePlay instance;
    public TextMesh infoText;
    public string[] instructionText;

    public GameObject[] arrows;

    public GameObject lineArrow;
    public GameObject betAmountArrow;
    public GameObject payTableArrow;
    public GameObject spinArrow;


    public int instructionIndex;

    internal static int instructionShown = 0;

    bool enableTouch = false;
    private GameObject payTableGo;
    public GameObject stripGo;


    void Awake()
    {
        instance = this;

    }


    void Start()
    {
        transform.position = new Vector3(50, 0, -3f);

        iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;
        iTween.MoveFrom(gameObject, transform.position - new Vector3(0, 4, 0), 1f);

        HideAllArrows();

        instructionIndex = 0;
        infoText.text = instructionText[0];
        Invoke("EnableTouch", 2f);
        payTableGo = GameObject.Find("PayTable Button");
    }

    void EnableTouch()
    {
        enableTouch = true;
    }

    void HideAllArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (!enableTouch)
            return;

        instructionIndex++;
        ChangeInstruction();

        Invoke("EnableTouch", 2f);
    }

    private void ChangeInstruction()
    {
        if (instructionIndex >= instructionText.Length)
        {
            Destroy(gameObject);
            return;
        }

        iTween.PunchScale(infoText.gameObject, new Vector3(.2f, .2f, .2f), 1f);

        infoText.text = instructionText[instructionIndex];

        if (instructionIndex == 1)
        {
            HideAllArrows();
            lineArrow.SetActive(true);
        }
        else if (instructionIndex == 2)
        {
            HideAllArrows();
            betAmountArrow.SetActive(true);
        }
        else if (instructionIndex == 3)
        {
            HideAllArrows();
            payTableGo.transform.localPosition = new Vector3(payTableGo.transform.localPosition.x, payTableGo.transform.localPosition.y, -3);
            GetComponent<Collider>().enabled = false;
            payTableArrow.SetActive(true);
        }
        else if (instructionIndex == 4)
        {
            HideAllArrows();
            spinArrow.SetActive(true);
        }

    }

    internal void EnableTutorail()
    {

        infoText.gameObject.SetActive(true);
        stripGo.SetActive(true);
        GetComponent<Collider>().enabled = true;
        instructionIndex++;
        ChangeInstruction();
    }

    internal void ResetPayTableButtonPosition()
    {
        HideAllArrows();
        infoText.gameObject.SetActive(false);
        stripGo.SetActive(false);
        payTableGo.transform.localPosition = new Vector3(payTableGo.transform.localPosition.x, payTableGo.transform.localPosition.y, 0);
    }
}
