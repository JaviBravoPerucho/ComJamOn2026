using UnityEngine;
using TMPro; // Importante para usar TextMeshPro

public class TextManager : MonoBehaviour
{
    public TMP_InputField campoEntrada; // La caja donde escribes
    public TMP_Text textoDestino;      // Donde aparecerá la palabra

    void Update()
    {
        // Detecta si presionas Enter o el Enter del teclado numérico
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EnviarTexto();
        }
    }

    public void EnviarTexto()
    {
        // Pasamos el texto del input al texto de la escena
        textoDestino.text = campoEntrada.text;

        // Opcional: Limpiar la caja después de enviar
        campoEntrada.text = "";

        // Opcional: Volver a poner el foco en la caja para seguir escribiendo
        campoEntrada.ActivateInputField();
    }
}