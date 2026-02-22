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

    private float pulseTimer = 0f;

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

            if (porcentajeActual >= startPulsePercentage)
            {
                float inc = porcentajeActual - startPulsePercentage;

                float velocidadActual = pulseSpeed * (1 + (inc * 10));

                cronoController.getClockInstance()
                    .setParameterByName("pitchClock", Mathf.Clamp01(velocidadActual));

                //  Acumulamos tiempo manualmente
                pulseTimer += Time.deltaTime * velocidadActual;

                float pingPongValue = Mathf.PingPong(pulseTimer, 1f);

                // Color
                cronoImage.color = Color.Lerp(Color.white, Color.red, pingPongValue);

                // Escala
                transform.localScale = Vector3.Lerp(originalScale, originalScale * 1.2f, pingPongValue);
            }
            else
            {
                cronoImage.color = Color.white;
                transform.localScale = originalScale;

                // Opcional: reiniciar el timer
                pulseTimer = 0f;
            }
        }
    }
}
