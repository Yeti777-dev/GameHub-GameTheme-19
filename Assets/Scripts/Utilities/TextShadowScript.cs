using UnityEngine;
using System.Collections;

public class TextShadowScript : MonoBehaviour 
{

   // public GameObject textBackPrefab;

    GameObject textBackObject;
    TextMesh backTextMesh;
    TextMesh frontTextMesh;

    public Color frontColor = Color.yellow;
    public Color backColor = Color.black;
    public bool backToFornt = false;
    public bool fastUpdate = false;

    public Vector3 shadowShift;

    void Awake()
    {
        frontTextMesh = GetComponent<TextMesh>();

        frontTextMesh.GetComponent<Renderer>().material.color = frontColor;
    }

	void Start()
    {
        

        if(shadowShift != Vector3.zero)
            GenerateBackText();
        
	}

    void GenerateBackText()
    {
        textBackObject = (GameObject)Instantiate(gameObject, transform.position, Quaternion.identity);
        Destroy(textBackObject.GetComponent<TextShadowScript>());
        
        textBackObject.transform.parent = transform;
        textBackObject.transform.localScale = new Vector3(1,1,1);

        textBackObject.transform.rotation = transform.rotation;

        if (backToFornt)
        {
            textBackObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .01f);
            textBackObject.transform.position -= shadowShift;
        }
        else
        {
            textBackObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - .01f);
            textBackObject.transform.position += shadowShift;
        }

        backTextMesh = textBackObject.GetComponent<TextMesh>();

        backTextMesh.GetComponent<Renderer>().material.color = backColor;
        backTextMesh.text = frontTextMesh.text;
        backTextMesh.alignment = frontTextMesh.alignment;
        backTextMesh.anchor = frontTextMesh.anchor;
        
        if(fastUpdate)
            InvokeRepeating("UpdateFigure", 0f, .02f);
        else
            InvokeRepeating("UpdateFigure", 0f, .1f);
    }
	
	
	void UpdateFigure() 
    {
        
        backTextMesh.text = frontTextMesh.text;
	
	}
}
