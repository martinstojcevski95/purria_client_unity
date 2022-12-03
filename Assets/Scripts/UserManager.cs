using Purria;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance { get; private set; }

    public LogInResponse login_response;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
    }
}
