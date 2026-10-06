using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundFxManager : MonoBehaviour 
{
    public static SoundFxManager instance;
    public AudioSource buttonTapSound;
    public AudioSource spinSound;
    public AudioSource NormalBgSound;
    public AudioSource FreeSpinsBgSound;
    public AudioSource BonusBg;

    public AudioSource columnSpinCompleteSound;
    public AudioSource BonusItemAppear;
    public AudioSource ScatterItemAppear;
    public AudioSource HighRichItemAppear;
    public AudioSource WinSound;

    public AudioSource MaxLineButtonSound;

    public AudioSource singleLineIndicationSound;
    public AudioSource TreasureFUll;

    public AudioSource WinBigSound;
 
    public AudioSource TurboOn;
    public AudioSource TurboOff;
    public AudioSource AutoSpinUp;
    public AudioSource AutoSpinDown;
    public AudioSource FlippingSound;
    public AudioSource WildExpandedSound;
    public AudioSource ReelBoomSound;
    public AudioSource BonusPopUpSound;
    public AudioSource FreeSpinsPopUpSound;
    public AudioSource Shuffling;
    public AudioSource AlphabetAppearSound;
   
    public List<AudioSource> allClips = new List<AudioSource>();
	
    void Awake()
    {
        instance = this;
    }

	void Start () {
        allClips.Add(NormalBgSound);
        NormalBgSound.Play();
        allClips.Add(FreeSpinsBgSound);

        allClips.Add(buttonTapSound);
		allClips.Add(spinSound);
       
        allClips.Add(columnSpinCompleteSound);
		allClips.Add(BonusItemAppear);
        allClips.Add(ScatterItemAppear);
        allClips.Add(HighRichItemAppear);
        allClips.Add(WinSound);
		allClips.Add(MaxLineButtonSound);
		allClips.Add(singleLineIndicationSound);
		allClips.Add(WinBigSound);
        allClips.Add(TurboOn);
        allClips.Add(TurboOff);
        allClips.Add(AutoSpinUp);
        allClips.Add(AutoSpinDown);
        allClips.Add(BonusBg);
        allClips.Add(FlippingSound);
        allClips.Add(WildExpandedSound);
        allClips.Add(ReelBoomSound);
        allClips.Add(BonusPopUpSound);
        allClips.Add(Shuffling);
        allClips.Add(AlphabetAppearSound);
        allClips.Add(FreeSpinsPopUpSound);
       
            if (PlayerPrefs.GetInt("Sound", 1) == 1)
                AudioListener.volume = 1;
            else
                AudioListener.volume = 0;
       // if (bgsoundmanager.instance)
          //  bgsoundmanager.instance.StopMainBG();
        PlayNormalBg();
       
    }


    public void MuteAllClips(){
		for(int i = 0; i < allClips.Count; i++){
			if(allClips[i])
				allClips[i].mute = !allClips[i].mute;
		}
	}

    public void PlayNormalBg() {
        BonusBg.Stop();
     
        NormalBgSound.Play();
        FreeSpinsBgSound.Stop();
    }

    public void PlayBonusBg() {
        BonusBg.Play();
     
        NormalBgSound.Stop();
        FreeSpinsBgSound.Stop();

    }
    public void PlayFreeSpinBg()
    {
        BonusBg.Stop();
         NormalBgSound.Stop();
       
        FreeSpinsBgSound.Play();

    }
    public void StopBgSoundOnly() {
       NormalBgSound.Stop();
     
    }
}
