using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{

    // Transform of the GameObject you want to shake
    private Transform c_transform;

    // Desired duration of the shake effect
    public  float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;

    void Awake()
    {
        if (c_transform == null)
        {
            c_transform = Camera.main.transform;
        }

        initialPosition = c_transform.localPosition;
    }

    private void Start()
    {
        if (c_transform == null)
        {
            c_transform = Camera.main.transform;
        }

        initialPosition = c_transform.localPosition;
    }

    void OnEnable()
    {
        if (c_transform == null)
        {
            c_transform = Camera.main.transform;
        }

        initialPosition = c_transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            c_transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            initialPosition = c_transform.localPosition;
        }
    }

    public void ShakeOnce(float shakeLength)
    {
        shakeDuration = shakeLength;
    }
}
