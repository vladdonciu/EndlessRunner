using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    public static DistanceCounter instance;

    [Header("UI Components")]
    public TextMeshProUGUI distanceText;

    [Header("Counting Settings")]
    public float updateInterval = 0.1f;

    private Vector3 previousPosition;
    private float displayedDistance = 0f;
    private float timer = 0f;
    private bool isCounting = true; // Flag pentru a verifica dacă numărătoarea este activă

    // Proprietate publică pentru distanța totală
    public float TotalDistance { get; private set; } = 0f;

    // Referință la PlayerMovement
    private PlayerMovement playerMovement;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        previousPosition = transform.position;
        displayedDistance = 0f;
        TotalDistance = 0f;

        // Obține referința la PlayerMovement
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            // Folosește metoda recomandată în loc de FindObjectOfType
            playerMovement = FindAnyObjectByType<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("Nu s-a găsit niciun component PlayerMovement în scenă!");
            }
        }
    }

    void Update()
    {
        // Verifică dacă numărătoarea este activă
        if (!isCounting)
            return;

        // Calculează distanța reală parcursă în acest frame
        float distanceTraveled = Vector3.Distance(transform.position, previousPosition);
        TotalDistance += distanceTraveled;
        previousPosition = transform.position;

        // Actualizează numărătoarea la intervale regulate
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;

            // Obține viteza curentă a jucătorului pentru o actualizare proporțională
            float speedFactor = 1f;
            if (playerMovement != null)
            {
                speedFactor = playerMovement.CurrentPlayerSpeed * 0.1f;
            }

            // Actualizează afișarea distanței treptat
            displayedDistance = Mathf.MoveTowards(displayedDistance, TotalDistance, speedFactor);

            UpdateDistanceText();
        }
    }

    void UpdateDistanceText()
    {
        if (displayedDistance < 10000f)
        {
            distanceText.text = "Distance: " + Mathf.RoundToInt(displayedDistance).ToString() + " m";
        }
        else
        {
            float kilometers = displayedDistance / 1000f;
            distanceText.text = "Distance: " + kilometers.ToString("F1") + " km";
        }
    }

    // Metodă pentru a opri numărătoarea
    public void StopCounting()
    {
        isCounting = false;
    }

    // Metodă pentru a reporni numărătoarea (dacă este necesar)
    public void StartCounting()
    {
        isCounting = true;
    }
}
