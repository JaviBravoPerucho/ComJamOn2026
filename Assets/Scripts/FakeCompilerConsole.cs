using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FakeCompilerConsole : MonoBehaviour
{
    [SerializeField] private TMP_Text consoleText;
    [SerializeField] private float lineInterval = 0.08f;
    [SerializeField] private float compileDuration = 3f; // duración en segundos
    [SerializeField] private Dialogue dialogue;

    private Queue<string> lines = new Queue<string>();
    List<string> wordList;
    private float nota = 5.0f;

    public void StartCompilation()
    {
        StopAllCoroutines();
        StartCoroutine(CompileRoutine());
    }

    public void SetWords(HashSet<string> palabras, float notaExamen)
    {
        wordList = new List<string>(palabras);
        nota = notaExamen;
    }

    private IEnumerator CompileRoutine()
    {
        float startTime = Time.time;

        while (Time.time - startTime < compileDuration)
        {
            string nuevaLinea = GenerarLinea(wordList);

            lines.Enqueue(nuevaLinea);

            if (lines.Count > 6)
                lines.Dequeue();

            consoleText.text = string.Join("\n", lines);

            yield return new WaitForSeconds(lineInterval);
        }

        if (dialogue)
        {
            Debug.Log("Corrige");
            dialogue.LanzarDialogo(TipoDialogo.Corrigiendo);
            GameManager.Instance.GameOver();
        }

        // Al terminar simplemente salimos.
        // No limpiamos nada, así se quedan los últimos mensajes visibles.
    }

    private string GenerarLinea(List<string> palabras)
    {
        float errorChance = Mathf.Clamp01((5f - nota) / 5f);      // nota baja = más errores
        float warningChance = Mathf.Clamp01((8f - nota) / 8f);    // nota media = warnings

        float roll = Random.value;

        string palabra = palabras.Count > 0
            ? palabras[Random.Range(0, palabras.Count)]
            : "variable";

        if (nota >= 9.99f)
        {
            return $"<color=#AAAAAA>Compiling {palabra}...</color>";
        }

        if (roll < errorChance)
        {
            return $"<color=#FF4444>Error: Invalid use of '{palabra}'</color>";
        }
        else if (roll < errorChance + warningChance)
        {
            return $"<color=#FFD700>Warning: '{palabra}' might be uninitialized</color>";
        }
        else
        {
            return $"<color=#AAAAAA>Building {palabra}...</color>";
        }
    }
}