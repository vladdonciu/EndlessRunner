using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Characters/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public GameObject runnerPrefab;
    //public AudioClip selectionSound;
    public float baseSpeed = 5f;
    public float accelerationRate = 0.1f;
}
