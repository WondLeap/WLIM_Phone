using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScript : MonoBehaviourPunCallbacks, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    // ��ư�� ���ε� ���� ������ ���
    private string listOfExhibitsUxmlPath = "UXML/MainItems/ListOfExhibits";
    private Button _listOfExhibitsButton;
    private Button _targetConnectButton;
    private Button _testButton;
    private Label _myActorNumLabel;
    private TextField _targetActorNumTextField;
    private Label _imuDataLabel;

    // TebbedMenu���� ���� �������� ���Ƴ��� �� ��ũ��Ʈ�� �ν��Ͻ��� ���� �� ���� ���ֹǷ� ���⼭�� ���� ��ư ��ɸ� �������ָ� ��
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        _listOfExhibitsButton = MyRoot.Q<Button>("ListOfExhibitsButton");
        _listOfExhibitsButton.RegisterCallback<ClickEvent>(OnListOfExhibitsButtonClicked);
        _targetConnectButton = MyRoot.Q<Button>("TargetConnectButton");
        _targetConnectButton.RegisterCallback<ClickEvent>(OnTargetConnectButtonButtonClicked);
        _testButton = MyRoot.Q<Button>("TestButton");
        _testButton.RegisterCallback<ClickEvent>(OnTestButtonButtonClicked);
        _myActorNumLabel = MyRoot.Q<Label>("MyActorNumLabel");
        _targetActorNumTextField = MyRoot.Q<TextField>("TargetActorNumTextField");
        _imuDataLabel = MyRoot.Q<Label>("IMULabel");
    }

    void Update()
    {
        // ������ �ȵǵ� ��½õ��ϴ� ����������
        /*
        _imuDataLabel.text = "imuLabel";
        Debug.Log($"acc: {RingIMUreceiver.Instance.ringIMU.acc}, gyr: {RingIMUreceiver.Instance.ringIMU.gyr}");
        _imuDataLabel.text = $"acc: {RingIMUreceiver.Instance.ringIMU.acc}, gyr: {RingIMUreceiver.Instance.ringIMU.gyr}";
        */
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("����!!!  " + PhotonNetwork.LocalPlayer.ActorNumber);
        _myActorNumLabel.text = $"me : {PhotonNetwork.LocalPlayer.ActorNumber}";
    }

    private void OnListOfExhibitsButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnListOfExhibitsButtonButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(listOfExhibitsUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(listOfExhibitsUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }

    private void OnTargetConnectButtonButtonClicked(ClickEvent evt)
    {
        Debug.Log($"OnTargetConnectButtonButtonClicked {_targetActorNumTextField.value}");
        if (!(_targetActorNumTextField.value == ""))
        {
            CrossDeviceState.Instance.CrossDeviceConnect(int.Parse(_targetActorNumTextField.value));
        } 
    }

    private void OnTestButtonButtonClicked(ClickEvent evt)
    {
        Debug.Log($"OnTestButtonButtonClicked");
        CrossDeviceState.Instance.TestConnect();
    }
}
