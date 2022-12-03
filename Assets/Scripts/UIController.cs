using DG.Tweening;
using Michsky.UI.ModernUIPack;
using Purria;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //  [SerializeField] ContractController contractController;

    [SerializeField]
    ContractObject contractObjPref;

    public static UIController Instance;

    [Header("SIGNUP AND LOGIN UI SCREENS")]
    public Canvas FullDashboardUI;
    [SerializeField] List<Button> AppMainHeaderButtons = new List<Button>();
    

    public RectTransform FullDashboard;
    public RectTransform ContractsUI;
    public RectTransform ContractsAuctionUI;
    public RectTransform DronesUI;
    public RectTransform FieldUI;
    public RectTransform WeatherUI;
    public RectTransform NotificationsUI;
    public Canvas LogInAndRegisterScreen;
    public RectTransform LoginUI, RegisterUI, LogAndRegInitialButtonsScreen;
    public GameObject FieldOnly;
    public RectTransform DashBoardUI;

    [Header("MainHeaderButtons")]
    [SerializeField] Button DroneBtn, ContractBtn, MessagesBtn, FieldBtn, WeatherBtn,NotificationsBtn;

    [Header("Seed/Plant 3D View")]
    [SerializeField] GameObject Plant3DView;    

    [Header("CreatingContractUI")]
    public RectTransform ContractLinkedFieldUI;
    public Button CreateContract;
    public InputField ContractFieldIDInput;


    [Header("ContractsFieldsUI")]
    public RectTransform ContractsFieldUI;
    public Dropdown ContractsFieldsDropdown;


    [SerializeField]
    Text currentTime;

    [Header("LOG INFO")]
    public Text LogText,DeleteDialogText,droneUIText;
    [SerializeField]
    public Canvas LogPanel;
    public Canvas DeleteContractDialogConfirmationPanel;
    public Canvas DroneUIInfo;
    public Button DeleteDialogYesButton;   
    public Canvas InfoDialogCanvas;


    public Canvas LoadingUI;
    public TMP_Text loadingMessage, inspectWindowMessage, inspectWindownTitle;


    [SerializeField]
    private GameObject ContractsListViewParent,FieldsListViewParent,PlantsListViewParent,DronesListViewParent;

    [SerializeField]
    NotificationManager notificationManager;

    [SerializeField]
    GameObject InspectWindow;
    private void Awake()
    {
        Instance = this;
    }

  

    #region RegisterAndLogin

    public void OpenRegister()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(-800, 0), 0.4f);
        RegisterUI.DOAnchorPos(new Vector2(0, 7.8f), 0.4f);
    }

    public void OpenLogin()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(800, 0), 0.4f);
        LoginUI.DOAnchorPos(new Vector2(0, 7.8f), 0.4f);
    }

    public void CloseRegister()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(0, 0), 0.4f);
        RegisterUI.DOAnchorPos(new Vector2(-800, 7.8f), 0.4f);
    }

    public void CloseLogin()
    {
        LogAndRegInitialButtonsScreen.DOAnchorPos(new Vector2(0, 0), 0.4f);
        LoginUI.DOAnchorPos(new Vector2(800, 7.8f), 0.4f);
    }

    #endregion

    #region FullDashboard

    private void Update()
    {
       // currentTime.text = DateTime.Now.ToString();
    }

    public void LoginIn()
    {
        LogPanel.enabled = true;
        LogText.text = "LOADING ...";

    }

    public void OpenAuctionUI()
    {
        ContractsAuctionUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
    }

    public void CloseAuctionUI()
    {
        ContractsAuctionUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
      //  contractController.countDownCounter = 5;
       // contractController.StartCountDown(false);
      //  contractController.ResetBidding();
    }


    public void NotificationOpen(string notificationMessage)
    {
        notificationManager = FindObjectOfType<NotificationManager>();
        notificationManager.CloseNotification();
        notificationManager.description = notificationMessage;
        notificationManager.UpdateUI();
        notificationManager.notificationStyle = NotificationManager.NotificationStyle.SLIDING;
        notificationManager.OpenNotification();
    }
    public void InspectWindowOpen(string title, string message)
    {
        InspectWindow.gameObject.SetActive(true);
        inspectWindowMessage.text = message ;
        inspectWindownTitle.text = "<b> " + title + "</b>";
    }

    public void OpenDashboardUI()
    {
        LogInAndRegisterScreen.enabled = false;
        FullDashboardUI.enabled = true;
        notificationManager.transform.SetParent(FullDashboardUI.transform);
    }
    public void CloseDashboardUI()
    {
        FullDashboardUI.enabled = false;
    }

    public void Activate3DPlantView(bool isOn)
    {
        Plant3DView.gameObject.SetActive(isOn);
        FullDashboardUI.enabled = !isOn;
    }

    public void OpenDashBoard()
    {

        LogInAndRegisterScreen.enabled = false;
        LogText.text = "";
        LogPanel.enabled = false;
        FullDashboardUI.enabled = true;
        FullDashboard.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -1253.06f), 0.5f);

    }
    #endregion

    #region LoadingUI
    public void OnLoadingUI(bool isOpen, string message = "<b>Loading</b>")
    {
       // FullDashboardUI.enabled = !isOpen;
        LoadingUI.enabled = isOpen;
        loadingMessage.text = message;
    }
    #endregion

    #region ContractUI

    public void OpenContractsUI()
    {

        ContractsUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        ContractBtn.interactable = false;
        CloseDronetUI();
        CloseFieldUI();
        CloseWeatherUI();
        CloseNotificationsUI();
    }

    public void CloseContractUI()
    {
        ContractsUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
        ContractBtn.interactable = true;
    }

    public void CloseCreateContractUI()
    {

        CreateContract.onClick.RemoveAllListeners();
        ContractLinkedFieldUI.DOAnchorPos(new Vector2(-1400, 0), 0.4f);
    }

    #endregion


    // SPAWN ALL 30 FIELDS AND ALL 30 PLANTS AND JUST CHANGE THE DATA ION THE SAME OBJECTS WHEN CONTRACT IS SELECTED AND FIELD ETC
    // the contracts will be dynamic spawned on the go
    //the drones will be predefined in the app the names as enums so we can choose from dropdown or something and set drone

    public void SetContractToListView(ContractResponse response)//, Fields_Manager fieldsManager)
    {
        var spawnedcontract = Instantiate(contractObjPref, ContractsListViewParent.transform.position, Quaternion.identity);
        spawnedcontract.transform.SetParent(ContractsListViewParent.transform);
        spawnedcontract.transform.localScale = new Vector3(1, 1, 1);
        spawnedcontract.GetComponent<ContractObject>().AssignData(response);
    }


    public void SetDroneToListView(GameObject drone, string jsonData)//, Fields_Manager fieldsManager)
    {
        var spawneddrone = Instantiate(drone, DronesListViewParent.transform.position, Quaternion.identity);
        spawneddrone.transform.SetParent(DronesListViewParent.transform);
        spawneddrone.transform.localScale = new Vector3(1, 1, 1);
        spawneddrone.transform.gameObject.SetActive(true);
      //  spawneddrone.GetComponent<SingleDrone>().SetDroneData(jsonData);
    }


    public void SpawnInitialPlants(GameObject plant, Action<GameObject> _OnSpawnedPlant)
    {
        var spawnedplant = Instantiate(plant, PlantsListViewParent.transform.position, Quaternion.identity);
        spawnedplant.transform.SetParent(PlantsListViewParent.transform);
        spawnedplant.transform.localScale = new Vector3(1, 1, 1);
        spawnedplant.gameObject.SetActive(false);
        _OnSpawnedPlant?.Invoke(spawnedplant);

    }

    public void SpawnInitialFields(GameObject field, Action<GameObject> _OnSpawnedField)
    {
        var spawnedfield = Instantiate(field, FieldsListViewParent.transform.position, Quaternion.identity);
        spawnedfield.transform.SetParent(FieldsListViewParent.transform);
        spawnedfield.transform.localScale = new Vector3(1, 1, 1);
        spawnedfield.gameObject.SetActive(false);
        _OnSpawnedField?.Invoke(spawnedfield);

    }

    public void SetFieldsData(GameObject field, string jsonData)
    {
        //print(jsonData);
        field.gameObject.SetActive(true);
      //  field.GetComponent<SingleField>().SetFieldData(jsonData);
    }
    
    public void SetSeedsData(GameObject plant, string jsonData)
    {
        plant.gameObject.SetActive(true);
      //  plant.GetComponent<SingleSeed>().SetSeedData(jsonData);
        
    }


    #region NotificationUI

    public void OpenNotificationsUI()
    {
        NotificationsUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        NotificationsBtn.interactable = false;
        CloseContractsFieldsUI();
        CloseContractUI();
        CloseWeatherUI();
        CloseDronetUI();
    }

    public void CloseNotificationsUI()
    {
        NotificationsUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
        NotificationsBtn.interactable = true;
    }
    #endregion


    #region DroneUI

    public void OpenDroneUI()
    {
        DronesUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        DroneBtn.interactable = false;
        CloseContractUI();
        CloseFieldUI();
        CloseWeatherUI();
        CloseNotificationsUI();
    }

    public void CloseDronetUI()
    {
        DronesUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
        DroneBtn.interactable = true;
    }

    #region DroneTeamMenus
    public void OpenDroneTeam(RectTransform droneTeam)
    {
        droneTeam.DOAnchorPos(new Vector2(0, 0), 0.4f);
    }

    public void CloseDroneTeam(RectTransform droneTeam)
    {
        droneTeam.DOAnchorPos(new Vector2(1318, 0), 0.4f);
    }

    #endregion

    #endregion


    #region FieldUI

    public void OpenFieldUI()
    {
        FieldUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        FieldBtn.interactable = false;
        FieldsDropdownData();
        CloseDronetUI();
        CloseContractUI();
        CloseWeatherUI();
        CloseFieldOnly();
        CloseNotificationsUI();
    }

    public void CloseFieldUI()
    {
        FieldUI.DOAnchorPos(new Vector2(0, -2537), 0.4f);
       // CloseFieldOnly();
        FieldBtn.interactable = true;
       // OpenFieldUI();
    }

    public void CloseFieldOnly()
    {
        FieldOnly.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-1251, -107), 0.4f);
        // 1272
    }

    public void OpenContractsFieldsUI()
    {
        ContractsFieldUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
    }

    public void CloseContractsFieldsUI()
    {
        ContractsFieldUI.DOAnchorPos(new Vector2(-1400, 0), 0.4f);
    }



    public void FieldsDropdownData()
    {
        ContractsFieldsDropdown.options.Clear();
       // contractController.ContractIDS.Sort();
       // ContractsFieldsDropdown.options.Add(new Dropdown.OptionData() { text = "" });
       // foreach (var item in contractController.ContractIDS)
       // {

        //    ContractsFieldsDropdown.options.Add(new Dropdown.OptionData() { text = "Contract " + item.ToString() });
       // }
    }

    #endregion

    #region WeatherUI

    public void OpenWeatherUI()
    {
        WeatherUI.DOAnchorPos(new Vector2(0, 0), 0.4f);
        WeatherBtn.interactable = false;
        CloseDronetUI();
        CloseFieldUI();
        CloseContractUI();
        CloseNotificationsUI();
    }
    public void CloseWeatherUI()
    {
        WeatherUI.DOAnchorPos(new Vector2(0, -1400), 0.4f);
        WeatherBtn.interactable = true;
    }


    #endregion




    public void DashBoard()
    {
        CloseDronetUI();
        CloseContractUI();
        CloseWeatherUI();
        CloseFieldUI();
        ManiButtonsActiveStatus(true);
    }

    void ManiButtonsActiveStatus(bool isActive)
    {
        AppMainHeaderButtons.ForEach(b => b.interactable = isActive);
    }


    public void CloseDeleteDialog()
    {
        DeleteContractDialogConfirmationPanel.enabled = false;
        DeleteDialogText.text = "";
    }

    public void OpenDroneUI(bool isPanelActive,string description)
    {
        droneUIText.text = description;
        if (isPanelActive)
        {
            DroneUIInfo.enabled = isPanelActive;
        }
        else
        {
            DroneUIInfo.enabled = isPanelActive;
        }
    }

    public void CloseDroneUI()
    {
        DroneUIInfo.enabled = false;
        droneUIText.text = "";
    }

    public void DeleteDialog(bool isPanelActive,string description)
    {
        DeleteDialogText.text = description;
        if (isPanelActive)
        {
            DeleteContractDialogConfirmationPanel.enabled = isPanelActive;
        }
        else
        {
            DeleteContractDialogConfirmationPanel.enabled = isPanelActive;
        }


    }
}
