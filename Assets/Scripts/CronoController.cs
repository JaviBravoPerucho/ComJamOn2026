using UnityEngine;
using UnityEngine.UI;

public class CronoController : MonoBehaviour
{
    public float maxTime = 60.0f;
    public float extraTime = 2f;
    public bool isRunning = true;
    // Si es true, el cronómetro se llenará a medida que avanza el tiempo. Si es false, se vaciará.
    public bool fill = true;

    private float actualTime = 0.0f;
    private Image crono_image;

    void Start()
    {
        crono_image = GetComponent<Image>();
        actualTime = 0.0f;
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
            actualTime = maxTime;
            GameManager.Instance.GameOver();
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
    }

    public void ResetCrono()
    {
        actualTime = 0.0f;
        isRunning = true;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void addTime()
    {
        actualTime -= extraTime;
    }
    public bool IsTimeUp()
    {
        return actualTime >= maxTime;
    }

    public float GetTimePorcentage()
    {
        return actualTime / maxTime;
    }
}
