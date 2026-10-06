using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tubroAnimation : MonoBehaviour
{
    public AudioClip TurboOnSound,TurboOffSound;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    public void TurboOnAnim()
    {
        GetComponent<Animator>().SetTrigger("turboOn");
        source.clip = TurboOnSound;
        source.Play();
    }
    public void TurboOffAnim()
    {
        GetComponent<Animator>().SetTrigger("turboOff");
        source.clip = TurboOffSound;
        source.Play();
    }
}
