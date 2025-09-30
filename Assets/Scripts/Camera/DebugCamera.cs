using UnityEngine;

public class DebugCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    [SerializeField] private float lookSpeed = 3.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Update()
    {
        // Вращение камеры при зажатой правой кнопке мыши
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * sprintMultiplier : moveSpeed;

        float moveX = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, 0, moveZ));
    }
}