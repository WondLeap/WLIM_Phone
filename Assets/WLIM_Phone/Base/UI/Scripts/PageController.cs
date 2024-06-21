using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static CrossDeviceState;

public class PageController : MonoBehaviour
{
    public static PageController Instance { get; private set; }

    //event_onSelObjForWebview �߻��� �ε��� ���
    private string rover_antennaInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_antennaInfo";
    private string rover_headlampInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_headlampInfo";
    private string rover_solarpanelnfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverDetail/Rover_solarpanelInfo";
    private TabbedMenu tabbedMenu;

    // �ڷΰ��� ������ ���� ����
    private Stack<UIControllerInterface> UIStack = new Stack<UIControllerInterface>();
    private Stack<UIControllerInterface> SettingStack = new Stack<UIControllerInterface>();

    public enum UICategory
    {
        UI,
        Setting
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
        }
    }

    private void Start()
    {
        tabbedMenu = GetComponent<TabbedMenu>();
        // ����Ʈ�۷��������� ������ ���� Ʈ���� ��ü ���ý� ���� �Լ�
        // Phone �̺�Ʈ�� ���� Glasses UI�� �����Ǵ� �κ��� �������� �ǰ� �ٲ����

        // �������� �����Ǵ� GlassState �Լ��� ���� �ش� �Լ��� GlassState�� �����Ǵ� ������ �ܺο��� ����, �������� �����Ҷ��� ������ �Ʒ� �������
        // GlassesState.event_onSelObjForWebview.AddListener(LoadNewPageFromGlasses);
    }

    // UI�ε�
    public TemplateContainer LoadNewPage(string path, UICategory category)
    {
        //Debug.Log("PageController:LoadNewPage - " + path);
        VisualTreeAsset uxml = Resources.Load<VisualTreeAsset>(path);
        if (uxml != null)
        {
            TemplateContainer root = uxml.CloneTree();
            SetNewPage(path, root, uxml.name, category);
            return root;
        }
        else
        {
            Debug.LogError("Page Asset not found at path: " + path);
            return null;
        }
    }

    // UI�� ��� ����(UI�� ������ �̸��� ��ũ��Ʈ�� �ν��Ͻ�ȭ, Initialize�� ������ ��� ���ΰ� stack�� �����ɽø� ���� TemplateContainer ���� ����)
    private void SetNewPage(string path, TemplateContainer root, string uxmlName, UICategory category)
    {
        string scriptName = $"{typeof(PageController).Namespace}.{uxmlName}Script";
        Type scriptType = Type.GetType(scriptName);

        if (scriptType != null && typeof(UIControllerInterface).IsAssignableFrom(scriptType))
        {
            UIControllerInterface UIController = gameObject.AddComponent(scriptType) as UIControllerInterface;
            if (UIController != null)
            {
                UIController.Initialize(path, root);
            }
            else
            {
                Debug.LogError("UIController is null" + scriptName);
            }

            //ToDo, ���� Ȱ��ȭ �Ǿ��ִ� UI�� �ش��ϴ� ��Ʈ�ѷ��� �츮�� �������� �� disable�ص־� �Է¿� ���� ó���� ��ġ�� ������

            if (UIController != null)
            {
                if (category == UICategory.UI)
                {
                    // ���� UI�� ������ ���� UI�� �ڵ带 ��Ȱ��ȭ
                    if (UIStack.Count > 0)
                    {
                        UIControllerInterface topUI = UIStack.Peek();
                        MonoBehaviour monoBehaviour = topUI as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.enabled = false;
                        }
                    }
                    UIStack.Push((UIControllerInterface)UIController);
                    //Debug.Log("SetNewPage UIStack: " + UIStack.Count);
                }
                else if(category == UICategory.Setting)
                {
                    if (SettingStack.Count > 0)
                    {
                        UIControllerInterface topUI = SettingStack.Peek();
                        MonoBehaviour monoBehaviour = topUI as MonoBehaviour;
                        if (monoBehaviour != null)
                        {
                            monoBehaviour.enabled = false;
                        }
                    }
                    SettingStack.Push((UIControllerInterface)UIController);
                    //Debug.Log("SetNewPage SettingStack: " + SettingStack.Count);
                }           
            }
            else
            {
                Debug.LogError("Failed to add script: " + scriptName);
            }
        }
        else
        {
            Debug.LogError("�ش� UXML�� ���� ��Ʈ�ѷ��� Script�� ���ǵ��� �ʾҰų� UIControllerInterface�� ������� �ʾҽ��ϴ�.");
        }
    }

    //ó�� ���鶧�� UI���� ������ ����� ��������, �̹� ����� �ҷ������� ������ ������Ʈ�� ���� Ȱ��ȭ��Ű�� �ű⿡ ������ִ� ����� UI�� �ҷ���
    public TemplateContainer LoadPrevPage(UICategory category)
    {
        UIControllerInterface UIController;
        if (category == UICategory.UI)
        {
            if (UIStack.Count > 1)
            {
                UIController = UIStack.Peek();
                MonoBehaviour monoBehaviour;
                monoBehaviour = UIController as MonoBehaviour;
                Destroy(monoBehaviour);
                UIStack.Pop();
                UIController = UIStack.Peek();
                monoBehaviour = UIController as MonoBehaviour;
                monoBehaviour.enabled = true;
                //Debug.Log("LoadPrevPage UIStack: " + UIStack);
                return UIController.MyRoot;  
            }
            return null;
        }
        else if (category == UICategory.Setting)
        {
            if (SettingStack.Count > 1)
            {
                UIController = SettingStack.Peek();
                MonoBehaviour monoBehaviour;
                monoBehaviour = UIController as MonoBehaviour;
                Destroy(monoBehaviour);
                SettingStack.Pop();
                UIController = SettingStack.Peek();
                monoBehaviour = UIController as MonoBehaviour;
                monoBehaviour.enabled = true;
                //Debug.Log("LoadPrevPage SettingStack: " + SettingStack);
                return UIController.MyRoot;
            }
            return null;
        }
        else
            return null;
    }

    public void LoadNewPageFromGlasses()
    {
        string url;
        switch (GlassesState.Instance.SelObjForWebview_Gla)
        {
            case Obj_Gla.Antenna:
                url = rover_antennaInfoUxmlPath;
                break;
            case Obj_Gla.Headlamp:
                url = rover_headlampInfoUxmlPath;
                break;
            case Obj_Gla.Solarpanel:
                url = rover_solarpanelnfoUxmlPath;
                break;
            default:
                url = "";
                break;
        }
        // ���� UI�� ������ ���� ��û���� �������� �ߺ��Ǵ��� üũ
        if (UIStack.Count > 0)
        {
            UIControllerInterface topUI = UIStack.Peek();
            if (url == topUI.MyURL)
                return;
        }
        if(url != "")
        {
            tabbedMenu.ReflectToMainContent(LoadNewPage(url, PageController.UICategory.UI));
        }
    }
}
