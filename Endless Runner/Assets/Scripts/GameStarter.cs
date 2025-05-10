using UnityEngine;
using TMPro;
using System.Collections;

public class GameStarter : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Animator playerAnimator;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => UI_Manager.Instance != null);

        UI_Manager.Instance.ResetUI();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerAnimator = playerMovement?.GetComponentInChildren<Animator>(true);

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.canMove = false;
        }

        if (playerAnimator != null)
        {
            playerAnimator.Rebind();
            playerAnimator.Play("Breathing Idle");
        }

        StartCoroutine(AnimateStartText());
    }

    IEnumerator AnimateStartText()
    {
        while (UI_Manager.Instance.startText != null && UI_Manager.Instance.startText.gameObject.activeSelf)
        {
            UI_Manager.Instance.startText.alpha = Mathf.PingPong(Time.time, 1);
            yield return null;
        }
    }

    void Update()
    {
        if (UI_Manager.Instance != null &&
            UI_Manager.Instance.startText != null &&
            UI_Manager.Instance.startText.gameObject.activeSelf &&
            Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }



    void StartGame()
    {
        UI_Manager.Instance.startText.gameObject.SetActive(false);
        if (playerMovement != null) playerMovement.canMove = true;
        if (playerAnimator != null) playerAnimator.SetBool("Running", true);
    }
}
