using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using static CrossDeviceState;

// Combining multiple device states and inputs to identify complex states and input intentions.
// This class mainly refers GlassesState, PhoneState classes


public class CrossDeviceState : MonoBehaviourPunCallbacks
{
    public enum DeviceType
    {
        Glasses,
        Phone,
        Watch,
        Ring,
        Hybrid
    }

    public enum InputMethod
    {
        PhoneSwipe,
        PhoneGyro,
        GlassesGyro,
        Null
    }

    [Serializable]
    public class DeviceInfo //���� ������� ������ ���� Ŭ����
    {
        public int ActorNumber { get; set; }
        public DeviceType DeviceType { get; set; }

        public DeviceInfo(int actorNumber, DeviceType deviceType)
        {
            ActorNumber = actorNumber;
            DeviceType = deviceType;
        }
    }
    private DeviceType deviceType;
    public DeviceType GlaWebviewType { get; set; }  // �۷���-�� ������ �۷���, �� ���� ���� ����
    public static CrossDeviceState Instance { get; private set; }
    public DeviceInfo MyDeviceInfo { get; private set; }
    public List<DeviceInfo> ConnectedDevicesInfo { get; private set; }
    private int preparedConnectedDevices;   // ��Ⱑ ����, ��ü������ ���� ��� �غ� ��ġ�� cnt++;, ��� ��Ⱑ �غ�Ǹ� ����ȭ �ڵ� ����
    public InputMethod ControlMode { get; set; }
    public bool IsObjBeingManip { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            deviceType = DeviceType.Phone;  // �� �ڵ带 ����ϴ� ��⿡ �°� �ٲٽÿ�
            GlaWebviewType = DeviceType.Phone;  // glasses �ܵ� ��� UI�� ���߿� ������Ʈ����, ����� phone �����ĸ�
            ControlMode = InputMethod.PhoneSwipe;
            IsObjBeingManip = false;
            MyDeviceInfo = null;
            ConnectedDevicesInfo = null;

            preparedConnectedDevices = 0;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        MyDeviceInfo = new DeviceInfo(PhotonNetwork.LocalPlayer.ActorNumber, deviceType);
        ConnectedDevicesInfo = new List<DeviceInfo> // �ڽ��� ������ ����� ��� ���
            {
                MyDeviceInfo
            };

        // ���� �������� �ҽ�, ���� ���� ��ȣ�ۿ� �ϴ� �ڵ�鿡 ������ ���� �ڽ��� �����ϴ� ��ü�� ���� ��������
        /*
        switch (MyDeviceInfo.DeviceType)
        {
            case DeviceType.Glasses:
                gameObject.AddComponent<GlassesState>();
                break;
            case DeviceType.Phone:
                gameObject.AddComponent<PhoneState>();
                break;
            case DeviceType.Watch:
                break;
            case DeviceType.Ring:
                break;
            default:
                break;
        }
        */
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
    }

    // connection ��û�� ������ �Ʒ��� �Լ��� ����
    public void CrossDeviceConnect(int targetActorNumber)
    {
        Debug.Log("CrossDeviceConnect");

        byte[] serializedConnectedDevicesInfo = Serialize<List<DeviceInfo>>(ConnectedDevicesInfo);
        byte[] serializedMyDeviceInfo = Serialize<DeviceInfo>(MyDeviceInfo);
        object[] eventData = new object[] { serializedConnectedDevicesInfo, serializedMyDeviceInfo };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { targetActorNumber } };
        byte eventCode = 101;

