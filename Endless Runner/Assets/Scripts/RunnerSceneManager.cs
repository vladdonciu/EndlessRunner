using UnityEngine;

public class RunnerSceneManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject defaultCharacter;

    void Start()
    {
        if (CharacterSelectionManager.Instance == null || CharacterSelectionManager.Instance.selectedCharacter == null)
        {
            Instantiate(defaultCharacter, spawnPoint.position, Quaternion.identity);
            return;
        }
        InstantiateSelectedCharacter();
    }

    void InstantiateSelectedCharacter()
    {
        // Instanțiază și activează explicit obiectul
        GameObject newPlayer = Instantiate(
            CharacterSelectionManager.Instance.selectedCharacter.runnerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
        newPlayer.SetActive(true); // Activează explicit

        // Gestionare camere
        Camera[] allCameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (Camera cam in allCameras)
        {
            if (cam.gameObject != newPlayer)
                cam.gameObject.SetActive(false);
        }

        // Forțează actualizarea camerei principale
        Camera playerCam = newPlayer.GetComponentInChildren<Camera>(true);
        if (playerCam != null)
        {
            playerCam.gameObject.SetActive(true);
            playerCam.tag = "MainCamera";
            playerCam.enabled = true;
        }
        else
        {
            Debug.LogError("Camera lipsește din prefab!");
        }
    }


    void ConfigureMovement(GameObject player)
    {
        // Corectare 4: Adaugă verificare pentru instanța managerului
        if (CharacterSelectionManager.Instance != null &&
            CharacterSelectionManager.Instance.selectedCharacter != null)
        {
            PlayerMovement movement = player.AddComponent<PlayerMovement>();
            movement.canMove = true;
            movement.initialPlayerSpeed = CharacterSelectionManager.Instance.selectedCharacter.baseSpeed;
        }
    }
}
