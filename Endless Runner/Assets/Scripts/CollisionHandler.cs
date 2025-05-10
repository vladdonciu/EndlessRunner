using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] AudioClip collisionSound;
    [SerializeField] GameObject fadeOutObject;

    [Header("Animation Parameters")]
    [SerializeField] string stumbleAnimTrigger = "Stumble";
    [SerializeField] string cameraAnimation = "CollisionCam";

    [Header("Timing")]
    [SerializeField] float deathDelay = 3f;
    [SerializeField] float fadeDelay = 3f;

    // Referințe componente
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    private DistanceCounter distanceCounter;
    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isDead = false;

    void Awake()
    {
        InitializeComponents();
        CacheReferences();
    }

    void InitializeComponents()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponentInChildren<Animator>(true);
        distanceCounter = GetComponent<DistanceCounter>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        if (!audioSource) audioSource = gameObject.AddComponent<AudioSource>();
        if (collisionSound) audioSource.clip = collisionSound;
    }

    void CacheReferences()
    {
        if (UI_Manager.Instance && !fadeOutObject)
            fadeOutObject = UI_Manager.Instance.fadeOut;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Obstacle"))
            StartCoroutine(ProcessCollision());
    }

    IEnumerator ProcessCollision()
    {
        isDead = true;
        StopPlayerMovement();
        StopDistanceCounting();
        PlayDeathEffects();

        yield return new WaitForSeconds(deathDelay);
        ActivateFadeEffect();

        yield return new WaitForSeconds(fadeDelay);
        ResetGameState();
    }

    void StopPlayerMovement()
    {
        // Oprire imediată a fizicii
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        // Dezactivează complet mișcarea scriptată
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            playerMovement.enabled = false;
            playerMovement.CurrentPlayerSpeed = 0f; // Forțează resetarea vitezei
        }
    }

    void StopDistanceCounting()
    {
        if (distanceCounter != null)
            distanceCounter.StopCounting();
    }

    void PlayDeathEffects()
    {
        PlayDeathAnimation();
        PlayCollisionSound();
        PlayCameraAnimation();
    }

    void PlayDeathAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger(stumbleAnimTrigger);
            playerAnimator.SetBool("Running", false);
            playerAnimator.SetTrigger(stumbleAnimTrigger);

            if (playerAnimator.HasState(0, Animator.StringToHash("Stumble Backwards")))
                playerAnimator.Play("Stumble Backwards", 0, 0f);
        }
    }

    void PlayCollisionSound()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();
    }

    void PlayCameraAnimation()
    {
        if (Camera.main.TryGetComponent<Animator>(out var camAnimator))
            camAnimator.Play(cameraAnimation, 0, 0f);
    }

    void ActivateFadeEffect()
    {
        if (fadeOutObject != null)
            fadeOutObject.SetActive(true);
        else if (UI_Manager.Instance != null && UI_Manager.Instance.fadeOut != null)
            UI_Manager.Instance.fadeOut.SetActive(true);
    }

    void ResetGameState()
    {
        Time.timeScale = 1f;
        UI_Manager.Instance?.ResetUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
