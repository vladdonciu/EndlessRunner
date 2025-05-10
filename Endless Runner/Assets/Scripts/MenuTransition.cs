using UnityEngine;
using DG.Tweening;

public class MenuTransition : MonoBehaviour
{
    [Header("Referințe")]
    public CameraOscillateY cameraOscillate;
    public RectTransform menuPanel;

    [Header("Setări")]
    public float rotationDuration = 1.5f;
    public float menuMoveDuration = 0.8f;
    public float targetYRotation = 112f;
    public float slideDistance = 1000f; // Cât de mult să alunece meniul (în pixeli)

    private Vector2 initialPosition; // Salvează poziția inițială

    private void Start()
    {
        initialPosition = menuPanel.anchoredPosition;
    }

    public void StartStoreTransition()
    {
        // Oprește oscilația și rotește camera
        cameraOscillate.RotateToTarget(targetYRotation);

        // Animație de slide la stânga și dezactivare UI
        menuPanel.DOAnchorPos(initialPosition + Vector2.right * slideDistance, menuMoveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                menuPanel.gameObject.SetActive(false);
            });
    }

    public void ReturnToMenu()
    {
        // Activează UI-ul și poziționează în dreapta
        menuPanel.gameObject.SetActive(true);
        menuPanel.anchoredPosition = initialPosition + Vector2.right * slideDistance;

        // Slide din dreapta spre poziția inițială
        menuPanel.DOAnchorPos(initialPosition, menuMoveDuration)
            .SetEase(Ease.OutBack);

        // Revine la oscilația camerei
        cameraOscillate.ResetRotation();
    }
}
