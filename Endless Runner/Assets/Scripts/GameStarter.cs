using UnityEngine;
using TMPro;
using System.Collections;

public class GameStarter : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator playerAnimator;
    public TextMeshProUGUI startText;
    public DistanceCounter distanceCounter; // Adăugat pentru a putea controla numărătoarea distanței

    private bool gameStarted = false;

    void Start()
    {
        // Asigură-te că jucătorul nu se mișcă la început
        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Păstrăm scriptul activ
            playerMovement.canMove = false; // Dar dezactivăm mișcarea
        }
        else
        {
            Debug.LogError("PlayerMovement reference is missing!");
        }

        // Verifică și afișează textul de start
        if (startText != null)
        {
            startText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Start text reference is missing!");
        }

        // Setează animația de idle
        if (playerAnimator != null)
        {
            playerAnimator.Play("Breathing Idle");
            // Asigură-te că parametrul Running este setat la false la început
            playerAnimator.SetBool("Running", false);
        }
        else
        {
            Debug.LogError("Player Animator reference is missing!");
        }

        // Dezactivează numărătoarea distanței la început
        if (distanceCounter != null)
        {
            distanceCounter.enabled = false;
        }

        StartCoroutine(AnimateStartText());
    }

    void Update()
    {
        // Verifică dacă jocul nu a început și dacă jucătorul a dat click
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;

        // Ascunde textul de start
        if (startText != null)
        {
            startText.gameObject.SetActive(false);
        }

        // Activează mișcarea jucătorului
        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }

        // Activează numărătoarea distanței
        if (distanceCounter != null)
        {
            distanceCounter.enabled = true;
        }

        // Declanșează animația de alergare
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("Running", true);
            Debug.Log("Set Running parameter to true"); // Debug pentru a verifica
        }
    }

    IEnumerator AnimateStartText()
    {
        while (!gameStarted)
        {
            startText.alpha = Mathf.PingPong(Time.time, 1);
            yield return null;
        }
    }

}

