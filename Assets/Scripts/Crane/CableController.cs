using UnityEngine;

public class CableController : MonoBehaviour
{
    [Header("Точки крепления")]
    [SerializeField] private Transform cableStartPoint; // Точка на лебедке
    [SerializeField] private Transform hookTransform;   // Сам крюк

    void LateUpdate()
    {
        if (cableStartPoint == null || hookTransform == null)
        {
            return;
        }

        float distance = Vector3.Distance(cableStartPoint.position, hookTransform.position);

        // Масштаб (длина) цилиндра
        transform.localScale = new Vector3(transform.localScale.x, distance / 2.0f, transform.localScale.z);

        // Положение цилиндра ровно посередине между точками
        transform.position = (cableStartPoint.position + hookTransform.position) / 2.0f;

        // "верх" цилиндра должен быть направлен к крюку
        transform.up = (hookTransform.position - cableStartPoint.position).normalized;
    }
}