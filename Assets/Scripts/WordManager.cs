using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;


public class WordManager : MonoBehaviour
{
    public string SilabaActual;
    public int MinLength;
    public int MaxLength;

    [SerializeField]
    private CameraShake2D shake2D;
    [SerializeField]
    private Dialogue dialogue;
    [SerializeField]
    private CronoController cronoController;

    public int minRangeMin = 4;
    public int minRangeMax = 6;
    public int minExtraRange = 3;
    public int maxExtraRange = 6;

    string vocales = "aeiou";

    private HashSet<string> palabrasUsadas = new HashSet<string>();

    public struct sesgoCorreccion
    {
        public int numPalabrasConseguidas;
        public float factorTiempoRestante;
        public int longitudTotalPalabras;
        public float nivelDificultad;
    }

    private sesgoCorreccion correccion;

    void Start()
    {
    }

    public int calcularNota()
    {
        correccion = new sesgoCorreccion();
        correccion.numPalabrasConseguidas = 0;
        correccion.factorTiempoRestante = cronoController.GetTimePorcentage();
        correccion.longitudTotalPalabras = 0;
        correccion.nivelDificultad = GameManager.Instance.Level;

        foreach(string palabra in palabrasUsadas)
        {
            correccion.numPalabrasConseguidas++;
            correccion.longitudTotalPalabras+=palabra.Length;
        }
        int longitudPrograma = GetComponent<TextManager>().longitudPrograma;
        int nota = 0;//Pendiente el calculo de nota
        return nota;
    }

    void GenerarNuevaRonda(bool primeraVez = false)
    {

        // Generar reglas de tamaño
        MinLength = Random.Range(minRangeMin, minRangeMax);
        MaxLength = Random.Range(MinLength + minExtraRange, MinLength + maxExtraRange);

        Debug.Log($"Nueva ronda - Sílaba: {SilabaActual} | Min: {MinLength} | Max: {MaxLength}");
    }



    public bool TryWord(string palabra)
    {

        if (!GameManager.Instance.JuegoIniciado)
        {
            shake2D.Shake(0.2f, 0.1f);
            return false;
        }

        palabra = LoadWords.Instance.QuitarTildes(palabra).ToLower();

        //  Chequear longitud
        if (palabra.Length < MinLength || palabra.Length > MaxLength)
        {
            shake2D.Shake(0.2f, 0.1f);
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
            shake2D.Shake(0.2f, 0.1f);
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
            shake2D.Shake(0.2f, 0.1f);
            Debug.Log("La palabra no existe");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraMal);
            }
            return false;
        }

        if (palabrasUsadas.Contains(palabra))
        {
            shake2D.Shake(0.2f, 0.1f);
            Debug.Log("Palabra ya usada");
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.PalabraRepetida);
            }
            return false;
        }

        palabrasUsadas.Add(palabra);

        if (cronoController)
        {
            cronoController.addTime();
        }

        if (dialogue)
        {
            dialogue.LanzarDialogo(TipoDialogo.Bien);
        }

        if (GetComponent<FakeCompilerConsole>())
        {
            GetComponent<FakeCompilerConsole>().SetWords(palabrasUsadas, GameManager.Instance.Puntuacion);
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
        if (len == 0) return "";

        char ultima = palabra[len - 1];

        // Regla 1: última letra es consonante
        if (!EsVocal(ultima))
        {
            return ultima.ToString();
        }

        // Si la última es vocal, revisamos la anterior
        if (len >= 2)
        {
            char anterior = palabra[len - 2];

            // Regla 2: vocal + vocal
            if (EsVocal(anterior))
            {
                return ultima.ToString();
            }

            // --- EXCEPCIONES DE CONSONANTES ---

            // Excepción de la 'x' 
            if (anterior == 'x' ||anterior == 'y')
            {
                return ultima.ToString();
            }

            // Excepción de la 'z' con 'e' e 'i'
            if ((anterior == 'z') && (ultima == 'e' || ultima == 'i'))
            {
                return ultima.ToString();
            }

            // Regla 3 Estándar: consonante + vocal
            return anterior.ToString() + ultima.ToString();
        }

        return ultima.ToString();
    }

    bool EsVocal(char c)
    {
        return vocales.Contains(c.ToString());
    }
}
