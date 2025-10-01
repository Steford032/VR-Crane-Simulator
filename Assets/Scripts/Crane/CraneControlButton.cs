using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// This defines our custom event that can pass a Vector3
[System.Serializable]
public class MovementEvent : UnityEvent<Vector3> { }

// Your class now uses the definition above
public class CraneControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Настройки кнопки")]
    [Tooltip("Направление, которое эта кнопка отправляет.")]
    [SerializeField] private Vector3 movementDirection;
    [Tooltip("Текст подсказки для этой кнопки.")]
    [SerializeField] private string tooltipText = "Описание действия";

    [Header("Ссылки на компоненты")]
    [Tooltip("Объект, который будет включаться при наведении.")]
    [SerializeField] private GameObject highlightObject;

    [Header("События для CraneController")]
    public MovementEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true); // Включаем подсветку
        }
        TooltipManager.instance.ShowTooltip(tooltipText);
    }

    // Вызывается, когда указатель покидает коллайдер
    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
        TooltipManager.instance.HideTooltip();
    }

    // Вызывается при нажатии
    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPressed.Invoke(movementDirection);
    }

    // Вызывается при отпускании
    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonReleased.Invoke();
    }
}