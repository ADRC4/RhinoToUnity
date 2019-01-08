using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] AudioClip _hitClip;

    AudioSource _audio;
    float _lastShot = 0.0f;
    MeshCollider[] _tiles;

    void Start()
    {
        _audio = GetComponent<AudioSource>();

        var parent = GameObject.Find("Assembly");
        if (parent == null)
        {
            Debug.Log("No assembly found");
            return;
        }

        _tiles = parent.GetComponentsInChildren<MeshCollider>();
        var joints = parent.GetComponentsInChildren<FixedJoint>();

       // StartCoroutine(RandomExplosion());
    }

    IEnumerator RandomExplosion()
    {
        var bbox = new Bounds();
        foreach (var tile in _tiles)
            bbox.Encapsulate(tile.bounds);

        var explosion = new GameObject("Explosion",typeof(AudioSource)).transform;
        var audio = explosion.GetComponent<AudioSource>();

        //  var layer = LayerMask.NameToLayer("Tiles");

        while (true)
        {
            float x = Random.Range(bbox.min.x, bbox.max.x);
            float y = Random.Range(bbox.min.y, bbox.max.y);
            float z = Random.Range(bbox.min.z, bbox.max.z);
            var p = new Vector3(x, y, z);
            float r = Random.Range(0.5f, 1f);

            explosion.position = p;
            audio.PlayOneShot(_hitClip);

            var colliders = Physics.OverlapSphere(p, r);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.layer != 9) continue;

                var force = Random.Range(2000f, 4000f);

                var rb = collider.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddExplosionForce(force, p, r);

                var color = Random.ColorHSV(0.2f, 0.8f, 0.5f, 0.5f, 2.0f, 2.0f, 0.0f, 0.0f);
                collider.GetComponent<TileInstance>().Hit(color);
            }

            yield return new WaitForSeconds(Random.Range(0.2f, 2f));
        }
    }

    void Update()
    {
        Shoot();
    }

    private void OnGUI()
    {
        //     GUI.Label(new Rect(25, 24, 200, 40), $"Gravity: {_gravity:0.00}");
    }

    void Shoot()
    {
        const float shootDelta = 0.15f;

        if (Time.time < _lastShot + shootDelta) return;
        bool fire = Input.GetAxisRaw("Fire") == 1.0f;

        if (fire)
        {
            _audio.Play();

            Instantiate(
                _laserPrefab,
                transform.position + transform.rotation * _laserPrefab.transform.position,
                transform.rotation * _laserPrefab.transform.rotation);

            _lastShot = Time.time;
        }
    }
}
