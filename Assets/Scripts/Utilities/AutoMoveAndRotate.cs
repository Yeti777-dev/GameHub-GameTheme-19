using System;
using UnityEngine;

    public class AutoMoveAndRotate : MonoBehaviour
    {
        public static AutoMoveAndRotate instance;
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
        private float m_LastRealTime;
        public float speedvalue;
    public float TurboSpeed;
        public bool isRotate = false;
        private void Start()
        {
            m_LastRealTime = Time.realtimeSinceStartup;
            instance = this;
        }


        // Update is called once per frame
        private void Update()
        {
            if (!isRotate)
                return;
            float deltaTime = Time.deltaTime;
            speedvalue -= Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }

        // transform.Translate(moveUnitsPerSecond.value*deltaTime, moveUnitsPerSecond.space);
        if (speedvalue > 0) {
            if (!GUIManager.instance.TurboBool)
                transform.Rotate(rotateDegreesPerSecond.value * speedvalue, moveUnitsPerSecond.space);
        else
                transform.Rotate(rotateDegreesPerSecond.value * TurboSpeed, moveUnitsPerSecond.space);
        }
    }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }

// -2.546, -1.363, -0.207,0.905, 2.052