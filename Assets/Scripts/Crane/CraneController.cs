// VR-Crane-Simulator/Assets/Scripts/Crane/CraneController.cs
using UnityEngine;

public class CraneController : MonoBehaviour
{
    [Header("Ссылки на компоненты")]
    [SerializeField] private Rigidbody bridgeRigidbody;  // Ссылка на Rigidbody моста (BeamHolder)
    [SerializeField] private Rigidbody trolleyRigidbody; // Ссылка на Rigidbody тележки (Trolley)
    [SerializeField] private Rigidbody hookRigidbody;    // Ссылка на Rigidbody крюка (Hook)
    [SerializeField] private Transform tubeTransform;    // Катушка тележки

    [SerializeField] private AudioSource chainAudioSource;

    [Header("Скорости движения")]
    [SerializeField] private float bridgeSpeed = 1.5f;   // Скорость моста (Север/Юг)
    [SerializeField] private float trolleySpeed = 3.0f;  // Скорость тележки (Запад/Восток)
    [SerializeField] private float hookSpeed = 1.5f;     // Скорость крюка (Вверх/Вниз)
    [SerializeField] private float tubeRotationSpeed = 150f; // Скорость вращения катушки

    [Header("Границы движения моста (по оси Z)")]
    [SerializeField] private float minZBoundary = -3;
    [SerializeField] private float maxZBoundary = 3;

    [Header("Границы движения каретки крюка (по оси X)")]
    [SerializeField] private float minXBoundary = -3f;
    [SerializeField] private float maxXBoundary = 3f;

    [Header("Границы движения крюка (по оси Y)")]
    [SerializeField] private float minYBoundary = -1f;
    [SerializeField] private float maxYBoundary = 1f;

    private Vector3 movementDirection;

    private float debugTimer = 0f;

    void Update()
    {
        // Add 1 to the timer every second
        debugTimer += Time.deltaTime;

        // If 1 second has passed, print the log and reset the timer
        if (debugTimer >= 3f)
        {
            Debug.Log("Направление движения: " + movementDirection);
            debugTimer = 0f; // Reset the timer
        }
    }

    // FixedUpdate используется для всех операций с Rigidbody
    private void FixedUpdate()
    {
        if (movementDirection == Vector3.zero)
        {
            StopChainSound();
            return;
        }

        // Движение Моста (Bridge) по глобальной оси Z с границами (остается без изменений)
        Vector3 bridgeVelocity = new Vector3(0, 0, movementDirection.z) * bridgeSpeed;
        Vector3 newBridgePos = bridgeRigidbody.position + bridgeVelocity * Time.fixedDeltaTime;
        newBridgePos.z = Mathf.Clamp(newBridgePos.z, minZBoundary, maxZBoundary);
        bridgeRigidbody.MovePosition(newBridgePos);

        // Движение Тележки (Trolley) по локальной оси X с границами
        if (movementDirection.x != 0)
        {
            // Работаем с локальной позицией для простоты применения границ
            Vector3 localTrolleyPos = trolleyRigidbody.transform.localPosition;
            Vector3 trolleyVelocity = new Vector3(movementDirection.x, 0, 0) * trolleySpeed;
            localTrolleyPos += trolleyVelocity * Time.fixedDeltaTime;

            // Применяем ограничение по локальной оси X
            localTrolleyPos.x = Mathf.Clamp(localTrolleyPos.x, minXBoundary, maxXBoundary);

            // Устанавливаем новую локальную позицию
            trolleyRigidbody.transform.localPosition = localTrolleyPos;
        }

        // Движение Крюка (Hook) по локальной оси Y с границами
        if (movementDirection.y != 0)
        {
            // Точно так же работаем с локальной позицией
            Vector3 localHookPos = hookRigidbody.transform.localPosition;
            Vector3 hookVelocity = new Vector3(0, movementDirection.y, 0) * hookSpeed;
            localHookPos += hookVelocity * Time.fixedDeltaTime;

            // Применяем ограничение по локальной оси Y
            localHookPos.y = Mathf.Clamp(localHookPos.y, minYBoundary, maxYBoundary);

            // Устанавливаем новую локальную позицию
            hookRigidbody.transform.localPosition = localHookPos;

            if (!(localHookPos.y <= minYBoundary) && !(localHookPos.y >= maxYBoundary))
            {
                tubeTransform.Rotate(Vector3.right, -movementDirection.y * tubeRotationSpeed * Time.fixedDeltaTime, Space.Self);
                PlayChainSound();
            }
            else
            {
                StopChainSound();
            }
        }
    }


    // --- Публичные методы для вызова с кнопок ---

    /// <summary>
    /// Начинает движение в указанном направлении.
    /// direction: X - Запад/Восток, Y - Вверх/Вниз, Z - Север/Юг
    /// </summary>
    public void StartMovement(Vector3 direction)
    {
        movementDirection = direction.normalized;
    }

    /// <summary>
    /// Останавливает любое движение.
    /// </summary>
    public void StopMovement()
    {
        movementDirection = Vector3.zero;
    }

    void PlayChainSound()
    {
        // Проигрываем звук, только если он еще не проигрывается
        if (!chainAudioSource.isPlaying)
        {
            chainAudioSource.Play();
        }
    }

    void StopChainSound()
    {
        // Останавливаем звук, если он проигрывается
        if (chainAudioSource.isPlaying)
        {
            chainAudioSource.Stop();
        }
    }
}