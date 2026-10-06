using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using gm.api.domain;

public class LoginScript : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button login;
    private GmWebSocketResponse<GmLoginResponse> loginResponse;
    public GameObject round_Load;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        login.interactable = false;
        username.text = "user2";
        password.text = "Admin101!";
     //   GameSocketManager.Instance.Connect();
     
    }


    // Update is called once per frame
    void Update()
    {
        if (this.loginResponse != null) {
            if (loginResponse.HasError)
            {
                //show error message with dialog
                DisplayErrorOnScreen.Instance.DisplayError("Login Error", loginResponse.ErrorMessage);
                // reset login response to null
                loginResponse = null;
                round_Load.SetActive(false);
            }
            else {

               
                SceneManager.LoadScene("02_GameList");
            }

        }

        if ((username.text.Length >= 3) && (password.text.Length >= 3))
        {

            login.interactable = true;
        }
        else
            login.interactable = false;

    }

    private void Awake()
    {
        GameSocketManager.OnAuthenticated += (res) => {
            this.loginResponse = res;
        };
    }


    public void Login()
    {
        print("login pressed");
        round_Load.SetActive(true);
        GameSocketManager.Instance.LoginAsync(username.text,password.text);
       
    }


}
