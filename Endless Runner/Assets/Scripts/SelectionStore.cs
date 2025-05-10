using UnityEngine;

public class SelectionStore : MonoBehaviour
{
    public float liftHeight = 2f;
    public float liftSpeed = 5f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private AudioSource audioSource;
    private static SelectionStore currentSelected; // Urmărește obiectul selectat curent
    public CharacterData characterData;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition; // Poziția inițială este ținta de bază
        audioSource = GetComponent<AudioSource>();
       // audioSource.clip = characterData.selectionSound;
    }

    void OnMouseDown()
    {
        // Dacă acesta este deja selectat, coboară-l
        if (currentSelected == this)
        {
            Lower();
            currentSelected = null;
        }
        else
        {
            // Coboară obiectul selectat anterior (dacă există)
            if (currentSelected != null)
            {
                currentSelected.Lower();
            }

            // Selectează acest obiect și ridică-l
            currentSelected = this;
            Lift();

            CharacterSelectionManager.Instance.SelectCharacter(characterData);
        }
    }

    void Update()
    {
        // Mișcă continuu către poziția țintă (indiferent de direcție)
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, liftSpeed * Time.deltaTime);
    }

    void Lift()
    {
        targetPosition = initialPosition + Vector3.up * liftHeight;
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    void Lower()
    {
        targetPosition = initialPosition;
    }
}
