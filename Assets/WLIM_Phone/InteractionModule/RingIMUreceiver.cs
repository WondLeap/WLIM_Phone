using TMPro;
using UnityEngine;

public class RingIMUreceiver : MonoBehaviour
{
    private AndroidJavaClass UnityDotCommunication;
    private AndroidJavaObject _unityDotCommunicationInstance;

    // �׽�Ʈ�� �ӽ�
    public TextMeshProUGUI sensorDataText;

    // Singleton instance
    private static RingIMUreceiver _instance;

    // Public property to access the singleton instance
    public static RingIMUreceiver Instance
    {
        get { return _instance; }
    }

    // Sensor data received from Android
    private RingIMU _ringIMU;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of the class exists
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the object alive between scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // �÷������� UnidyDotCommucation�� intantiate�ϸ�, �ű⼭ Base�� instantiate, �ϴ��� ReceiveRingIMU�� ȣ����
        UnityDotCommunication = new AndroidJavaClass("com.example.xsensedot.UnityDotCommunication");
        _unityDotCommunicationInstance = UnityDotCommunication.CallStatic<AndroidJavaObject>("instnace");
    }

    // �ȵ���̵忡�� ���޵� ���� �����͸� ó���ϴ� �Լ�
    public void ReceiveRingIMU(string jsonData)
    {
        // ���� ������ ó��
        Debug.Log("Received sensor data: " + jsonData);

        // JSON ������ �����͸� �ʿ��� ���·� �Ľ��Ͽ� Ȱ���� �� �ֽ��ϴ�.
        // ����: JSON ���ڿ��� �ٽ� ��ü�� ��ȯ
        _ringIMU = JsonUtility.FromJson<RingIMU>(jsonData);
        sensorDataText.text = jsonData;
        // ���⿡ �����͸� Ȱ���ϴ� �ڵ� �ۼ�
    }

    // �ܺο��� RingIMU�� ������ �� �ִ� ������Ƽ
    public RingIMU ringIMU
    {
        get { return _ringIMU; }
    }

    // �ȵ���̵忡�� ������ ���� �����͸� �Ľ��ϱ� ���� Ŭ����
    [System.Serializable]
    public class RingIMU
    {
        public float[] acc;
        public float[] gyr;
    }
}