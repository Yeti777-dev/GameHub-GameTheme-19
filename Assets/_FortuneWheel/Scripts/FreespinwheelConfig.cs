namespace Curiologix
{
    using System;
    using UnityEngine;
    public class FreespinwheelConfig : MonoBehaviour
    {
        public float[] prizes = new float[8];
        public AudioClip tingSound;
        [Range(1, 10)]
        public int speedMultiplier;
        [Range(2, 15)]
        public int duration;
        public bool timedTurn;
        public Sprite[] illuminatiDots;
        public AnimationCurve animationCurve;

        public int FreeGameWon = 0;
      //  private static FortuneWheelConfig _instance;
        //public static FortuneWheelConfig Instance
        //{ get { if (_instance == null) _instance = FindObjectOfType<FortuneWheelConfig>(); return _instance; } }

        public static string GetValueFormated(float val)
        {
            return String.Format("{0,00}", val);
        }
       
    }
}