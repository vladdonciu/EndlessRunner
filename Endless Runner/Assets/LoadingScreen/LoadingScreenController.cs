using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class LoadingScreenController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoRawImage; // GameObject cu RawImage și CanvasGroup
    public CanvasGroup videoCanvasGroup; // Referință la CanvasGroup-ul de pe RawImage
    public string sceneToLoad = "MainMenu";
    public float initialDelay = 5f; // Delay înainte de startul videoclipului
    public float fadeDuration = 1.5f; // Durata fade in-ului

    void Start()
    {
        videoRawImage.SetActive(false); // Ascunde RawImage-ul la început
        videoCanvasGroup.alpha = 0f;    // Asigură-te că este complet transparent
        videoPlayer.Stop();             // Asigură-te că video-ul nu rulează
        StartCoroutine(LoadingFlow());
    }

    IEnumerator LoadingFlow()
    {
        // Începe încărcarea scenei Main Menu în fundal
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // Așteaptă delay-ul inițial
        yield return new WaitForSeconds(initialDelay);

        // Activează RawImage-ul și pregătește videoclipul
        videoRawImage.SetActive(true);

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();

        // Fade in la video
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            videoCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        videoCanvasGroup.alpha = 1f; // Asigură-te că este complet opac

        // Așteaptă terminarea videoclipului
        while (videoPlayer.isPlaying)
            yield return null;

        // Activează scena principală
        asyncLoad.allowSceneActivation = true;
    }
}
