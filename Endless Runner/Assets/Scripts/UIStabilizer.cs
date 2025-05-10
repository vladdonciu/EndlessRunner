using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIStabilizer : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.position;
        initialRotation = rectTransform.rotation;
        initialScale = rectTransform.localScale;
    }

    void LateUpdate()
    {
        rectTransform.SetPositionAndRotation(initialPosition, initialRotation);
        rectTransform.localScale = initialScale;
    }
}
