using UnityEngine;
using TMPro;
using System.Collections;

// 1. Defines los tipos de comportamiento/eventos
public enum TipoDialogo
{
    Preparate,
    Bien,
    PalabraRepetida,
    PalabraFueraRango,
    PalabraSilabaIncorrecta,
    PalabraMal,
    SeAcaboExamen,
    BuenaNota,
    NotaMedia,
    NotaMala
}

// 2. Agrupas el enum con las posibles líneas que puede decir
[System.Serializable]
public class ConfiguracionDialogo
{
    public TipoDialogo tipo;
    [TextArea(2, 4)]
    public string[] posiblesLineas; // Array de opciones para elegir una al azar
}

public class Dialogue : MonoBehaviour
{
    [Header("Configuración de Diálogos")]
    // Este array aparecerá en el Inspector donde enlazarás el enum con los textos
    [SerializeField] private ConfiguracionDialogo[] listaDialogos;

    [Header("Referencias UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Ajustes de Texto")]
    public float timeToLetter = 0.05f;

    // Guardamos la corrutina actual para poder detenerla si es necesario
    private Coroutine corrutinaEscritura;

    void Start()
    {
        // Ejemplo de uso al inicio:
        // LanzarDialogo(TipoDialogo.Saludo);
    }

    // 3. Método público para llamar al diálogo pasándole el enum
    public void LanzarDialogo(TipoDialogo tipoDeseado)
    {
        // Buscamos la configuración que coincida con el enum solicitado
        ConfiguracionDialogo configEncontrada = null;
        foreach (var config in listaDialogos)
        {
            if (config.tipo == tipoDeseado)
            {
                configEncontrada = config;
                break;
            }
        }

        // Si no hay configuración o no hay líneas escritas, salimos
        if (configEncontrada == null || configEncontrada.posiblesLineas.Length == 0)
        {
            Debug.LogWarning($"No se encontraron diálogos para el tipo: {tipoDeseado}");
            return;
        }

        // Elegimos una línea aleatoria del array
        int indiceRandom = Random.Range(0, configEncontrada.posiblesLineas.Length);
        string lineaElegida = configEncontrada.posiblesLineas[indiceRandom];

        // VITAL: Si ya hay un texto escribiéndose, lo detenemos de golpe
        if (corrutinaEscritura != null)
        {
            StopCoroutine(corrutinaEscritura);
        }

        // Nos aseguramos de que el panel esté visible
        dialoguePanel.SetActive(true);

        // Iniciamos la nueva escritura y guardamos la referencia
        corrutinaEscritura = StartCoroutine(ShowLine(lineaElegida));
    }

    private IEnumerator ShowLine(string linea)
    {
        dialogueText.text = string.Empty;

        foreach (char c in linea)
        {
            dialogueText.text += c;
            // Usamos la variable timeToLetter que ya tenías
            yield return new WaitForSeconds(timeToLetter);
        }

        // Opcional: Cuando termina de escribir, limpiamos la referencia
        corrutinaEscritura = null;
    }
}