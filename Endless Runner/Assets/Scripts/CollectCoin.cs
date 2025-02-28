using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] AudioSource coinFX;


    private void OnTriggerEnter(Collider other)
    {
        coinFX.Play();
        MasterInfo.coinCount += 1;
        this.gameObject.SetActive(false);
    }

}
