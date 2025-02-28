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


    void OnTriggerEnter(Collider other)
    {
        //collisionFX.Play();
        //thePlayer.GetComponent<PlayerMovement>().enabled = false;
        //playerAnim.GetComponent<Animator>().Play("Stumble Backwards");

        StartCoroutine(CollisionEnd());
    }

    IEnumerator CollisionEnd()
    {
        collisionFX.Play();
        thePlayer.GetComponent<PlayerMovement>().enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        mainCam.GetComponent<Animator>().Play("CollisionCam");

        yield return new WaitForSeconds(3);
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
             


    }
}
