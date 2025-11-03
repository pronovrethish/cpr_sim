using UnityEngine;

public class SimpleWalk : MonoBehaviour
{
    public float speed = 3f;
    public float mouseSensitivity = 2f;
    float rotX = 0f, rotY = 0f;

    void Update()
    {
        // Keyboard movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        transform.position += move * speed * Time.deltaTime;

        // Mouse look
        rotX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotY = Mathf.Clamp(rotY, -80, 80);
        transform.localRotation = Quaternion.Euler(rotY, rotX, 0);
    }
}
