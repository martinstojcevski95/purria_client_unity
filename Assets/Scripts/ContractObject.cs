using Purria;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractObject : MonoBehaviour
{
    public ContractResponse contract_object_response;
    [SerializeField] TMP_Text contractname, contractdescription, contractlevel;

    public void AssignData(ContractResponse data)
    {
        contract_object_response = data;
        contractname.text = contract_object_response.name;
        contractdescription.text = contract_object_response.description;
        contractlevel.text = contract_object_response.level.ToString();

    }
}
