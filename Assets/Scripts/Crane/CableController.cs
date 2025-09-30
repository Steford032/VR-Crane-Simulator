using UnityEngine;

public class CableController : MonoBehaviour
{
    [Header("����� ���������")]
    [SerializeField] private Transform cableStartPoint; // ����� �� �������
    [SerializeField] private Transform hookTransform;   // ��� ����

    void LateUpdate()
    {
        if (cableStartPoint == null || hookTransform == null)
        {
            return;
        }

        float distance = Vector3.Distance(cableStartPoint.position, hookTransform.position);

        // ������� (�����) ��������
        transform.localScale = new Vector3(transform.localScale.x, distance / 2.0f, transform.localScale.z);

        // ��������� �������� ����� ���������� ����� �������
        transform.position = (cableStartPoint.position + hookTransform.position) / 2.0f;

        // "����" �������� ������ ���� ��������� � �����
        transform.up = (hookTransform.position - cableStartPoint.position).normalized;
    }
}