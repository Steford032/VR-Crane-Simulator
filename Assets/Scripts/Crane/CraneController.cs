// VR-Crane-Simulator/Assets/Scripts/Crane/CraneController.cs
using UnityEngine;

public class CraneController : MonoBehaviour
{
    [Header("������ �� ����������")]
    [SerializeField] private Rigidbody bridgeRigidbody;  // ������ �� Rigidbody ����� (BeamHolder)
    [SerializeField] private Rigidbody trolleyRigidbody; // ������ �� Rigidbody ������� (Trolley)
    [SerializeField] private Rigidbody hookRigidbody;    // ������ �� Rigidbody ����� (Hook)
    [SerializeField] private Transform tubeTransform;    // ������� �������

    [SerializeField] private AudioSource chainAudioSource;

    [Header("�������� ��������")]
    [SerializeField] private float bridgeSpeed = 1.5f;   // �������� ����� (�����/��)
    [SerializeField] private float trolleySpeed = 3.0f;  // �������� ������� (�����/������)
    [SerializeField] private float hookSpeed = 1.5f;     // �������� ����� (�����/����)
    [SerializeField] private float tubeRotationSpeed = 150f; // �������� �������� �������

    [Header("������� �������� ����� (�� ��� Z)")]
    [SerializeField] private float minZBoundary = -3;
    [SerializeField] private float maxZBoundary = 3;

    [Header("������� �������� ������� ����� (�� ��� X)")]
    [SerializeField] private float minXBoundary = -3f;
    [SerializeField] private float maxXBoundary = 3f;

    [Header("������� �������� ����� (�� ��� Y)")]
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
            Debug.Log("����������� ��������: " + movementDirection);
            debugTimer = 0f; // Reset the timer
        }
    }

    // FixedUpdate ������������ ��� ���� �������� � Rigidbody
    private void FixedUpdate()
    {
        if (movementDirection == Vector3.zero)
        {
            StopChainSound();
            return;
        }

        // �������� ����� (Bridge) �� ���������� ��� Z � ��������� (�������� ��� ���������)
        Vector3 bridgeVelocity = new Vector3(0, 0, movementDirection.z) * bridgeSpeed;
        Vector3 newBridgePos = bridgeRigidbody.position + bridgeVelocity * Time.fixedDeltaTime;
        newBridgePos.z = Mathf.Clamp(newBridgePos.z, minZBoundary, maxZBoundary);
        bridgeRigidbody.MovePosition(newBridgePos);

        // �������� ������� (Trolley) �� ��������� ��� X � ���������
        if (movementDirection.x != 0)
        {
            // �������� � ��������� �������� ��� �������� ���������� ������
            Vector3 localTrolleyPos = trolleyRigidbody.transform.localPosition;
            Vector3 trolleyVelocity = new Vector3(movementDirection.x, 0, 0) * trolleySpeed;
            localTrolleyPos += trolleyVelocity * Time.fixedDeltaTime;

            // ��������� ����������� �� ��������� ��� X
            localTrolleyPos.x = Mathf.Clamp(localTrolleyPos.x, minXBoundary, maxXBoundary);

            // ������������� ����� ��������� �������
            trolleyRigidbody.transform.localPosition = localTrolleyPos;
        }

        // �������� ����� (Hook) �� ��������� ��� Y � ���������
        if (movementDirection.y != 0)
        {
            // ����� ��� �� �������� � ��������� ��������
            Vector3 localHookPos = hookRigidbody.transform.localPosition;
            Vector3 hookVelocity = new Vector3(0, movementDirection.y, 0) * hookSpeed;
            localHookPos += hookVelocity * Time.fixedDeltaTime;

            // ��������� ����������� �� ��������� ��� Y
            localHookPos.y = Mathf.Clamp(localHookPos.y, minYBoundary, maxYBoundary);

            // ������������� ����� ��������� �������
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


    // --- ��������� ������ ��� ������ � ������ ---

    /// <summary>
    /// �������� �������� � ��������� �����������.
    /// direction: X - �����/������, Y - �����/����, Z - �����/��
    /// </summary>
    public void StartMovement(Vector3 direction)
    {
        movementDirection = direction.normalized;
    }

    /// <summary>
    /// ������������� ����� ��������.
    /// </summary>
    public void StopMovement()
    {
        movementDirection = Vector3.zero;
    }

    void PlayChainSound()
    {
        // ����������� ����, ������ ���� �� ��� �� �������������
        if (!chainAudioSource.isPlaying)
        {
            chainAudioSource.Play();
        }
    }

    void StopChainSound()
    {
        // ������������� ����, ���� �� �������������
        if (chainAudioSource.isPlaying)
        {
            chainAudioSource.Stop();
        }
    }
}