using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInstance : MonoBehaviour
{
    MeshRenderer _renderer;
    MaterialPropertyBlock _block;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _block = new MaterialPropertyBlock();
        Color diffuse = Color.white * Random.Range(0.6f, 0.8f);
        _block.SetColor("_Color", diffuse);
        _renderer.SetPropertyBlock(_block);
    }

    public void Hit(Color color)
    {
        StartCoroutine(Gradient(0.25f, color));
    }

    IEnumerator Gradient(float time, Color color)
    {
        float t = 0.0f;

        while (t <= 1.0f)
        {
            var emission = Color.Lerp(color, Color.black, t);
            _block.SetColor("_EmissionColor", emission);
            _renderer.SetPropertyBlock(_block);
            t += Time.deltaTime / time;
            yield return null;
        }

        _block.SetColor("_EmissionColor", Color.black);
        _renderer.SetPropertyBlock(_block);
    }
}
