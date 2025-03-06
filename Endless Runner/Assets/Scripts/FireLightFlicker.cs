using UnityEngine;

public class FireLightFlicker : MonoBehaviour
{
    private Light fireLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        fireLight = GetComponent<Light>();
    }

    void Update()
    {
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0f));
    }
}
