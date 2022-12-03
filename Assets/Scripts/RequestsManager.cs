using Purria;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestsManager : MonoBehaviour
{
    public static RequestsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
 
    }

    public void POST(string url,string json, Action<string,long> requestResponse)
    {
        StartCoroutine(PostRequest(url,json,requestResponse));
    }

    public void GET(string url, Action<string,long> requestResponse)
    {
        StartCoroutine(GetRequest(url,requestResponse));
    }

    IEnumerator GetRequest(string url, Action<string,long> requestResponse)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        uwr.SetRequestHeader("Authorization", "Bearer " + UserManager.Instance.login_response.tokens.access);

        yield return uwr.SendWebRequest();
        //set BEARER 

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            requestResponse?.Invoke(uwr.downloadHandler.text, uwr.responseCode);
           // Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            requestResponse.Invoke(uwr.downloadHandler.text, uwr.responseCode);
            //Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    IEnumerator PostRequest(string url, string json, Action<string,long> requestResponse)
    {

        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        print("token " + UserManager.Instance.login_response.tokens.access);
        uwr.SetRequestHeader("Authorization", "Bearer " + UserManager.Instance.login_response.tokens.access);

        //set BEARER 

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            requestResponse.Invoke("",uwr.responseCode);
            //Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {

           requestResponse.Invoke(uwr.downloadHandler.text, uwr.responseCode);
            Debug.Log("Received: " + uwr.downloadHandler.text);

        }
    }
}
