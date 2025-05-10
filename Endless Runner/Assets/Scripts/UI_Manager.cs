using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [Header("UI References")]
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI startText;
    public GameObject fadeOut;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindUIReferences();
        ResetUI();
    }

    void FindUIReferences()
    {
        // Folosește Resources pentru a găsi obiecte inactive
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.CompareTag("FadeOut")) fadeOut = obj;
            if (obj.CompareTag("DistanceText")) distanceText = obj.GetComponent<TextMeshProUGUI>();
            if (obj.CompareTag("StartText")) startText = obj.GetComponent<TextMeshProUGUI>();
        }
    }


    public void ResetUI()
    {
        if (fadeOut != null) fadeOut.SetActive(false);
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
            startText.alpha = 1f;
        }
        if (distanceText != null) distanceText.text = "Distance: 0 m";
    }
}
