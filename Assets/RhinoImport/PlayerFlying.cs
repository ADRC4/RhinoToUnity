using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlying : MonoBehaviour
{
    [SerializeField] public GameObject _laserPrefab;

    AudioSource _audio;
    CharacterController _controller;
    float lastShot = 0.0f;

    float _gravity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _audio = GetComponent<AudioSource>();
        _controller = GetComponent<CharacterController>();
        Physics.gravity = Vector3.zero;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(25, 24, 200, 40), $"Gravity: {_gravity:0.00}");
    }

    void Update()
    {
        Move();
        Shoot();
        SetGravity();
    }

    void SetGravity()
    {
        float increment = Input.GetAxisRaw("Mouse ScrollWheel");

        if(increment != 0)
        {
            _gravity += increment;
            Physics.gravity = Vector3.up * _gravity;
        }
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

    void Shoot()
    {
        const float shootDelta = 0.15f;

        if (Time.time < lastShot + shootDelta) return;
        bool fire = Input.GetAxisRaw("Fire") == 1.0f;

        if (fire)
        {
            _audio.Play();

            Instantiate(
                _laserPrefab,
                transform.position + transform.rotation * _laserPrefab.transform.position,
                transform.rotation * _laserPrefab.transform.rotation);

            lastShot = Time.time;
        }
    }
}
