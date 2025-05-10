using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [Header("Referințe")]
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        ConfigureCameras();
    }

    void ConfigureCameras()
    {
        // Configurare cameră UI
        uiCamera.depth = 1;
        uiCamera.cullingMask = LayerMask.GetMask("UI");

        // Configurare cameră principală
        mainCamera.depth = 0;
        mainCamera.cullingMask = ~LayerMask.GetMask("UI");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
