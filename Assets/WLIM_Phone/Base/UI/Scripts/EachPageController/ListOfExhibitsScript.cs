using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListOfExhibitsScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    // ��ư�� ���ε� ���� ������ ���
    private string roverInfoUxmlPath = "UXML/MainItems/ExhibitDetail/RoverInfo";
    private Button roverInfoButton;

    // TebbedMenu���� ���� �������� ���Ƴ��� �� ��ũ��Ʈ�� �ν��Ͻ��� ���� �� ���� ���ֹǷ� ���⼭�� ���� ��ư ��ɸ� �������ָ� ��
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        roverInfoButton = MyRoot.Q<Button>("RoverButton");
        roverInfoButton.RegisterCallback<ClickEvent>(OnRoverInfoButtonClicked);
    }

    private void OnRoverInfoButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnRoverInfoButtonButtonClicked");
        if (GlassesState.Instance == null) // ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(roverInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        } // ������������ �ü��� ��ü������ ���� �ʴ� �����϶� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(roverInfoUxmlPath, PageController.UICategory.UI);
            gameObject.GetComponent<TabbedMenu>().ReflectToMainContent(root);
        }
    }
}
