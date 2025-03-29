using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionDetect : MonoBehaviour
{
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject fadeOut;

    // Variabilă pentru a ține evidența dacă coliziunea a fost deja detectată
    private bool collisionDetected = false;

    void OnTriggerEnter(Collider other)
    {
        if (!collisionDetected)
        {
            collisionDetected = true;
            StartCoroutine(CollisionEnd());
        }
    }

    IEnumerator CollisionEnd()
    {
        // Oprește sunetul de coliziune
        collisionFX.Play();

        // Dezactivează controlul jucătorului
        thePlayer.GetComponent<PlayerMovement>().enabled = false;

        // Oprește numărătoarea distanței
        DistanceCounter distanceCounter = thePlayer.GetComponent<DistanceCounter>();
        if (distanceCounter != null)
        {
            distanceCounter.StopCounting();
        }

        // Redă animația de cădere
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");

        // Activează camera de coliziune
        mainCam.GetComponent<Animator>().Play("CollisionCam");

        // Așteaptă 3 secunde
        yield return new WaitForSeconds(3);

        // Activează efectul de fade out
        fadeOut.SetActive(true);

        // Așteaptă încă 3 secunde
        yield return new WaitForSeconds(3);

        // Încarcă scena inițială
        SceneManager.LoadScene(0);
    }
}
