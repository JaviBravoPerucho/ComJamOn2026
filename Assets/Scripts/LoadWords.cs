using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class LoadWords : MonoBehaviour
{
    public static LoadWords Instance { get; private set; }

    private HashSet<string> palabras;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        palabras = new HashSet<string>(700000);
        CargarPalabras();
    }

    void CargarPalabras()
    {
        TextAsset[] archivos = Resources.LoadAll<TextAsset>("Words");

        foreach (TextAsset archivo in archivos)
        {
            string[] lineas = archivo.text.Split('\n');

            foreach (string linea in lineas)
            {
                ProcesarLinea(linea.Trim().ToLower());
            }
        }

        Debug.Log("Palabras cargadas: " + palabras.Count);
    }

    void ProcesarLinea(string linea)
    {
        if (string.IsNullOrWhiteSpace(linea))
            return;

        // Quitar números al final (cocha1 → cocha)
        string limpia = QuitarNumerosFinal(linea);

        if (!limpia.Contains(","))
        {
            palabras.Add(limpia);
            return;
        }

        // Si tiene coma (ej: cochambroso, sa)
        string[] partes = limpia.Split(',');

        string baseWord = partes[0].Trim();
        palabras.Add(baseWord);

        for (int i = 1; i < partes.Length; i++)
        {
            string sufijo = partes[i].Trim();

            if (string.IsNullOrEmpty(sufijo))
                continue;

            string variante = GenerarVariante(baseWord, sufijo);
            palabras.Add(variante);
        }
    }

    string QuitarNumerosFinal(string palabra)
    {
        return Regex.Replace(palabra, @"\d+$", "");
    }

    string GenerarVariante(string baseWord, string sufijo)
    {
        int letrasAReemplazar = sufijo.Length;

        if (baseWord.Length >= letrasAReemplazar)
        {
            string raiz = baseWord.Substring(0, baseWord.Length - letrasAReemplazar);
            return raiz + sufijo;
        }

        return baseWord + sufijo;
    }

    public bool Existe(string palabra)
    {
        return palabras.Contains(palabra.ToLower());
    }
}
