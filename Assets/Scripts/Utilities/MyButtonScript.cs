using UnityEngine;
using System.Collections;
//using ChartboostSDK;

public class MyButtonScript : MonoBehaviour
{
    public MonoBehaviour scriptToCall;
    public string methodToInvoke;

    private Vector3 InitialScale;
    public GameObject spinParticle;
    public Texture2D blurryImage, ClearImage;
    public bool IsSpinAnim = false;
    public Animator SpinAnim;
    private void Start()
    {
        InitialScale = transform.localScale;

    }
    private void OnMouseDown()
    {

        if (TryGetComponent<AudioSource>(out AudioSource source))
        {

            source.Play();

        }
        else
        {
            //  transform.localScale = new Vector3(InitialScale.x + (InitialScale.x / 8), InitialScale.y + (InitialScale.y / 8), 1f);
            iTween.PunchScale(transform.gameObject, transform.localScale * 0.2f, 0.2f);
        }
        if (IsSpinAnim)
        {
            SpinAnim.SetTrigger("spin");
        }

    }

    public void blurTheImage() {
        // GetComponent<Renderer>().material.mainTexture = blurryImage;
        GetComponent<Renderer>().enabled = false;
    }
    public void ClearTheImage()
    {
        GetComponent<Renderer>().enabled = true;
        //  GetComponent<Renderer>().material.mainTexture = ClearImage;
    }

    void OnMouseUp()
    {
        transform.localScale = InitialScale;
     
        if (scriptToCall != null)
            scriptToCall.Invoke(methodToInvoke, 0.01f);
    }
}
