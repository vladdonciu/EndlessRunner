using UnityEngine;
using System.Collections;

public class CameraOscillateY : MonoBehaviour
{
    [Header("Oscilații")]
    public float amplitude = 10f;
    public float frequency = 1f;

    [Header("Tranziții")]
    public float rotationDuration = 2f;
    public float resetSpeed = 1.5f;

    private Quaternion initialRotation;
    private float baseY;
    private Coroutine activeCoroutine;
    private bool isOscillating = true;

    void Start()
    {
        initialRotation = transform.rotation;
        baseY = initialRotation.eulerAngles.y;
    }

    void Update()
    {
        if (isOscillating)
        {
            float newY = baseY + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.rotation = Quaternion.Euler(
                initialRotation.eulerAngles.x,
                newY,
                initialRotation.eulerAngles.z
            );
        }
    }

    public void RotateToTarget(float targetY)
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(RotateCoroutine(targetY));
    }

    public void ResetRotation()
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(ResetCoroutine());
    }

    IEnumerator RotateCoroutine(float targetY)
    {
        isOscillating = false;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(startRot.eulerAngles.x, targetY, startRot.eulerAngles.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        // Asigură poziția finală
        transform.rotation = targetRot;
        activeCoroutine = null;
    }

    IEnumerator ResetCoroutine()
    {
        Quaternion currentRot = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * resetSpeed;
            transform.rotation = Quaternion.Slerp(currentRot, initialRotation, t);
            yield return null;
        }
        // Asigură poziția finală și reia oscilația
        transform.rotation = initialRotation;
        isOscillating = true;
        activeCoroutine = null;
    }
}
