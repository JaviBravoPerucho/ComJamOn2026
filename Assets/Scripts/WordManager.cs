using System.Collections.Generic;
using UnityEngine;


public class WordManager : MonoBehaviour
{
    public string SilabaActual { get; private set; }
    public int MinLength { get; private set; }
    public int MaxLength { get; private set; }

    string vocales = "aeiou";

    private HashSet<string> palabrasUsadas = new HashSet<string>();

    void Start()
    {
        GenerarNuevaRonda(true);
    }

    void GenerarNuevaRonda(bool primeraVez = false)
    {
        if (primeraVez)
        {
            // Generar sílaba inicial aleatoria (consonante + vocal)
            string consonantes = "bcdfglmnprstv";
            char c = consonantes[Random.Range(0, consonantes.Length)];
            char v = vocales[Random.Range(0, vocales.Length)];
            SilabaActual = (c.ToString() + v.ToString()).ToLower();
        }

        // Generar reglas de tamaño
        MinLength = Random.Range(4, 7);
        MaxLength = Random.Range(MinLength + 1, MinLength + 4);

        Debug.Log($"Nueva ronda - Sílaba: {SilabaActual} | Min: {MinLength} | Max: {MaxLength}");
    }

    public bool TryWord(string palabra)
    {
        palabra = palabra.ToLower();

        //  Chequear longitud
        if (palabra.Length < MinLength || palabra.Length > MaxLength)
        {
            Debug.Log("Longitud incorrecta");
            return false;
        }

        // 2️⃣ Chequear sílaba inicial
        if (!palabra.StartsWith(SilabaActual))
        {
            Debug.Log("No empieza por la sílaba correcta");
            return false;
        }

        //  Chequear diccionario
        if (!LoadWords.Instance.Existe(palabra))
        {
            Debug.Log("La palabra no existe");
            return false;
        }

        if (palabrasUsadas.Contains(palabra))
        {
            Debug.Log("Palabra ya usada");
            return false;
        }

        palabrasUsadas.Add(palabra);

        //  Generar siguiente sílaba
        SilabaActual = GenerarNuevaSilaba(palabra);

        //  Nueva regla de longitud
        GenerarNuevaRonda();

        return true;
    }

    string GenerarNuevaSilaba(string palabra)
    {
        int len = palabra.Length;

        char ultima = palabra[len - 1];

        // Regla 1  última letra consonante
        if (!EsVocal(ultima))
        {
            return ultima.ToString();
        }

        // Si la última es vocal
        if (len >= 2)
        {
            char anterior = palabra[len - 2];

            // Regla 2  vocal + vocal
            if (EsVocal(anterior))
            {
                return ultima.ToString();
            }

            // Regla 3  consonante + vocal
            return anterior.ToString() + ultima.ToString();
        }

        return ultima.ToString();
    }

    bool EsVocal(char c)
    {
        return vocales.Contains(c.ToString());
    }
}
