using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;


public class WordManager : MonoBehaviour
{
    public string SilabaActual { get; private set; }
    public int MinLength { get; private set; }
    public int MaxLength { get; private set; }

    [SerializeField]
    private Dialogue dialogue;

    public int minRangeMin = 4;
    public int minRangeMax = 6;
    public int minExtraRange = 3;
    public int maxExtraRange = 6;

    string vocales = "aeiou";

    private HashSet<string> palabrasUsadas = new HashSet<string>();

    void Awake()
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
        MinLength = Random.Range(minRangeMin, minRangeMax);
        MaxLength = Random.Range(MinLength + minExtraRange, MinLength + maxExtraRange);

        Debug.Log($"Nueva ronda - Sílaba: {SilabaActual} | Min: {MinLength} | Max: {MaxLength}");
    }



    public bool TryWord(string palabra)
    {
        palabra = LoadWords.Instance.QuitarTildes(palabra).ToLower();

        //  Chequear longitud
        if (palabra.Length < MinLength || palabra.Length > MaxLength)
        {
            Debug.Log("Longitud incorrecta");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraFueraRango);
            }
            return false;
        }

        // 2️⃣ Chequear sílaba inicial
        if (!palabra.StartsWith(SilabaActual))
        {
            Debug.Log("No empieza por la sílaba correcta");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraSilabaIncorrecta);
            }
            return false;
        }

        //  Chequear diccionario
        if (!LoadWords.Instance.Existe(palabra))
        {
            Debug.Log("La palabra no existe");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraMal);
            }
            return false;
        }

        if (palabrasUsadas.Contains(palabra))
        {
            Debug.Log("Palabra ya usada");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraRepetida);
            }
            return false;
        }

        palabrasUsadas.Add(palabra);

        if (dialogue)
        {
            dialogue.LanzarDialogo(TipoDialogo.Bien);
        }

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
