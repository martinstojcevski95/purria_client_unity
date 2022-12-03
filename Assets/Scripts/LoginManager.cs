using Purria;
using Purria.PurriaUrls;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

    [SerializeField] TMP_InputField loginEmail, loginPassword, loginUser, signupEmail, signupPassword, signupUser, username;

    [SerializeField] Button LogInBtn, SignUpBtn;

    public static Action LoginFinished;

    private void OnEnable()
    {
        SignUpBtn.onClick.AddListener(SignUp);
        LogInBtn.onClick.AddListener(LogIn);
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("email") && PlayerPrefs.HasKey("password"))
        {
            loginEmail.text = PlayerPrefs.GetString("email");
            loginPassword.text = PlayerPrefs.GetString("password");
 
        }
    }

    private void LogIn()
    {

        var login = new LogIn(loginEmail.text, loginUser.text, loginPassword.text);
        PlayerPrefs.SetString("email", loginEmail.text);
        PlayerPrefs.SetString("password", loginPassword.text);


        string toJson = JsonUtility.ToJson(login);
        StartCoroutine(PostRequest(ApiClasses.login_url, toJson, OnLoginFinished));
    }

    private void OnLoginFinished(string response, long responseCode)
    {
        if (responseCode != 200)
        {
            var responseMessage = JsonUtility.FromJson<ResponseMessage>(response);
            UIController.Instance.NotificationOpen(responseMessage.message);
            print(response);
        }

        else if (responseCode == 200)
        {
            UserManager.Instance.login_response = JsonUtility.FromJson<LogInResponse>(response);
            UIController.Instance.OpenDashboardUI();
            LoginFinished?.Invoke();
        }

    }


    IEnumerator PostRequest(string url, string json, Action<string, long> requestResponse)
    {

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            requestResponse.Invoke("", uwr.responseCode);
            //Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {

            requestResponse.Invoke(uwr.downloadHandler.text, uwr.responseCode);
            Debug.Log("Received: " + uwr.downloadHandler.text);

        }
    }

    private void SignUp()
    {
        var signup = new SignUp(signupEmail.text, signupUser.text, signupPassword.text);

        string toJson = JsonUtility.ToJson(signup);

        StartCoroutine(PostRequest(ApiClasses.signup_url, toJson, OnSignUpFinished));
    }

    private void OnSignUpFinished(string response, long responseCode)
    {
        if (responseCode != 201)
        {
            UIController.Instance.NotificationOpen(response);
            print(response);
        }

        else if (responseCode == 201)
        {
            var signupResponse = JsonUtility.FromJson<SignUpResponse>(response);

            loginEmail.text = signupResponse.data.email;
            loginUser.text = signupResponse.data.username;

            UIController.Instance.CloseRegister();
            UIController.Instance.OpenLogin();
        }



    }
}
