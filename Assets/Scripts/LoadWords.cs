using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class LoadWords : MonoBehaviour
{
    public static LoadWords Instance { get; private set; }

    private HashSet<string> palabras;

    string consonantes = "bcdfglmnprstvz";
    string vocales = "aeiou";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        palabras = new HashSet<string>(220000);
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
    public string QuitarTildes(string texto)
    {
        // 1. Normalizamos el texto (separa la letra del acento)
        string textoNormalizado = texto.Normalize(NormalizationForm.FormD);

        StringBuilder sb = new StringBuilder();

        foreach (char c in textoNormalizado)
        {
            // 2. Comprobamos si el carácter es una categoría de "marca" (como un acento)
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);

            // 3. Si NO es un acento, lo añadimos al resultado
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        // 4. Devolvemos el texto ya limpio
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    void ProcesarLinea(string linea)
    {
        if (string.IsNullOrWhiteSpace(linea))
            return;

        // Quitar números al final (cocha1 → cocha)
        string limpia = QuitarNumerosFinal(QuitarTildes(linea));

        if (!limpia.Contains(","))
        {
            if (vocales.Contains(limpia[limpia.Length - 1]))
            {
                string pluralVocal = limpia + "s";
                palabras.Add(pluralVocal);
            }
            else if (consonantes.Contains(limpia[limpia.Length - 1]))
            {
                string pluralConsonante = limpia + "es";
                palabras.Add(pluralConsonante);
            }
            palabras.Add(limpia);
            return;
        }

        // Si tiene coma (ej: cochambroso, sa)
        string[] partes = limpia.Split(',');

        string baseWord = partes[0].Trim();
        palabras.Add(baseWord);

        if (vocales.Contains(baseWord[baseWord.Length - 1])){
            string pluralVocal = baseWord + "s";
            palabras.Add(pluralVocal);
        }
        else if(consonantes.Contains(baseWord[baseWord.Length - 1]))
        {
            string pluralConsonante = baseWord + "es";
            palabras.Add(pluralConsonante);
        }

        for (int i = 1; i < partes.Length; i++)
        {
            string sufijo = partes[i].Trim();

            if (string.IsNullOrEmpty(sufijo))
                continue;

            string variante = GenerarVariante(baseWord, sufijo);
            palabras.Add(variante);

            if (vocales.Contains(variante[variante.Length - 1]))
            {
                string pluralVocal = variante + "s";
                palabras.Add(pluralVocal);
            }
            else if (consonantes.Contains(variante[variante.Length - 1]))
            {
                string pluralConsonante = variante + "es";
                palabras.Add(pluralConsonante);
            }
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
