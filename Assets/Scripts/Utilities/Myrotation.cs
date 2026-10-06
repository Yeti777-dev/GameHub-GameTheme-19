using System;
using UnityEngine;

    public class Myrotation : MonoBehaviour
    {
        public static AutoMoveAndRotate instance;
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
        private float m_LastRealTime;
        public float speedvalue;
        public bool isRotate = false;
        private void Start()
        {
            m_LastRealTime = Time.realtimeSinceStartup;
        }


        // Update is called once per frame
        private void Update()
        {
            if (!isRotate)
                return;
            float deltaTime = Time.deltaTime;
           
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }

           // transform.Translate(moveUnitsPerSecond.value*deltaTime, moveUnitsPerSecond.space);
           
            transform.Rotate(rotateDegreesPerSecond.value, moveUnitsPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }

