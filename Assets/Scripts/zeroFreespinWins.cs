using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class zeroFreespinWins : MonoBehaviour
{
    public TextMeshPro Greetings;
    public GameObject youwonTextmesh;
    public GameObject CoinsParticles;
    private string[] winstext = { "BIG WIN!", "FABULOUS!" , "UNBELIEVABLE!!","SUPER!!!","JUST WOW!","SENSATIONAL!","EXCEPTIONAL!","SUPER","" };
    // Start is called before the first frame update
    private AudioSource source;
    void Awake()
    {

        if (TryGetComponent<AudioSource>(out AudioSource source))
        {

            source.playOnAwake = false;

        }

    }
    void Start()
    {
        if (SlotManager.instance.FreeSpinsWinSum == 0)
        {
            youwonTextmesh.SetActive(false);
            Greetings.text = "Better Luck Next Time";
            CoinsParticles.SetActive(false);
          //  Destroy(gameObject, 3f);

        }
        else {

            if (SlotManager.instance.FreeSpinsWinSum >= 5 && SlotManager.instance.FreeSpinsWinSum > (SlotManager.instance.totalBetAmount *5))
            {
                if (TryGetComponent<AudioSource>(out AudioSource source))
                {

                    source.Play();

                }
                youwonTextmesh.SetActive(false);
                Greetings.text = winstext[Random.Range(0, winstext.Length)];
                youwonTextmesh.GetComponent<TextMeshPro>().text = "";
                CoinsParticles.SetActive(true);
            }
            else {
                CoinsParticles.SetActive(true);
                Greetings.text = "";
                youwonTextmesh.GetComponent<TextMeshPro>().text = "";
            }
        }
    }

}
