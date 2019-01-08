using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFlying : MonoBehaviour
{
    CharacterController _controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

       _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        const float speed = 0.1f;
        const float lookSpeed = 1.0f;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float z = Input.GetAxisRaw("Forward");
        float yaw = Input.GetAxisRaw("Mouse X") * lookSpeed;
        float pitch = Input.GetAxisRaw("Mouse Y") * lookSpeed;

        Vector3 velocity = new Vector3(x, y, z).normalized * speed;
        velocity = transform.rotation * velocity;
        _controller.Move(velocity);

        transform.Rotate(Vector3.up, yaw, Space.World);
        transform.Rotate(Vector3.right, pitch, Space.Self);
    }
}
