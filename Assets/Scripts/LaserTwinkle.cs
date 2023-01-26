using System;
using System.Collections;
using UnityEngine;

public class LaserTwinkle : MonoBehaviour
{
    [Header("闪烁间隔时间")]
    public float interval = 2f;

    private MeshRenderer _meshRenderer;
    private BoxCollider _boxCollider;
    private AudioSource _audioSource;
    private Light _light;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _light = GetComponent<Light>();
        _boxCollider = GetComponent<BoxCollider>();
        _audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            _meshRenderer.enabled = !_meshRenderer.enabled;
            _light.enabled = !_light.enabled;
            _boxCollider.enabled = !_boxCollider.enabled;
            _audioSource.enabled = !_audioSource.enabled;
        }
    }
}
