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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            var rigidBody = other.GetComponent<Rigidbody>();

            if (rigidBody.isKinematic)
            {
                rigidBody.isKinematic = false;
            }

            rigidBody.AddForceAtPosition(transform.up * 2000.0f, transform.position, ForceMode.Force);
            other.GetComponent<TileInstance>().Hit(_color);
        }

        Destroy(gameObject);
    }
}
