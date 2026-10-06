using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


public class ApiManager : MonoBehaviour
{
    private string BaseUrl = "https://apis.win777slots.com/";
    private string RedirectUrl = "https://luxorgames.com/games";
    private string sessionCheckEncrypted = "api/games/nQTslNDVjGfjBYalC5F4bu85dXYclBEu";
    private string getUserBalance = "api/games/9Du8id1afU7KdKA1mSk8JYOZ4s8gC4JE";
    private string updateWallet = "api/games/fh1jNfCMKLX3tPkX7ULrmCbR2Ba7aJjK";
    private string exit = "api/games/WM9WgF1TqEr87RzlfHjytQnLQ2l971z4";
    private string addCredit = "api/games/5hhdv6cDxKV2MrSyDLfUD0A4o6m7tesy";
    private string getJackpots = "api/games/external/getJackpots";
    private string MaxRTPUrl = "api/games/yDJi0ESXh1nZ0BB9UvWpo7PKUATBeuBE";

    public GameObject ErrorPanel, LoadingPanel, AddCredits, ClosePanel, NormalButtons, ButtonsInBonus;
    public TMP_Text ErroMessage, AvailableCredits;
    public TMP_InputField CreditInputField;
    private string luxorGameUid, luxorToken, luxorWalletUid, luxorUserUid, env;
    public int TargetRTP;
    public static ApiManager instance;

    public float TotalbetPlaced, TotalWinings, Balance, MainWalletBalance, AddAmount;

    public bool maxRtpExceed, is_bonus;
    public string PayoutString, EventID;
    public Button ExitYes, ExitNo;
    [DllImport("__Internal")]
    private static extern void closewindow(string env);

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        DontDestroyOnLoad(this);

#if UNITY_EDITOR
        var uri = new Uri("https://dc0kwxop7vx27.cloudfront.net/index.html?luxorGameUid=d1b9c760-e0b7-492f-83da-747744d1d335&luxorToken=a2c279b7-e2cd-4414-ba97-b79528317ada&luxorWalletUid=930f57e3-85b4-43ed-b994-65610696bf1b&luxorUserUid=757d13d4-fece-42f9-86c6-a57656c98cb3&base_url=apis.win777slots.com/&redirect_url=win777slots.com/games&env=2 ");
        var query = HttpUtility.ParseQueryString(uri.Query);
        luxorGameUid = query.Get("luxorGameUid");
        luxorToken = query.Get("luxorToken");
        luxorWalletUid = query.Get("luxorWalletUid");
        luxorUserUid = query.Get("luxorUserUid");
        BaseUrl = query.Get("base_url");
        RedirectUrl = query.Get("redirect_url");
        env = query.Get("env");
        //print("luxorGameUid xxxxxxxxxxxxx = " + luxorGameUid);
        //print("luxorToken xxxxxxxxxxxxx = " + luxorToken);
        //print("luxorWalletUid xxxxxxxxxxxxx = " + luxorWalletUid);
        //print("luxorUserUid xxxxxxxxxxxxx = " + luxorUserUid);
        //print("base_urlxxxxxxxxxxxxx = " + BaseUrl);
        //print("redirect_url xxxxxxxxxxxxx = " + RedirectUrl);
#else
        var uri = new Uri(Application.absoluteURL.ToString());
        var query = HttpUtility.ParseQueryString(uri.Query);
        luxorGameUid = query.Get("luxorGameUid");
        luxorToken = query.Get("luxorToken");
        luxorWalletUid = query.Get("luxorWalletUid");
        luxorUserUid = query.Get("luxorUserUid");
        BaseUrl = query.Get("base_url");
        RedirectUrl = query.Get("redirect_url");
        env = query.Get("env");
#endif

