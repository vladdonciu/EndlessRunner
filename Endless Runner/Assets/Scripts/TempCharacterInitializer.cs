using UnityEngine;

public class TempCharacterInitializer : MonoBehaviour
{
    void Start()
    {
        if (CharacterSelectionManager.Instance == null || CharacterSelectionManager.Instance.selectedCharacter == null)
        {
            GetComponent<PlayerMovement>().canMove = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
