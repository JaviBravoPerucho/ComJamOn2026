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
        // Pasamos el texto del input al texto de la escena
        textoDestino.text += "\n"+campoEntrada.text;

        // Opcional: Limpiar la caja después de enviar
        campoEntrada.text = "";

        // Opcional: Volver a poner el foco en la caja para seguir escribiendo
        campoEntrada.ActivateInputField();

        siguienteSilaba.text = wordManager.SilabaActual;

        rangoLongitud.text = "(" + wordManager.MinLength + "-" + wordManager.MaxLength + ")";
    }
}