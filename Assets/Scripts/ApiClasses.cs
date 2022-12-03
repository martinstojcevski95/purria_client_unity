using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Purria
{

    #region USER
    [Serializable]
    public class SignUp
    {
        public string email;
        public string username;
        public string password;

        public SignUp(string email, string username, string password)
        {
            this.email = email;
            this.username = username;
            this.password = password;
        }
    }

    [Serializable]
    public class LogIn
    {
        public string email;
        public string username;
        public string password;

        public LogIn(string email, string username, string password)
        {
            this.email = email;
            this.username = username;
            this.password = password;
        }
    }



    [Serializable]
    public class SignUpResponse
    {
        public string message;
        public SignUp data;
    }

    [Serializable]
    public class LogInResponse
    {
        public string message;
        public Tokens tokens;
    }

    [Serializable]
    public class Tokens
    {
        public string access;
        public string refresh;
    }

    #endregion

    #region GARDEN
    [Serializable]
    public class Garden
    {
        public string garden_contract_id;
        public string garden;
        public int id;
    }

    #endregion

    #region CONTRACT

    [Serializable]
    public class ContractResponse
    {
        public string id;
        public string name;
        public string description;
        public int level;
        public List<Garden> garden;
        public string user;
    }

    [Serializable]
    public class CreateContract
    {
        public string name;
        public string description;
        public int? level;

        public CreateContract(string name, string description, int? level = null)
        {
            this.name = name;
            this.description = description;
            this.level = level;
        }
    }

    [Serializable]
    public class ContractRoot
    {
        public List<ContractResponse> result;
    }

    #endregion

    #region ERROR_MESSAGE
    public class ResponseMessage
    {
        public string message;
    }

    public class ErrorMessage
    {
        public List<string> errors;
    }


    #endregion
    #region URLS
    namespace PurriaUrls
    {
        public class ApiClasses
        {

            private const string api_v1_auth = "http://mymac:8010/v1/auth/";
            private const string api_v1 = "http://mymac:8010/v1/purria/";


            public const string signup_url = api_v1_auth + "signup/";
            public const string login_url = api_v1_auth + "login/";

            public const string contracts_get_url = api_v1 + "contracts/";

            public const string gardens_for_contract_url = api_v1 + "gardens_for_contract/{0}";
        }
    }

    #endregion
}

