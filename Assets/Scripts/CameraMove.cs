using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public ScrollRect scrollbar; // assign in the inspector
    public float Horizental_X;

  
    public float Speed =1f;
    public float PreviousValue =0;

    private void Awake()
    {
   

        if (scrollbar != null)
        {
            scrollbar.onValueChanged.AddListener(onScroll);
        }
    }

    void Update() {

    
    }



    private void onScroll(Vector2 value)
    {
        Speed = scrollbar.content.GetComponentsInChildren<GameData>().GetLength(0) -1;
        if (Speed < 0)
            Speed = 0;
        Horizental_X = scrollbar.horizontalNormalizedPosition *Speed;



        Horizental_X = (float) System.Math.Round(Horizental_X, 2);

        float diff = Horizental_X - PreviousValue;

       

        Vector3 currentPos = transform.position;
        currentPos.x = Horizental_X;
      
       transform.position = currentPos;
    }


 
}
