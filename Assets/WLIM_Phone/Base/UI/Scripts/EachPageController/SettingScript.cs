using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingScript : MonoBehaviour, UIControllerInterface
{
    public string MyURL { get; set; }
    public TemplateContainer MyRoot { get; set; }

    private string gazeInteractionModeUxmlPath = "UXML/SettingItems/GazeInteractionMode";
    private Button gazeInteractionModeButton;
    private Button webVizModeButton;
    public void Initialize(string url, TemplateContainer root)
    {
        MyURL = url;
        MyRoot = root;
        gazeInteractionModeButton = MyRoot.Q<Button>("GazeInteractionModeButton");
        gazeInteractionModeButton.RegisterCallback<ClickEvent>(OnGazeInteractionModeButtonClicked);
        gazeInteractionModeButton = MyRoot.Q<Button>("WebVizModeButton");
        gazeInteractionModeButton.RegisterCallback<ClickEvent>(OnWebVizModeButtonClicked);
    }
    private void OnGazeInteractionModeButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnGazeInteractionModeButtonClicked");
        if (GlassesState.Instance == null) // �۷��� ���� �ȉ������� �׳� �۵�
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(gazeInteractionModeUxmlPath, PageController.UICategory.Setting);
            gameObject.GetComponent<TabbedMenu>().ReflectToSettingContent(root);
        } // �۷��� ���� �������� �ü��� ��ü ��Ʈ�ѷ� ��� �ȵǰ��������� �۵�
        else if (GlassesState.Instance != null && GlassesState.Instance.GazeOnObj_Gla == Obj_Gla.Null)
        {
            TemplateContainer root = gameObject.GetComponent<PageController>().LoadNewPage(gazeInteractionModeUxmlPath, PageController.UICategory.Setting);
            gameObject.GetComponent<TabbedMenu>().ReflectToSettingContent(root);
        }
    }

    private void OnWebVizModeButtonClicked(ClickEvent evt)
    {
        // �̰� ��ư�� ������������ ����, �� â���� ���� �� �� �ִ� UI�� �ٲ�� �ҵ�..?
        // �ϴ� ���� �Ǵ°� �⺻���� �صΰ� �׽�Ʈ �Ϸ� �� ���׷��̵� ����
    }
}
