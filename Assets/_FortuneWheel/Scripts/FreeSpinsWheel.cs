namespace Curiologix
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using System;
    using gm.api.domain.Models;

    public class FreeSpinsWheel : GamePlaySocketManager
    {
        [Header("UI Properties")]

        public Button spinButton;
        public const string COINS_COUNT = "COINS_COUNT";
        [Space(20)]
        [Header("Fortune Wheel Properties")]
        public Transform wheelToRotate;


        int _selectReward, count = 0, cost = 300;
        float _coins;
        AudioSource[] audSource;
        Sprite[] dots = new Sprite[2];
        bool spinning;
        float anglePerReward;

        public int MaxRewardIndex;

        private int RewardIndex;



        public FreespinwheelConfig config;
        internal int rewardCount { get { return config.prizes.Length; } }

        public float Coins
        {
            get { return _coins; }
            set
            {
                _coins = value;
            }
        }
        public void OnEnable()
        {
           // bgsoundmanager.instance.StopMainBG();
           spinButton.gameObject.SetActive(true);
            Connect();
            //  rewardCount = config.prizes.Length;
            Invoke("LateSpinAct", 1);
        }
        public int SelectedReward
        {
            get
            {
                return _selectReward;
            }
            set
            {
                _selectReward = Mathf.Clamp(value, 0, config.prizes.Length);
               
            }
        }
        public void onClickSpinNow()
        {

          //  SpinType.ResetTimer(SpinType.wheelType);
                spinButton.interactable = false;
            spinButton.gameObject.SetActive(false);
                StartSpin();
            
        }

     

      

        void Awake()
        {
          
           
            dots[0] = config.illuminatiDots[0];
            dots[1] = config.illuminatiDots[1];
            Coins = PlayerPrefs.GetInt(COINS_COUNT, 3000);
            spinButton.onClick.AddListener(onClickSpinNow);
            spinning = false;
            anglePerReward = 360 / rewardCount;
           
           
            audSource = new AudioSource[5];
            for (int i = 0; i < 5; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                audSource[i] = source;
            }
           
        }

        void LateSpinAct() {
            spinButton.interactable = true;


        }
        private int[] winspots = { 1, 5, 11,15 };
        internal int TargetToStopOn { get { return winspots[UnityEngine.Random.Range(0, winspots.Length)]; } }
        public void StartSpin()
        {
            if (!spinning)
            {
                RewardIndex = TargetToStopOn;
             
                       // InvokePlayGame(GameListManager.Instance.FortuneGameDataLeft.Id,GameListManager.Instance.FortuneGameDataLeft.Name,0);
                float maxAngle = 360 * config.speedMultiplier + RewardIndex * anglePerReward;
                print("Max Angle is " + maxAngle);
                print("wheel started");
                StartCoroutine(RotateWheel(config.duration, maxAngle));
            }
        }
        IEnumerator RotateWheel(float time, float maxAngle)
        {
            GetComponent<AudioSource>().Play();
            spinning = true;
            float timer = 0.0f;
            float startAngle = wheelToRotate.transform.eulerAngles.z;
            maxAngle = maxAngle - startAngle;
            while (timer < time)
            {
                //to calculate rotation
                float angle = maxAngle * config.animationCurve.Evaluate(timer / time);
                wheelToRotate.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += Time.deltaTime;
                yield return 0;
            }
            GetComponent<AudioSource>().Stop();
            wheelToRotate.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
            StartCoroutine(ShowHideParticles());
            BonusWheelManager.Instance.FreeGamesWon = (int) config.prizes[RewardIndex];

        }
        void GivePrize()
        {
            spinning = false;

            BonusWheelManager.Instance.HideWheelGraphics();
         

          
        }

     
        IEnumerator ShowHideParticles()
        {
            yield return new WaitForSeconds(0.5f);
            GivePrize();
        }
        private IEnumerator IncrementCoroutine(Text l, int targetValue, int startingValue = 0)
        {
            float time = 0;
            l.text = startingValue.ToString();
            float incrementTime = 1.5f;
            while (time < incrementTime)
            {
                yield return null;
                time += Time.deltaTime;
                float factor = time / incrementTime;
                l.text = Mathf.Lerp(startingValue, targetValue, factor).ToString("F2");
            }
            l.text = targetValue.ToString("F2");
            yield break;
        }
        internal void HitStart(SpriteRenderer sp)
        {
            PlayHitClip();
        }
       

      
   
        public void PlayHitClip()
        {
            for (int i = 0; i < audSource.Length; i++)
            {
                if (!audSource[i].isPlaying)
                {
                    audSource[i].clip = config.tingSound;
                    audSource[i].Play();
                    break;
                }
            }
        }
      
       
    }
}