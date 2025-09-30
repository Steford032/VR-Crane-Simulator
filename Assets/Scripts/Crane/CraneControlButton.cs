using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// This defines our custom event that can pass a Vector3
[System.Serializable]
public class MovementEvent : UnityEvent<Vector3> { }

// Your class now uses the definition above
public class CraneControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Tooltip("The direction this button sends.")]
    [SerializeField] private Vector3 movementDirection;

    [Header("Events for CraneController")]
    public MovementEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    // This method is automatically called when the Vive Caster detects a press
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " PRESSED!"); // Debug line for testing
        OnButtonPressed.Invoke(movementDirection);
    }

    // This method is automatically called when the Vive Caster detects a release
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " RELEASED!"); // Debug line for testing
        OnButtonReleased.Invoke();
    }
}