        // �̺�Ʈ ����
        PhotonNetwork.RaiseEvent(eventCode, eventData, raiseEventOptions, SendOptions.SendReliable);
    }

    public void TestConnect()
    {
        Debug.Log("TestSendIntList");
        List<int> intList = new List<int> { 1, 2, 3, 4, 5 }; // �����ϰ� ���� int�� ����Ʈ
        int[] intArray = intList.ToArray(); // int�� ����Ʈ�� �迭�� ��ȯ
        object[] eventData = new object[] { intArray }; // object �迭�� int�� �迭�� ���Խ�Ŵ
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] { 1 } }; // ��ǥ actor ����
        byte eventCode = 102; // ����� �̺�Ʈ �ڵ�

        PhotonNetwork.RaiseEvent(eventCode, eventData, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEventReceived(EventData eventData)
    {
        Debug.Log("OnEventReceived");
        byte eventCode = eventData.Code;
        if (eventCode == 101)
        {
            object[] data = (object[])eventData.CustomData;
            List<DeviceInfo> connectedDevicesInfo = Deserialize<List<DeviceInfo>>((byte[])data[0]);
            DeviceInfo senderPlayerInfo = Deserialize<DeviceInfo>((byte[])data[1]);

            CompareAndUpdatePlayerInfo(connectedDevicesInfo, senderPlayerInfo);
        }
        else if (eventCode == 102) // �̺�Ʈ �ڵ� 102�� ���� ó��
        {
            object[] data = (object[])eventData.CustomData;
            int[] intArray = (int[])data[0];
            List<int> intList = new List<int>(intArray); // �迭�� �ٽ� List<int>�� ��ȯ

            // ��ȯ�� List<int> ���
            Debug.Log("Received int list:");
            foreach (int item in intList)
            {
                Debug.Log(item);
            }
        }
    }

    // CrossDeviceConnect�� ��û�ϴ� ��⿡�� �ڽ��� �������ִ� ����Ʈ�� ������ �Ѱ���, ���� �������� �ڽ��� ������ ��, �� ���� ���������� ������Ʈ
    // plays�� �ش��ϴ� ��󿡰Ը� RPC ȣ��
    void CompareAndUpdatePlayerInfo(List<DeviceInfo> connectedDevicesInfo, DeviceInfo senderPlayerInfo)
    {
        if (connectedDevicesInfo.Count > ConnectedDevicesInfo.Count)
        {
            // ����� ������ ������ ����, ������� + ���ڽ� �߰��� ������Ʈ
            connectedDevicesInfo.Add(MyDeviceInfo);
            foreach (DeviceInfo deviceInfo in connectedDevicesInfo)
            {
                // ActorNumber�� ����Ͽ� Player ��ü ��ȸ
                Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    // DeviceInfo ��ü�� ����ȭ�Ͽ� RPC ȣ��
                    byte[] serializedData = Serialize<List<DeviceInfo>>(connectedDevicesInfo);
                    photonView.RPC("RPC_SyncCrossDeviceState", player, serializedData);
                }
            }
        }
        else
        {
            // �� ������ ��뺸�� ����, ������ + ��� �߰��� ������Ʈ
            ConnectedDevicesInfo.Add(senderPlayerInfo);
            foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
            {
                // ActorNumber�� ����Ͽ� Player ��ü ��ȸ
                Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                if (player != null)
                {
                    // DeviceInfo ��ü�� ����ȭ�Ͽ� RPC ȣ��
                    byte[] serializedData = Serialize<List<DeviceInfo>>(ConnectedDevicesInfo);
                    photonView.RPC("RPC_SyncCrossDeviceState", player, serializedData);
                }
            }
        }
    }


    [PunRPC]
    private void RPC_SyncCrossDeviceState(byte[] serializedConnectedDevicesInfo)
    {
        Debug.Log("RPC_SyncCrossDeviceState");

        List<DeviceInfo> connectedDevicesInfo = Deserialize<List<DeviceInfo>>(serializedConnectedDevicesInfo);

        ConnectedDevicesInfo = connectedDevicesInfo;

        // ����̽� ���� ����ȭ�� �ʿ��� �ν��Ͻ� ����
        foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
        {
            switch (deviceInfo.DeviceType)
            {
                case DeviceType.Glasses:
                    AddComponentIfMissing<GlassesState>();
                    break;
                case DeviceType.Phone:
                    AddComponentIfMissing<PhoneState>();
                    break;
                case DeviceType.Watch:
                    break;
                case DeviceType.Ring:
                    break;
                default:
                    break;
            }
        }

        // 2�� RPC��
        foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
        {
            Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
            if (player != null)
            {
                photonView.RPC("RPC_IncreasePreparedConnectedDevices", player);
            }
        }
    }

    [PunRPC]
    private void RPC_IncreasePreparedConnectedDevices()
    {
        preparedConnectedDevices++;
        Debug.Log($"RPC_IncreasePreparedConnectedDevices - {preparedConnectedDevices}");
        if (preparedConnectedDevices == ConnectedDevicesInfo.Count)
        {
            SyncCrossDeviceState();
        }
    }

    private void SyncCrossDeviceState()
    {
        Debug.Log("SyncCrossDeviceState");
        // ���� �ش� ����̽����, �� ���¸� ������� ��ü ����ȭ ����
        switch (MyDeviceInfo.DeviceType)
        {
            case DeviceType.Glasses:
                // ����� �������� �۷��� RPC ����ȭ ȣ��
                foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
                {
                    Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                    if (player != null)
                    {
                        // ����ȭ�� RPC�� (������ ���� ����ȭ)
                    }
                }
                break;
            case DeviceType.Phone:
                // ����� �������� �� RPC ����ȭ ȣ��
                foreach (DeviceInfo deviceInfo in ConnectedDevicesInfo)
                {
                    Player player = GetPlayerByActorNumber(deviceInfo.ActorNumber);
                    if (player != null)
                    {
                        // ����ȭ�� RPC�� (������ ���� ����ȭ)
                        photonView.RPC("RPC_ObjCntrlModeBtnClick", player, Instance.ControlMode);
                    }
                }
                break;
            case DeviceType.Watch:
                break;
            case DeviceType.Ring:
                break;
            default:
                break;
        }
    }

    // DeviceInfo ��ü�� ����ȭ�ϴ� �޼���
    byte[] Serialize<T>(T obj)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
    }

    // DeviceInfo ��ü�� ������ȭ�ϴ� �޼���
    T Deserialize<T>(byte[] serializedData)
    {
        using (MemoryStream stream = new MemoryStream(serializedData))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }
    }

    // ActorNumber�� ����Ͽ� Player ��ü�� ã�� �޼���
    public Player GetPlayerByActorNumber(int actorNumber)
    {
        if (PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumber, out Player player))
        {
            return player;
        }
        return null;
    }

    private void AddComponentIfMissing<T>() where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            gameObject.AddComponent<T>();
        }
        else
        {
        }
    }


    // �� ������Ʈ������ ����� ���ϴ� ��󿡰Ը� �����ϱ����� Network Messages(RaiseEvent), RPC ����� ���
    /*
    public void InitializeWithOtherPlayer()
    {
        // ���� �÷��̾��� DeviceState�� ���� �÷��̾��� DeviceState�� ����ȭ
        if (PhotonNetwork.PlayerListOthers.Length > 0)
        {
            Player otherPlayer = PhotonNetwork.PlayerListOthers[0]; // ���� �÷��̾� �� �� �� ����
            if (otherPlayer.CustomProperties.TryGetValue("DeviceState", out object deviceStateObj))
            {
                DeviceState otherDeviceState = (DeviceState)deviceStateObj;
                // ���� �÷��̾��� DeviceState ������ ���� �÷��̾��� DeviceState�� ����
                ObjControlMode = otherDeviceState.ObjControlMode;
                SwipeDel = otherDeviceState.SwipeDel;
                GyroDel = otherDeviceState.GyroDel;
                IsObjBeingManip = otherDeviceState.IsObjBeingManip;
                SelectedObjFromPhone = otherDeviceState.SelectedObjFromPhone;
                SelectedObjFromGlasses = otherDeviceState.SelectedObjFromGlasses;
                GazeOnObjFromGlasses = otherDeviceState.GazeOnObjFromGlasses;
            }
        }
    }
    */

        // PhotonNetwork.Instantiate�� ������ ��ü���� OnPhotonSerializeView ����� ������Ʈ�ҽ� �Ʒ��� ���
        // �� ������Ʈ������ ����� ���ϴ� ��󿡰Ը� �����ϱ����� Network Messages(RaiseEvent), RPC ����� ���
        /*
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((int)ObjControlMode);
                stream.SendNext(SwipeDel);
                stream.SendNext(GyroDel);
                stream.SendNext(IsObjBeingManip);
                stream.SendNext((int)SelectedObjFromPhone);
                stream.SendNext((int)SelectedObjFromGlasses);
                stream.SendNext((int)GazeOnObjFromGlasses);
            }
            else
            {
                ObjControlMode = (ObjControlMode)(int)stream.ReceiveNext();
                SwipeDel = (Vector3)stream.ReceiveNext();
                GyroDel = (Vector3)stream.ReceiveNext();
                IsObjBeingManip = (bool)stream.ReceiveNext();
                SelectedObjFromPhone = (SelectedObjFromPhone)(int)stream.ReceiveNext();
                SelectedObjFromGlasses = (SelectedObjFromGlasses)(int)stream.ReceiveNext();
                GazeOnObjFromGlasses = (GazeOnObjFromGlasses)(int)stream.ReceiveNext();
            }
        }
        */
    }