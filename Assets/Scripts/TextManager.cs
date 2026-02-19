using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class TextManager : MonoBehaviour
{
    public TMP_InputField campoEntrada; // La caja donde escribes
    public TMP_Text textoDestino;      // Donde aparecerá la palabra
    public TMP_Text contadorLongitud;
    public TMP_Text siguienteSilaba;
    public TMP_Text rangoLongitud;


    private WordManager wordManager;

    [System.Serializable] // Esto es vital para que se vea en el Inspector
    public class ConfiguracionTexto
    {
        public string palabraClave;
        public Color colorAsociado;
    }

    // Tu array de pares [string, color]
    public ConfiguracionTexto[] misConfiguraciones = new ConfiguracionTexto[]
    {
        new ConfiguracionTexto { palabraClave = "#include ", colorAsociado = new Color(0f, 0.2f,0f,1f) },
        new ConfiguracionTexto { palabraClave = "#include ", colorAsociado = new Color(0f, 0.2f,0f,1f) },
        new ConfiguracionTexto { palabraClave = "\n"+"class ", colorAsociado = new Color(0f, 0f,0.2f,1f) },
        new ConfiguracionTexto { palabraClave = "\tint ", colorAsociado = new Color(0f, 0f,0.2f,1f) }
    };

    private int configIndex = 0;

    private void Start()
    {
        wordManager = GetComponent<WordManager>();

        campoEntrada.onValueChanged.AddListener(ActualizarContador);
        siguienteSilaba.text = wordManager.SilabaActual;
        rangoLongitud.text = "(" + wordManager.MinLength + "-" + wordManager.MaxLength + ")";
    }

    void Update()
    {
        // Detecta si presionas Enter o el Enter del teclado numérico
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(wordManager.TryWord(campoEntrada.text))EnviarTexto();
            else
            {
                // Opcional: Limpiar la caja después de enviar
                campoEntrada.text = "";

                // Opcional: Volver a poner el foco en la caja para seguir escribiendo
                campoEntrada.ActivateInputField();
            }
        }
    }

    public void ActualizarContador(string text)
    {
        contadorLongitud.text = text.Length.ToString();
    }

    public void EnviarTexto()
    {
        if (configIndex >= misConfiguraciones.Length) return;

        textoDestino.color = misConfiguraciones[configIndex].colorAsociado;
        textoDestino.text += misConfiguraciones[configIndex].palabraClave + campoEntrada.text + "\n";

        configIndex++;

        // Opcional: Limpiar la caja después de enviar
        campoEntrada.text = "";

        // Opcional: Volver a poner el foco en la caja para seguir escribiendo
        campoEntrada.ActivateInputField();

        siguienteSilaba.text = wordManager.SilabaActual;

        rangoLongitud.text = "(" + wordManager.MinLength + "-" + wordManager.MaxLength + ")";
    }
}