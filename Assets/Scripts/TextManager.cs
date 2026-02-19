using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class TextManager : MonoBehaviour
{
    public TMP_InputField campoEntrada; // La caja donde escribes
    public TMP_Text textoDestino;      // Donde aparecerá la palabra

    private WordManager wordManager;

    private void Start()
    {
        wordManager = GetComponent<WordManager>();
    }

    void Update()
    {
        // Detecta si presionas Enter o el Enter del teclado numérico
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && wordManager.TryWord(campoEntrada.text))
        {
            EnviarTexto();
        }
    }

    public void EnviarTexto()
    {
        // Pasamos el texto del input al texto de la escena
        textoDestino.text += "\n"+campoEntrada.text;

        // Opcional: Limpiar la caja después de enviar
        campoEntrada.text = "";

        // Opcional: Volver a poner el foco en la caja para seguir escribiendo
        campoEntrada.ActivateInputField();
    }
}