using UnityEngine;
using System.Collections;

public class SlotItemEffects : MonoBehaviour 
{
    public SlotItem mySlotItemScript;
    internal bool isHighlighting = false;

	// Use this for initialization
	void Start () 
    {
	
	}



    internal void Highlight(int val)
    {
        isHighlighting = true;

        int animationIndex;
        animationIndex = mySlotItemScript.animationIndex;
       // mySlotItemScript.ItemPackedSprite.PlayAnim(animationIndex);

        if (animationIndex < mySlotItemScript.audios.Length)
        {
            mySlotItemScript.GetComponent<AudioSource>().clip = mySlotItemScript.audios[animationIndex];
            mySlotItemScript.GetComponent<AudioSource>().Play();
            
        }







        if (val == 0)
        {
            iTween.Defaults.easeType = iTween.EaseType.easeInExpo;
            iTween.PunchScale(gameObject, new Vector3(.2f, .2f, .2f), 2f);
        }
        else if (val == 1)
        {
            iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;
            iTween.ScaleFrom(gameObject, new Vector3(0, transform.localScale.y, transform.localScale.z), 1f);
        }
        else if (val == 2)
        {
            iTween.Defaults.easeType = iTween.EaseType.easeOutExpo;
            iTween.ScaleFrom(gameObject, new Vector3(transform.localScale.x, 0, transform.localScale.z), 1f);
        }
    }


    void DoAnimation()
    {
        iTween.Defaults.easeType = iTween.EaseType.linear;
        iTween.RotateFrom(gameObject, new Vector3(0, 90, 0), .5f);
    }


    internal void UnHighlight()
    {
        CancelInvoke("DoAnimation");
        isHighlighting = false;

      //  mySlotItemScript.ItemPackedSprite.PauseAnim();
      //  mySlotItemScript.ItemPackedSprite.SetFrame(mySlotItemScript.animationIndex, 0);
    }
}
