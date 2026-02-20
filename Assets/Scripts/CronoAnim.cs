using UnityEngine;
using UnityEngine.UI;

public class CronoAnim : MonoBehaviour
{
    public float startPulsePercentage = 0.7f; // Porcentaje del tiempo en el que comienza a pulsar
    public float pulseSpeed = 3.0f; // Velocidad del pulso
    private Image cronoImage;
    private CronoController cronoController;
    private Vector3 originalScale;
    void Start()
    {
        cronoImage = GetComponent<Image>();
        cronoController = GetComponent<CronoController>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!cronoController.IsTimeUp())
        {
            // El cronómetro pulse o cambie de color
            cronoImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));

            if (cronoController.GetTimePorcentage() >= startPulsePercentage)
            {
                // velocidad del pulso aumenta a medida que se acerca al final
                float inc = cronoController.GetTimePorcentage() - startPulsePercentage;
                transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.2f, Mathf.PingPong(Time.time * inc * pulseSpeed, 1));
            }
        }
    }
}
