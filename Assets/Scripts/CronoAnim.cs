using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class CronoAnim : MonoBehaviour
{
    public float startPulsePercentage = 0.7f; // Arranca cuando ha pasado el 70% del tiempo
    public float pulseSpeed = 3.0f;

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
            float porcentajeActual = cronoController.GetTimePorcentage();

            // Si ya pasamos el porcentaje de inicio (ej: 0.7 o 70% del tiempo total)
            if (porcentajeActual >= startPulsePercentage)
            {
                // 1. Calculamos la intensidad/velocidad basándonos en cuánto nos acercamos a 1.0
                // 'inc' irá desde 0.0 (en 0.7) hasta 0.3 (en 1.0)
                float inc = porcentajeActual - startPulsePercentage;

                // Multiplicamos la velocidad base para que se note la aceleración
                float velocidadActual = pulseSpeed * (1 + (inc * 10));
                cronoController.getClockInstance().setParameterByName("pitchClock", Mathf.Clamp01(velocidadActual));

                // 2. Parpadeo de color (ahora está dentro del if)
                cronoImage.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * velocidadActual, 1));

                // 3. Efecto de latido (Scale)
                transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.2f, Mathf.PingPong(Time.time * velocidadActual, 1));
            }
            else
            {
                // Restauramos si el tiempo es menor a 0.7 (por si recupera tiempo con extraTime)
                cronoImage.color = Color.white;
                transform.localScale = originalScale;
            }
        }
    }
}
