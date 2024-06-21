using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TabbedMenu : MonoBehaviour
{
    public static TabbedMenu Instance { get; private set; }
    //�۽��� �������� �ε��� ���
    private string mainUxmlPath = "UXML/Main";
    private string settingUxmlPath = "UXML/Setting";

    //�̺�Ʈ ó�� ���ȭ 
    private TabbedMenuController tabbedMenuController;
    private PageController pageController;
    //�� ��ȯ�� ���� ������ ������������ ����������, ������������ ��� �̺�Ʈ�� ���� �ε�Ǵ� �������� �޶���
    private VisualElement mainContent;
    private VisualElement settingContent;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // �� �޴��� ���� ȭ�� ��ȯ�� �۵��ϵ��� ��Ʈ�ѷ� ���
            UIDocument menu = GetComponent<UIDocument>();
            VisualElement root = menu.rootVisualElement;

            tabbedMenuController = new TabbedMenuController(root);
            pageController = gameObject.AddComponent<PageController>();

            tabbedMenuController.RegisterTabCallbacks();

            // ���� ȭ�鿡 ��� ������ �ε�
            mainContent = root.Q<VisualElement>("mainContent");
            settingContent = root.Q<VisualElement>("settingContent");

            ReflectToMainContent(pageController.LoadNewPage(mainUxmlPath, PageController.UICategory.UI));
            ReflectToSettingContent(pageController.LoadNewPage(settingUxmlPath, PageController.UICategory.Setting));
        }
        else
        {
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            // ���� �ΰ��̹Ƿ� Ȱ��ȭ �� �������� �Է¹޵��� ó��
            if(tabbedMenuController.CurTab == TabbedMenuController.TabName.mainTab)
            {
                TemplateContainer root;
                if((root=pageController.LoadPrevPage(PageController.UICategory.UI)) != null)
                {
                    Debug.Log("Escape2");
                    ReflectToMainContent(root);
                }
            }
            else if (tabbedMenuController.CurTab == TabbedMenuController.TabName.settingTab)
            {
                TemplateContainer root;
                if ((root = pageController.LoadPrevPage(PageController.UICategory.Setting)) != null)
                {
                    Debug.Log("Escape3");
                    ReflectToSettingContent(root);
                }
            }
        }
    }

    public void ReflectToMainContent(TemplateContainer root)
    {
        mainContent.Clear();
        mainContent.Add(root);
    }

    public void ReflectToSettingContent(TemplateContainer root)
    {
        settingContent.Clear();
        settingContent.Add(root);
    }
}
