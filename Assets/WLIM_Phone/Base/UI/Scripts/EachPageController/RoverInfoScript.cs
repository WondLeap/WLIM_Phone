using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static CrossDeviceState;

public class RoverInfoScript : MonoBehaviourPunCallbacks, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    // ��ư�� ���ε� ���� ������ ���
    private string rover_antennaInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_antennaInfo";
    private string rover_headlampInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_headlampInfo";
    private string rover_solarpanelnfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_solarpanelInfo";
    private Button _rover_antennaInfoUButton;
    private Button _rover_headlampInfoUButton;
    private Button _rover_solarpanelnfoUButton;
    private Button _rover_antennaGenUButton;
    private Button _rover_headlampGenUButton;
    private Button _rover_solarpaneGenUButton;

    private PhotonView PV;

    // TebbedMenu���� ���� �������� ���Ƴ��� �� ��ũ��Ʈ�� �ν��Ͻ��� ���� �� ���� ���ֹǷ� ���⼭�� ���� ��ư ��ɸ� �������ָ� ��
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        _rover_antennaInfoUButton = MyRoot.Q<Button>("AntennaInfoButton");
        _rover_antennaInfoUButton.RegisterCallback<ClickEvent>(OnAntennaInfoButtonClicked);
        _rover_headlampInfoUButton = MyRoot.Q<Button>("HeadlampInfoButton");
        _rover_headlampInfoUButton.RegisterCallback<ClickEvent>(OnHeadlampInfoButtonButtonClicked);
        _rover_solarpanelnfoUButton = MyRoot.Q<Button>("SolarpanelInfoButton");
        _rover_solarpanelnfoUButton.RegisterCallback<ClickEvent>(OnSolarpanelInfoButtonClicked);

        _rover_antennaGenUButton = MyRoot.Q<Button>("AntennaGenButton");
        _rover_antennaGenUButton.RegisterCallback<ClickEvent>(OnAntennaGenButtonClicked);
        _rover_headlampGenUButton = MyRoot.Q<Button>("HeadlampGenButton");
        _rover_headlampGenUButton.RegisterCallback<ClickEvent>(OnHeadlampGenButtonClicked);
        _rover_solarpaneGenUButton = MyRoot.Q<Button>("SolarpanelGenButton");
        _rover_solarpaneGenUButton.RegisterCallback<ClickEvent>(OnSolarpanelGenButtonClicked);

        GameObject PVobject = GameObject.FindGameObjectWithTag("PUN2manager");
        if (PVobject != null && (PV = PVobject.GetComponent<PhotonView>()) != null)
        {
        }
        else
        {
            Debug.LogError("GameObject with PUN2manager tag or PhotonView not found!");
        }
    }

    private void OnAntennaInfoButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnAntennaInfoButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_antennaInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_antennaInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }

    private void OnHeadlampInfoButtonButtonClicked(ClickEvent evt)
    {
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_headlampInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_headlampInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }

    private void OnSolarpanelInfoButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnSolarpanelInfoButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_solarpanelnfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(rover_solarpanelnfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }

    private void OnAntennaGenButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnAntennaGenButtonClicked");

        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Antenna);
                }
            } 
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Antenna);
                }
            }
        }
    }

    private void OnHeadlampGenButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnHeadlampGenButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Headlamp);
                }
            }
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Headlamp);
                }
            }
        }
    }

    private void OnSolarpanelGenButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnSolarpanelGenButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Solarpanel);
                }
            }
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            foreach (DeviceInfo deviceInfo in Instance.ConnectedDevicesInfo)
            {
                Player player = Instance.GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    PV.RPC("RPC_GenBtnClick", player, Obj_Pho.Solarpanel);
                }
            }
        }
    }
}
