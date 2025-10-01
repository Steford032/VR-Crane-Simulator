using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// This defines our custom event that can pass a Vector3
[System.Serializable]
public class MovementEvent : UnityEvent<Vector3> { }

// Your class now uses the definition above
public class CraneControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("��������� ������")]
    [Tooltip("�����������, ������� ��� ������ ����������.")]
    [SerializeField] private Vector3 movementDirection;
    [Tooltip("����� ��������� ��� ���� ������.")]
    [SerializeField] private string tooltipText = "�������� ��������";

    [Header("������ �� ����������")]
    [Tooltip("������, ������� ����� ���������� ��� ���������.")]
    [SerializeField] private GameObject highlightObject;

    [Header("������� ��� CraneController")]
    public MovementEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true); // �������� ���������
        }
        TooltipManager.instance.ShowTooltip(tooltipText);
    }

    // ����������, ����� ��������� �������� ���������
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
        TooltipManager.instance.HideTooltip();
    }

    // ���������� ��� �������
    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed.Invoke(movementDirection);
    }

    // ���������� ��� ����������
    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonReleased.Invoke();
    }
}