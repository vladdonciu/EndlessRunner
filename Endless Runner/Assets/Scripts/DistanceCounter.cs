using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    public static DistanceCounter instance;
    public float TotalDistance { get; private set; } = 0f;

    [Header("Settings")]
    public float updateInterval = 0.1f;

    private TextMeshProUGUI distanceText;
    private Vector3 previousPosition;
    private float displayedDistance = 0f;
    private float timer = 0f;
    private bool isCounting = true;
    private float totalDistance = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        previousPosition = transform.position;
        distanceText = UI_Manager.Instance.distanceText;

        if (distanceText == null)
        {
            Debug.LogError("Distance text reference missing!");
            enabled = false;
        }
    }

    void Update()
    {
        if (!isCounting) return;

        float frameDistance = Vector3.Distance(transform.position, previousPosition);
        totalDistance += frameDistance;
        previousPosition = transform.position;

        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            displayedDistance = Mathf.Lerp(displayedDistance, totalDistance, 0.5f);
            UpdateUIText();
        }
    }

    void UpdateUIText()
    {
        if (totalDistance < 1000f)
        {
            distanceText.text = $"Distance: {Mathf.RoundToInt(displayedDistance)}m";
        }
        else
        {
            distanceText.text = $"Distance: {(displayedDistance / 1000f):0.00}km";
        }
    }

    public void StopCounting() => isCounting = false;
    public void StartCounting() => isCounting = true;
}
