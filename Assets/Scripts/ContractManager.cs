using Michsky.UI.ModernUIPack;
using Purria;
using Purria.PurriaUrls;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractManager : MonoBehaviour
{

    public ContractRoot contract_root;
    [SerializeField] TMP_Text contractname, contractdescription;
    [SerializeField] CustomDropdown contractlevel;

    private void OnEnable()
    {

        LoginManager.LoginFinished += OnLoginFinished;
    }


    private void OnLoginFinished()
    {
        print("logged in, retrieving contract data...");

        RequestsManager.Instance.GET(ApiClasses.contracts_get_url, OnContractResponse);
    }

    private void OnContractResponse(string response, long responseCode)
    {
        if (responseCode != 200)
            return;

        contract_root = JsonUtility.FromJson<ContractRoot>(response);

        for (int i = 0; i < contract_root.result.Count; i++)
        {

            UIController.Instance.SetContractToListView(contract_root.result[i]);
        }
    }


    public void CreateContract()
    {
        var contractData = new CreateContract(contractname.text, contractdescription.text);
        string jsonData = JsonUtility.ToJson(contractData);

        RequestsManager.Instance.POST(ApiClasses.contracts_get_url, jsonData, OnCreatedContractResponse);
    }

    private void OnCreatedContractResponse(string response, long responseCode)
    {
        if (response.Contains("errors"))
        {
            var responseMessage = JsonUtility.FromJson<ErrorMessage>(response);
            UIController.Instance.NotificationOpen(responseMessage.errors[0]);
            return;
        }
        if (response.StartsWith("name"))
        {
            print("1");
        }
        print(responseCode);
 

        UIController.Instance.SetContractToListView(JsonUtility.FromJson<ContractResponse>(response));

    }
}
