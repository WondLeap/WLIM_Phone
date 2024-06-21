using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController_example : MonoBehaviour
{
    //���� ��Ʈ �θ�
    private VisualElement _bottomContainer;
    //���� ��ư
    private Button _openButton;
    //�ݱ� ��ư
    private Button _closeButton;
    //���� ��Ʈ
    private VisualElement _bottomSheet;
    //������
    private VisualElement _scrim;

    //�ҳ�� �ҳ�
    private VisualElement _boy;
    private VisualElement _girl;

    //�ҳ� ���� ���̺�
    private Label _message;


    // Start is called before the first frame update
    void Start()
    {
        //UI ��ť��Ʈ�� �ִ� �ֻ��� ���־󿤸���Ʈ�� �����Ѵ�.
        var root = GetComponent<UIDocument>().rootVisualElement;

        //���� ��Ʈ�� �θ�
        _bottomContainer = root.Q<VisualElement>("Container_Bottom");

        //����, �ݱ� ��ư
        _openButton = root.Q<Button>("Button_Open");
        _closeButton = root.Q<Button>("Button_Close");
        //���� ��Ʈ�� ������
        _bottomSheet = root.Q<VisualElement>("BottomSheet");
        _scrim = root.Q<VisualElement>("Scrim");

        //�ҳ�� �ҳ�
        _boy = root.Q<VisualElement>("Image_Boy");
        _girl = root.Q<VisualElement>("Image_Girl");

        //�ҳ� ���� �޽���
        _message = root.Q<Label>("Message");

        //������ �� ���� ��Ʈ �׷��� �����.
        _bottomContainer.style.display = DisplayStyle.None;

        //��ư�� �� ��
        _openButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        _closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        //�ҳ� �ִϸ��̼�
        //AnimateBoy();
        //���� �����ϰ� 0.5�� �ڿ� �ҳ��� �ִϸ��̼� �Ѵ�.
        Invoke("AnimateBoy", .5f);

        //���ҽ�Ʈ�� ������ ���� �׷��� ����.
        _bottomSheet.RegisterCallback<TransitionEndEvent>(OnBottomSheetDown);
    }

    private void AnimateBoy()
    {
        _boy.RemoveFromClassList("image--boy--inair");
    }

    private void Update()
    {
        //Ư�� ��Ÿ�� Ŭ������ ����Ʈ �ȿ� �ִ��� �� ������ Ȯ���Ѵ�.
        //Debug.Log(_boy.ClassListContains("image--boy--inair"));
    }


    private void OnOpenButtonClicked(ClickEvent evt)
    {
        //���ҽ�Ʈ �׷��� �����ش�.
        _bottomContainer.style.display = DisplayStyle.Flex;

        //���ҽ�Ʈ�� ������ �ִϸ��̼�
        _bottomSheet.AddToClassList("bottomsheet--up");
        _scrim.AddToClassList("scrim--fadein");

        AnimateGirl();
    }

    private void AnimateGirl()
    {
        //�ҳ� Ŭ��������Ʈ�� �ִ� image--girl--up�� �߰��ϰų� �����Ѵ�.
        _girl.ToggleInClassList("image--girl--up");

        //Ʈ�������� ���� ��, image--girl--up�� �߰��ϰų� �����Ѵ�.
        _girl.RegisterCallback<TransitionEndEvent>
        (
            evt => _girl.ToggleInClassList("image--girl--up")
        );
    }

    private void OnCloseButtonClicked(ClickEvent evt)
    {
        //���ҽ�Ʈ�� ������ �ִϸ��̼�
        _bottomSheet.RemoveFromClassList("bottomsheet--up");
        _scrim.RemoveFromClassList("scrim--fadein");
    }

    private void OnBottomSheetDown(TransitionEndEvent evt)
    {
        // ������ ���� Ʈ�����ǿ����� ���ҽ�Ʈ �׷��� ���߰� �Ѵ�.
        if (!_bottomSheet.ClassListContains("bottomsheet--up"))
        {
            //���ҽ�Ʈ �׷��� �����.
            _bottomContainer.style.display = DisplayStyle.None;
        }
    }
}
