using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ScrollBarController : MonoBehaviour
{
    private Scrollbar scrollbar;

    [SerializeField] private RectTransform rectTexto;
    [SerializeField] private RectTransform rectLine;

    [SerializeField] public float distanciaRecorrido = 500f;

    private Vector2 posicionInicial;
    private Vector2 posicionInicialtexto;

    public float lastValue = 0;

    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();

        if (rectLine != null)
        {
            posicionInicial = rectLine.anchoredPosition;
        }

        if (rectTexto != null)
        {
            posicionInicialtexto = rectTexto.anchoredPosition;
        }

        // Suscribimos el método al evento de cambio de valor
        scrollbar.onValueChanged.AddListener(OnScrollChanged);

        // Inicializamos la posición
        OnScrollChanged(scrollbar.value);
    }

    public void OnScrollChanged(float value)
    {
        if (!rectTexto && !rectLine) return;

        lastValue = value;
        scrollbar.value = value;

        float offset = value * distanciaRecorrido;

        //rectTexto.anchoredPosition = new Vector2(
        //    posicionInicialtexto.x,
        //    posicionInicialtexto.y + offset
        //);

        rectLine.anchoredPosition = new Vector2(
            posicionInicial.x,
            posicionInicial.y + offset
        );
    }

    private void OnDestroy()
    {
        // Buena práctica: limpiar el listener al destruir el objeto
        if (scrollbar != null)
            scrollbar.onValueChanged.RemoveListener(OnScrollChanged);
    }
}