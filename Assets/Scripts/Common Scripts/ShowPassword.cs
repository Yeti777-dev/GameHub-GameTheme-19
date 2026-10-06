using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPassword : MonoBehaviour
{

    public Sprite show, hide;
    public Image PassDisplayOpt;
    // Start is called before the first frame update
    void Start()
    {
        PassDisplayOpt.sprite = show;
    }
 
        [SerializeField] private InputField userPassword;

        public void ShowUserPassword()
        {
            if (userPassword.contentType == InputField.ContentType.Password)
            {
                userPassword.contentType = InputField.ContentType.Standard;
            PassDisplayOpt.sprite = hide;
            }
            else
            {
                userPassword.contentType = InputField.ContentType.Password;
            PassDisplayOpt.sprite = show;
            }
            userPassword.ForceLabelUpdate();
        }
    
}
