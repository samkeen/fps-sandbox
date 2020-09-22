using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private Transform playerBody;

    private float headTilt; 
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * this.mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * this.mouseSensitivity * Time.deltaTime;

        this.headTilt -= mouseY;
        this.headTilt = Mathf.Clamp(this.headTilt, -90f, 90f);

        this.transform.localRotation = Quaternion.Euler(this.headTilt, 0f, 0f);
        this.playerBody.Rotate(Vector3.up * mouseX);
    }
}
