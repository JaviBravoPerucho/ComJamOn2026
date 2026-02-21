using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CronoController : MonoBehaviour
{
    public float maxTime = 60.0f;
    public float extraTime = 2f;
    public bool isRunning = true;
    // Si es true, el cronómetro se llenará a medida que avanza el tiempo. Si es false, se vaciará.
    public bool fill = true;
    public WordManager wordManager;

    [SerializeField]
    private GameObject buttonCompile;

    [SerializeField]
    private GameObject extraSeconds;

    [SerializeField]
    private Transform transformSegundos;

    private float actualTime = 0.0f;
    private Image crono_image;
    private FMOD.Studio.EventInstance clockInstance;
    public EventReference tiempoAcabado;


    public FMOD.Studio.EventInstance getClockInstance() {return clockInstance;}

    void Start()
    {
        crono_image = GetComponent<Image>();
        actualTime = 0.0f;
        clockInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Clock");
    }

    void Update()
    {
        if (!isRunning)
        {
            return;
        }

        actualTime += Time.deltaTime;

        if (actualTime >= maxTime)
        {
            RuntimeManager.PlayOneShot(tiempoAcabado);
            StopCrono();
            GameManager.Instance.JuegoIniciado = false;
            GameManager.Instance.Puntuacion = wordManager.calcularNota();
            
            if(buttonCompile)
                buttonCompile.SetActive(true);
            //GameManager.Instance.GameOver();
        }

        if (fill)
        {
            crono_image.fillAmount = actualTime / maxTime;
        }
        else
        {
            crono_image.fillAmount = 1 - (actualTime / maxTime);
        }
    }

    public void StopCrono()
    {
        isRunning = false;
        clockInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void ResetCrono()
    {
        actualTime = 0.0f;
        isRunning = true;


        clockInstance.start();
        clockInstance.release();
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void addTime()
    {

        actualTime -= extraTime;
        if(actualTime <= 0)
        {
            actualTime = 0;
        }

        //GameObject text = Instantiate(extraSeconds, transformSegundos);

        //text.GetComponent<TMP_Text>().text = "+" + extraSeconds.ToString() + "segundos";

       // Destroy(text, 2f);
    }
    public bool IsTimeUp()
    {
        return actualTime >= maxTime;
    }

    public float GetTimePorcentage()
    {
        // Ahora empieza en 0 y termina en 1.
        return (actualTime)/ maxTime;
    }

    public float GetRemainingTimePercentage()
    {
        return (maxTime- actualTime)/ actualTime;
    }
}