        StartCoroutine(SessionStart());
    }


    public void LogInBut()
    {

        if (SceneManager.GetActiveScene().name != "Play")
            SceneManager.LoadScene("Play");
    }


    public string OpenSSLEncrypt(string plainText, string passphrase)
    {
        // generate salt
        byte[] key, iv;
        byte[] salt = new byte[8];
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        rng.GetNonZeroBytes(salt);
        DeriveKeyAndIV(passphrase, salt, out key, out iv);
        // encrypt bytes
        byte[] encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
        // add salt as first 8 bytes
        byte[] encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + 8];
        Buffer.BlockCopy(Encoding.ASCII.GetBytes("Salted__"), 0, encryptedBytesWithSalt, 0, 8);
        Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, 8, salt.Length);
        Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + 8, encryptedBytes.Length);
        // base64 encode
        return Convert.ToBase64String(encryptedBytesWithSalt);
    }

    public string OpenSSLDecrypt(string encrypted, string passphrase)
    {
        // base 64 decode
        byte[] encryptedBytesWithSalt = Convert.FromBase64String(encrypted);
        // extract salt (first 8 bytes of encrypted)
        byte[] salt = new byte[8];
        byte[] encryptedBytes = new byte[encryptedBytesWithSalt.Length - salt.Length - 8];
        Buffer.BlockCopy(encryptedBytesWithSalt, 8, salt, 0, salt.Length);
        Buffer.BlockCopy(encryptedBytesWithSalt, salt.Length + 8, encryptedBytes, 0, encryptedBytes.Length);
        // get key and iv
        byte[] key, iv;
        DeriveKeyAndIV(passphrase, salt, out key, out iv);
        return DecryptStringFromBytesAes(encryptedBytes, key, iv);
    }

    private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
    {
        // generate key and iv
        List<byte> concatenatedHashes = new List<byte>(48);

        byte[] password = Encoding.UTF8.GetBytes(passphrase);
        byte[] currentHash = new byte[0];
        MD5 md5 = MD5.Create();
        bool enoughBytesForKey = false;
        // See http://www.openssl.org/docs/crypto/EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM
        while (!enoughBytesForKey)
        {
            int preHashLength = currentHash.Length + password.Length + salt.Length;
            byte[] preHash = new byte[preHashLength];

            Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
            Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
            Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);

            currentHash = md5.ComputeHash(preHash);
            concatenatedHashes.AddRange(currentHash);

            if (concatenatedHashes.Count >= 48)
                enoughBytesForKey = true;
        }

        key = new byte[32];
        iv = new byte[16];
        concatenatedHashes.CopyTo(0, key, 0, 32);
        concatenatedHashes.CopyTo(32, iv, 0, 16);

        md5.Clear();
        md5 = null;
    }

    static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key");
        if (iv == null || iv.Length <= 0)
            throw new ArgumentNullException("iv");

        // Declare the stream used to encrypt to an in memory
        // array of bytes.
        MemoryStream msEncrypt;

        // Declare the RijndaelManaged object
        // used to encrypt the data.
        RijndaelManaged aesAlg = null;

        try
        {
            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            msEncrypt = new MemoryStream();
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {

                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();
                    swEncrypt.Close();
                }
            }
        }
        finally
        {
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
                aesAlg.Clear();
        }

        // Return the encrypted bytes from the memory stream.
        return msEncrypt.ToArray();
    }

    static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (key == null || key.Length <= 0)
            throw new ArgumentNullException("key");
        if (iv == null || iv.Length <= 0)
            throw new ArgumentNullException("iv");

        // Declare the RijndaelManaged object
        // used to decrypt the data.
        RijndaelManaged aesAlg = null;

        // Declare the string used to hold
        // the decrypted text.
        string plaintext;

        try
        {
            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                        srDecrypt.Close();
                    }
                }
            }
        }
        finally
        {
            // Clear the RijndaelManaged object.
            if (aesAlg != null)
                aesAlg.Clear();
        }

        return plaintext;
    }


    IEnumerator SessionStart()
    {
        WWWForm Form = new WWWForm();
        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}";
        var KeyComponents = ReturnKey();
        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];
        var encriptionkey = KeyComponents[2];

        //print("Encryption key = " + encriptionkey);
        //print("To be encrypted = " + toBeEncrypted);
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);
        //print("Encrption data = " + encryptedData);
        //print("Data to be sent to server = " + $"{salt01}|{encryptedData}|{salt02}");
        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");
        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + sessionCheckEncrypted, Form);
        yield return www.SendWebRequest();


        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            string result = www.downloadHandler.text;
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
        }
        else
        {
            string result = www.downloadHandler.text;

            SessionCheck _SessionCheck = JsonConvert.DeserializeObject<SessionCheck>(result);
            string[] dataSplit = _SessionCheck.j.Split('|');
            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];
            string decriptionkey = Ms(salt02, salt01);
            AvoEx.AesEncryptor.keyString = decriptionkey;
            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);

            I_SessionCheck myDeserialized_i_sessioncheck = JsonConvert.DeserializeObject<I_SessionCheck>(decrypted);


            if (myDeserialized_i_sessioncheck.status == true)
            {
                TargetRTP = myDeserialized_i_sessioncheck.rtp;
                Balance = MathF.Round(myDeserialized_i_sessioncheck.balance, 2);
                PayoutString = myDeserialized_i_sessioncheck.payout_string;
                TotalbetPlaced = myDeserialized_i_sessioncheck.total_bet;
                TotalWinings = myDeserialized_i_sessioncheck.total_win;
                is_bonus = myDeserialized_i_sessioncheck.is_bonus;
                if (myDeserialized_i_sessioncheck.max_rtp == "1")
                    maxRtpExceed = true;
                else
                    maxRtpExceed = false;

                SceneManager.LoadScene("Play");
                LoadingPanel.SetActive(false);
            }
            else
            {
                ErrorClass errorResposnse = JsonConvert.DeserializeObject<ErrorClass>(decrypted);
                ErrorPanel.SetActive(true);
                ErroMessage.text = ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID;

                if (errorResposnse.error_message == "ActiveUserSessionException")
                {
                    ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    ErroMessage.text = ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID + "    Please reload the page";

                }
            }

        }
        www.Dispose();
        StopCoroutine("SessionStart");
    }



    public IEnumerator UpdateWalletCor(float WinAmount, float bet_amount, string gameplay, bool isbonus)
    {
        if (GetComponent<Canvas>().worldCamera == null)
            GetComponent<Canvas>().worldCamera = Camera.main;

        WinAmount = MathF.Round(WinAmount, 2);
        bet_amount = MathF.Round(bet_amount, 2);
        WWWForm Form = new WWWForm();

        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}|{WinAmount}|{bet_amount}|{gameplay}|";
        var KeyComponents = ReturnKey();
        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];
        var encriptionkey = KeyComponents[2];

        //print("Encryption key = " + encriptionkey);
        //print("To be encrypted = " + toBeEncrypted);
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);
        //print("Encrption data = " + encryptedData);
        //print("Data to be sent to server = " + $"{salt01}|{encryptedData}|{salt02}");
        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");

        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + updateWallet, Form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
        }
        else
        {
            string result = www.downloadHandler.text;
            // print(result);
            UpdateWalletBalance _UpdateWalletBalance = JsonConvert.DeserializeObject<UpdateWalletBalance>(result);

            string[] dataSplit = _UpdateWalletBalance.j.Split('|');

            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];
            string decriptionkey = Ms(salt02, salt01);
            AvoEx.AesEncryptor.keyString = decriptionkey;
            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);

            I_UpdateWalletBalance myDeserialized_i_UpdateWalletBalance = JsonConvert.DeserializeObject<I_UpdateWalletBalance>(decrypted);

            if (myDeserialized_i_UpdateWalletBalance.status == true)
            {
                EventID = myDeserialized_i_UpdateWalletBalance.eventId;
                Balance = MathF.Round(myDeserialized_i_UpdateWalletBalance.balance_after, 2);

                if(!isbonus)
                    ColumnManager.instance.StartSpin();
                SlotManager.instance.TranSactionID.text = EventID;
                is_bonus = myDeserialized_i_UpdateWalletBalance.is_bonus;
            }
            else
            {
                ErrorClass errorResposnse = JsonConvert.DeserializeObject<ErrorClass>(decrypted);
                ErrorPanel.SetActive(true);
                ErroMessage.text = ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID;

                if (errorResposnse.error_message == "ActiveUserSessionException")
                {
                    ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    ErroMessage.text = ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID + "    Please reload the page";

                }
            }

        }
        www.Dispose();
        StopCoroutine("UpdateWalletCor");
    }




    IEnumerator SetMaxRtpBool(string max_rtp = "1")
    {
        WWWForm Form = new WWWForm();
        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}|{max_rtp}";
        var KeyComponents = ReturnKey();
        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];
        var encriptionkey = KeyComponents[2];
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);
        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");

        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + MaxRTPUrl, Form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            string result = www.downloadHandler.text;
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
        }
        else
        {
            string result = www.downloadHandler.text;

            MaxRTPResponse _MaxRTPResponse = JsonConvert.DeserializeObject<MaxRTPResponse>(result);
            string[] dataSplit = _MaxRTPResponse.j.Split('|');
            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];
            string decriptionkey = Ms(salt02, salt01);
            AvoEx.AesEncryptor.keyString = decriptionkey;
            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);

            I_MaxRTPResponse myDeserialized_I_MaxRTPResponse = JsonConvert.DeserializeObject<I_MaxRTPResponse>(decrypted);


            if (myDeserialized_I_MaxRTPResponse.max_rtp == "1")
            {
                maxRtpExceed = true;
            }
            else
            {
                maxRtpExceed = true;
            }
        }
        www.Dispose();
        StopCoroutine("SetMaxRtpBool");
    }
    public IEnumerator Exit(int collect)
    {
        yield return new WaitForSeconds(.1f);
        WWWForm Form = new WWWForm();
        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}|{collect}";
        var KeyComponents = ReturnKey();
        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];
        var encriptionkey = KeyComponents[2];
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);
        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");
        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + exit, Form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
        }
        else
        {
            string result = www.downloadHandler.text;
            ExitResponse _ExitResponse = JsonConvert.DeserializeObject<ExitResponse>(result);
            string[] dataSplit = _ExitResponse.j.Split('|');
            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];
            string decriptionkey = Ms(salt02, salt01);
            AvoEx.AesEncryptor.keyString = decriptionkey;
            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);
            I_ExitResponse myDeserialized_i_ExitResponse = JsonConvert.DeserializeObject<I_ExitResponse>(decrypted);


            if (myDeserialized_i_ExitResponse.status != true)
            {
                ErrorClass errorResposnse = JsonConvert.DeserializeObject<ErrorClass>(decrypted);
                ErrorPanel.SetActive(true);
                ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID;
                ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                LoadingPanel.SetActive(false);

                if (collect == 0)
                {
                    Redirect_to_Main();
                }
            }
            else
            {
                Redirect_to_Main();
            }

        }

        www.Dispose();
        StopCoroutine("Exit");
    }
    public IEnumerator AddCreditCor(string amount)
    {
        yield return new WaitForSeconds(.1f);
        WWWForm Form = new WWWForm();
        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}|{amount}";
        var KeyComponents = ReturnKey();
        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];
        var encriptionkey = KeyComponents[2];
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);
        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");

        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + addCredit, Form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
            ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            string result = www.downloadHandler.text;

            AddBalanceClass _AddBalanceClass = JsonConvert.DeserializeObject<AddBalanceClass>(result);
            string[] dataSplit = _AddBalanceClass.j.Split('|');
            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];
            string decriptionkey = Ms(salt02, salt01);
            AvoEx.AesEncryptor.keyString = decriptionkey;
            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);
            I_AddBalanceClass myDeserialized_I_AddBalanceClass = JsonConvert.DeserializeObject<I_AddBalanceClass>(decrypted);



            if (myDeserialized_I_AddBalanceClass.status == "false")
            {
                ErrorClass errorResposnse = JsonConvert.DeserializeObject<ErrorClass>(decrypted);
                ErrorPanel.SetActive(true);
                ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID;
                ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            else
            {

                Balance = MathF.Round(myDeserialized_I_AddBalanceClass.game_balance, 2);
                Game.Instance.AddBalance();
                AddCredits.SetActive(false);

            }
            is_busy = false;
        }

        www.Dispose();
        StopCoroutine("AddCreditCor");
    }
    public IEnumerator GetUserBalance()
    {
        if (GetComponent<Canvas>().worldCamera == null)
            GetComponent<Canvas>().worldCamera = Camera.main;

        yield return new WaitForSeconds(.1f);
        WWWForm Form = new WWWForm();

        var toBeEncrypted = $"{luxorGameUid}|{luxorToken}|{luxorWalletUid}|{luxorUserUid}";

        var KeyComponents = ReturnKey();

        var salt01 = KeyComponents[0];
        var salt02 = KeyComponents[1];

        var encriptionkey = KeyComponents[2];
        var encryptedData = OpenSSLEncrypt(toBeEncrypted, encriptionkey);


        Form.AddField("data", $"{salt01}|{encryptedData}|{salt02}");


        UnityWebRequest www = UnityWebRequest.Post(BaseUrl + getUserBalance, Form);

        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
            ErrorPanel.SetActive(true);
            ErroMessage.text = www.error;
        }
        else
        {
            string result = www.downloadHandler.text;

            GetUserBalanceClass _GetUserBalanceClass = JsonConvert.DeserializeObject<GetUserBalanceClass>(result);


            string[] dataSplit = _GetUserBalanceClass.j.Split('|');

            string encryptedDataReceived = dataSplit[0];
            salt01 = dataSplit[1];
            salt02 = dataSplit[2];

            string decriptionkey = Ms(salt02, salt01);


            AvoEx.AesEncryptor.keyString = decriptionkey;

            var decrypted = OpenSSLDecrypt(encryptedDataReceived, decriptionkey);

            I_GetUserBalanceClass myDeserialized_i_GetUserBalanceClass = JsonConvert.DeserializeObject<I_GetUserBalanceClass>(decrypted);

            //  print("status = " + myDeserialized_i_GetUserBalanceClass.status);




            if (myDeserialized_i_GetUserBalanceClass.status == true)
            {

                MainWalletBalance = MathF.Round(myDeserialized_i_GetUserBalanceClass.main_wallet, 2);
                // print(MainWalletBalance);
                Balance = MathF.Round(myDeserialized_i_GetUserBalanceClass.balance, 2);
                is_bonus = myDeserialized_i_GetUserBalanceClass.is_bonus;


                if (is_bonus)
                {
                    ErrorPanel.SetActive(true);
                    ErroMessage.text = "You have not enough balance and you cannot add credit because you are in bonus mode";
                    ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    AddCredits.SetActive(true);
                    ButtonsInBonus.SetActive(false);
                    NormalButtons.SetActive(true);
                }

                AvailableCredits.text = "Available Credits: " + MainWalletBalance.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); ;
            }
            else
            {
                ErrorClass errorResposnse = JsonConvert.DeserializeObject<ErrorClass>(decrypted);
                ErrorPanel.SetActive(true);
                ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID;
                if (errorResposnse.error_message == "ActiveUserSessionException")
                {
                    ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    ErroMessage.text = errorResposnse.error + "\n" + errorResposnse.error_message + "\n" + "RefID = " + errorResposnse.refID + "    Please reload the page";

                }
            }

        }

        www.Dispose();
        StopCoroutine("GetUserBalance");
    }

    public class I_SessionCheck
    {
        public bool status { get; set; }
        public float balance { get; set; }
        public int rtp { get; set; }
        public bool is_bonus { get; set; }
        public float main_wallet { get; set; }
        public string payout_string { get; set; }
        public string max_rtp { get; set; }
        public float total_bet { get; set; }
        public float total_win { get; set; }
    }

    public class SessionCheck
    {
        public string j { get; set; }
        public I_SessionCheck i { get; set; }
    }

    public class UpdateWalletBalance
    {
        public string j { get; set; }
        public I_UpdateWalletBalance i { get; set; }
    }
    public class I_UpdateWalletBalance
    {

        public bool status { get; set; }
        public float balance_after { get; set; }
        public string eventId { get; set; }
        public bool is_bonus { get; set; }
    }

    public class MaxRTPResponse
    {
        public string j { get; set; }
        public I_MaxRTPResponse i { get; set; }
    }
    public class I_MaxRTPResponse
    {
        public bool status { get; set; }
        public string max_rtp { get; set; }
    }
    public class AddBalanceClass
    {
        public string j { get; set; }
        public I_AddBalanceClass i { get; set; }

    }
    public class I_AddBalanceClass
    {
        public string status { get; set; }
        public float main_balance { get; set; }
        public float game_balance { get; set; }
    }
    public class ErrorClass
    {
        public bool status { get; set; }
        public string error { get; set; }
        public string error_message { get; set; }
        public int refID { get; set; }
    }
    public class GetUserBalanceClass
    {
        public string j { get; set; }
        public I_GetUserBalanceClass i { get; set; }

    }
    public class I_GetUserBalanceClass
    {
        public bool status { get; set; }
        public float balance { get; set; }
        public bool is_bonus { get; set; }
        public float main_wallet { get; set; }
    }

    public class ExitResponse
    {
        public string j { get; set; }
        public I_ExitResponse i { get; set; }

    }
    public class I_ExitResponse
    {

        public bool status { get; set; }
    }
    public enum WinProbability
    {

        ZERO = 0,
        VERY_LOW = 2,
        LOW = 4,
        LOW_HIGH = 6,
        HIGH = 10,
        NATURAL_WIN = 11,
        VERY_HIGH = 30
    }

    public float maxPercentage = 0.10f;
    public WinProbability GetWinningProbability(float userGameRtp, float winProbability, float bet)
    {


        float chance = UnityEngine.Random.value;
        float maxRtp = winProbability + winProbability * maxPercentage;

        if (userGameRtp > maxRtp)
        {
            StartCoroutine(SetMaxRtpBool("1"));
            maxRtpExceed = true;
        }


        if (userGameRtp > maxRtp || maxRtpExceed)
        {
            if (chance <= 0.8f)
                return WinProbability.ZERO;
            else if (chance > 0.8f && chance <= 0.97f)
                return WinProbability.VERY_LOW;
            else
                return WinProbability.LOW_HIGH;
        }
        else if (userGameRtp > winProbability || bet > 1)
        {
            if (chance <= 0.4f)
                return WinProbability.ZERO;
            else if (chance > 0.4f && chance <= 0.8f)
                return WinProbability.VERY_LOW;
            else if (chance > 0.8f && chance <= 0.9f)
                return WinProbability.LOW;
            else if (chance > 0.9f && chance <= 0.95f)
                return WinProbability.LOW_HIGH;
            else
                return WinProbability.HIGH;
        }
        else
        {
            return WinProbability.NATURAL_WIN;
        }

    }

    public WinProbability GetFreeSpinWinningProbability(float betAmount, float totalFreeSpinWinAmount, float gameRtp)
    {
        float chance = UnityEngine.Random.value;


        if (gameRtp == 70)
        {
            if (chance <= 0.1f)
                return WinProbability.ZERO;
            else if (chance <= 0.8f)
                return WinProbability.VERY_LOW;
            else
                return WinProbability.LOW;

        }


        if (totalFreeSpinWinAmount < (10 * betAmount))
        {
            // free spin win amount is less than 10 times
            if (chance <= 0.1f)
                return WinProbability.ZERO;
            else if (chance <= 0.8f)
                return WinProbability.VERY_LOW;
            else if (chance <= 0.9f)
                return WinProbability.LOW;
            else if (chance <= 0.95f)
                return WinProbability.LOW_HIGH;
            else
                return WinProbability.HIGH;


        }
        else
        {
            // free spin win amount is more than 10 times
            if (chance <= 0.5f)
                return WinProbability.ZERO;
            else
                return WinProbability.VERY_LOW;
        }
    }

    public void Home(float _Balance)
    {
        Balance = MathF.Round(_Balance, 2);
        ClosePanel.SetActive(true);
    }
    public void CloseYesNo(int val)
    {

        ClosePanel.SetActive(false);
        StartCoroutine(Exit(val));
        LoadingPanel.SetActive(true);

    }
    public void Redirect_to_Main()
    {
        closewindow("https://" + RedirectUrl);

    }


    public void CreditAmountClick(int amount)
    {
        if (CreditInputField.text.Length == 0)
            CreditInputField.text = "0";
        string oldvalue = string.Format(CreditInputField.text, 2);
        float temp = float.Parse(oldvalue);
        CreditInputField.text = (temp + amount).ToString("F2", System.Globalization.CultureInfo.InvariantCulture); ;


    }
    public void AddAllClicked()
    {
        CreditInputField.text = MainWalletBalance.ToString("F2", System.Globalization.CultureInfo.InvariantCulture); ;
    }
    public void ResetButtonClicked()
    {
        CreditInputField.text = "";
    }

    public void AddCreditButtonClicked()
    {

        if (is_busy)
            return;
        else
        {

            if (float.Parse(CreditInputField.text) > MainWalletBalance)
            {
                ErrorPanel.SetActive(true);
                ErroMessage.text = "The amount is greater than available credits";
                ErrorPanel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                return;
            }
            else
            {
                if (is_bonus && float.Parse(CreditInputField.text) > 100)
                {
                    ErrorPanel.SetActive(true);
                    ErroMessage.text = "You can not add more than 100";
                    return;
                }



                StartCoroutine(AddCreditCor(CreditInputField.text.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                is_busy = true;
            }


        }

    }

    bool is_busy = false;

    private string Ms(string a, string b)
    {
        string combined = b + a;
        char[] split = combined.ToCharArray();
        char[] arr0 = new char[split.Length / 4];
        char[] arr1 = new char[split.Length / 4];
        char[] arr2 = new char[split.Length / 4];
        char[] arr3 = new char[split.Length / 4];

        for (int i = 0; i < split.Length; i++)
        {
            if (i % 4 == 1) arr1[i / 4] = split[i];
            else if (i % 4 == 2) arr2[i / 4] = split[i];
            else if (i % 4 == 3) arr3[i / 4] = split[i];
            else if (i % 4 == 0) arr0[i / 4] = split[i];
        }

        Array.Reverse(arr3);
        Array.Reverse(arr2);
        Array.Reverse(arr1);
        Array.Reverse(arr0);

        return new string(arr2) + new string(arr0) + new string(arr3) + new string(arr1);
    }

    private string[] ReturnKey()
    {
        string salt01 = Guid.NewGuid().ToString();
        string salt02 = Guid.NewGuid().ToString();
        // print("salt01 xxxxxxxxxxxxx = " + salt01);
        // print("salt02 xxxxxxxxxxxxx = " + salt02);
        string key = Ms(salt02, salt01);
        return new[] { salt01, salt02, key };
    }


}
