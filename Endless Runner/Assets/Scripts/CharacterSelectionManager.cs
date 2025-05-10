using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;
    public CharacterData[] availableCharacters; // Array cu toate caracterele disponibile
    [HideInInspector] public CharacterData selectedCharacter;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Metoda nouă adăugată pentru selecție
    public void SelectCharacter(CharacterData data)
    {
        selectedCharacter = data;
    }
}
