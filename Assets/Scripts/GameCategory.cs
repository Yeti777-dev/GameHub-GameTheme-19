using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCategory : MonoBehaviour
{
   public string categoryName;
  

    public void OnCategorySelect() {

        GameListManager.Instance.GamesToShow(categoryName);
    
    }
}
