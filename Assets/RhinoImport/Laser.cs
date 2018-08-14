using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Laser : MonoBehaviour
{
    Color _color;

    void Start()
    {
        _color = Random.ColorHSV(0.2f, 0.8f, 0.5f, 0.5f, 2.0f, 2.0f, 0.0f, 0.0f);
        var renderer = GetComponent<MeshRenderer>();
        var block = new MaterialPropertyBlock();
        block.SetColor("_EmissionColor", _color);
        renderer.SetPropertyBlock(block);

        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity += transform.up * 20.0f;
        Destroy(gameObject, 60);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            var rigidBody = collision.collider.GetComponent<Rigidbody>();

            if (rigidBody.isKinematic)
            {
                rigidBody.isKinematic = false;
            }

            Vector3 center = Vector3.zero;
            foreach (var contact in collision.contacts)
                center += contact.point;

            center /= collision.contacts.Length;
            rigidBody.AddForceAtPosition(transform.up * 2000.0f, center, ForceMode.Force);
           // collision.collider.GetComponent<TileInstance>().Hit();
        }

        Destroy(gameObject);
    }
}
