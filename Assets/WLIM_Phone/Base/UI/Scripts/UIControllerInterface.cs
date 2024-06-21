
using UnityEngine.UIElements;

public interface UIControllerInterface
{
    string MyURL { get; set; } //glasses���� UI ������ ������ ������ UI�� ������ ������ ���� ���ϱ� ����
    TemplateContainer MyRoot { get; set; } //stack�� ����� ��Ʈ�ѷ��� ������ �ű⿡ �ش�Ǵ� UI�� �ҷ����� ����

    void Initialize(string url, TemplateContainer root); //��� ������� UI�� ���� �ҷ��ý� �ش� �̸��� ������ Script�� �����ϰ� Initialize�� ���� ��� ����
}