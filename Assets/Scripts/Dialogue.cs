using UnityEngine;
using TMPro;
using System.Collections;
using FMODUnity;
using UnityEngine.UI;

// 1. Defines los tipos de comportamiento/eventos
public enum TipoDialogo
{
    Preparate,
    Comenzar,
    Bien,
    PalabraRepetida,
    PalabraFueraRango,
    PalabraSilabaIncorrecta,
    PalabraMal,
    SeAcaboExamen,
    Compilar,
    Corrigiendo,
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

    [Header("Avatar / Frames")]
    [SerializeField] private Image characterImage;

    [SerializeField] private Sprite frame1;
    [SerializeField] private Sprite frame2;
    [SerializeField] private Sprite frame3;

    [SerializeField] private float flipDuration = 0.2f;

    private bool flipped;
    private bool isFlipping;

    // Guardamos la corrutina actual para poder detenerla si es necesario
    private Coroutine corrutinaEscritura;

    public EventReference letterEvent;

    void Start()
    {
        // Ejemplo de uso al inicio:
        LanzarDialogo(TipoDialogo.Preparate);
    }

    public void EmpezarExamen()
    {
        LanzarDialogo(TipoDialogo.Comenzar);
    }

    public void CompilarExamen()
    {
        LanzarDialogo(TipoDialogo.Compilar);
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

        CambiarFrame(tipoDeseado);

        // Iniciamos la nueva escritura y guardamos la referencia
        corrutinaEscritura = StartCoroutine(ShowLine(lineaElegida));
    }

    private void CambiarFrame(TipoDialogo tipo)
    {
        if (isFlipping) return;

        Sprite nuevoSprite = ObtenerSpriteSegunTipo(tipo);
        StartCoroutine(FlipAnimation(nuevoSprite));
    }

    private Sprite ObtenerSpriteSegunTipo(TipoDialogo tipo)
    {
        switch (tipo)
        {
            case TipoDialogo.Bien:
            case TipoDialogo.BuenaNota:
                return frame1;

            case TipoDialogo.NotaMedia:
            case TipoDialogo.Compilar:
            case TipoDialogo.Comenzar:
            case TipoDialogo.Preparate:
                return frame2;

            default:
                return frame3;
        }
    }

    private IEnumerator FlipAnimation(Sprite nuevoSprite)
    {
        isFlipping = true;

        float halfDuration = flipDuration / 2f;

        Quaternion startRot = characterImage.rectTransform.localRotation;
        Quaternion midRot = Quaternion.Euler(0, 90f, 0);
        Quaternion endRot = Quaternion.Euler(0, flipped ? 0f : 180f, 0);

        float t = 0;

        // Primera mitad (0  90)
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerp = t / halfDuration;
            characterImage.rectTransform.localRotation =
                Quaternion.Lerp(startRot, midRot, lerp);
            yield return null;
        }

        // Cambiamos sprite en el punto medio
        characterImage.sprite = nuevoSprite;

        t = 0;

        // Segunda mitad (90  180 o 0)
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            float lerp = t / halfDuration;
            characterImage.rectTransform.localRotation =
                Quaternion.Lerp(midRot, endRot, lerp);
            yield return null;
        }

        flipped = !flipped;
        isFlipping = false;
    }

    private IEnumerator ShowLine(string linea)
    {
        dialogueText.text = string.Empty;

        foreach (char c in linea)
        {
            dialogueText.text += c;
            // Usamos la variable timeToLetter que ya tenías
            RuntimeManager.PlayOneShot(letterEvent);
            yield return new WaitForSeconds(timeToLetter);
        }

        // Opcional: Cuando termina de escribir, limpiamos la referencia
        corrutinaEscritura = null;
    }
}