using UnityEngine;
using System.Collections;

public class ImageAnimator : MonoBehaviour {

	public Texture[] frames;
	public float delay ;
    public bool doOnce;

    public bool invokeFunction;
    public int invokeAtFrame;
    public MonoBehaviour scriptToCall;
    public string[] methodToInvoke;
   
    public bool stop = true;
    int index = 0;
    float counter;

	void Update() {

        if (doOnce)
            if (frames.Length - 1 == index)
                stop = true;

        if (!stop) {
            counter += Time.deltaTime;
            if(counter > delay) {
                counter = 0;
                index++;

                if(invokeFunction)
                  if(index == invokeAtFrame)
                        if (scriptToCall != null)
                            for(int i = 0; i< methodToInvoke.Length; i++)
                                scriptToCall.Invoke(methodToInvoke[i], .1f);

                if (index == frames.Length)
                    index = 0;

    	        GetComponent<Renderer>().material.mainTexture = frames[index];
            }
        }
    }
}
