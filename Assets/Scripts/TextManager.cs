using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

// 1. Enumeración con todos los tipos de líneas que puedes tener en C++
public enum TipoInstruccionCpp
{
    Include,
    Namespace,
    Main,
    Enum,
    Awake,
    Start,
    Void,
    Update,
    Valor,
    ClaseDef,
    If,
    For,
    VariableInt,
    PrintConsola,
    ReturnCero,
    CierreBloque,
    Comentario
}

public class TextManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TMP_InputField campoEntrada;
    public TMP_Text textoDestino;
    public TMP_Text contadorLongitud;
    public TMP_Text siguienteSilaba;
    public TMP_Text rangoLongitud;
    public TMP_Text lineIndex;

    private WordManager wordManager;

    [SerializeField]
    private CronoController cronoController;

    [SerializeField]
    private GameObject buttonCompile;

    [SerializeField]
    private Dialogue dialogue;

    [SerializeField]
    private GameObject scrollbar;

    private ScrollBarController scrollbarcontroller;

    // 2. Definición visual (Palabras y colores)
    [System.Serializable]
    public class DefinicionSintaxis
    {
        public TipoInstruccionCpp tipo;
        public string palabraClave;
        public Color colorPalabraClave;
        public Color colorInput;
    }

    // 3. Estructura de cada "Programa" o nivel
    [System.Serializable]
    public class NivelPrograma
    {
        public string nombreDelPrograma;
        public TipoInstruccionCpp[] secuenciaLineas; // Aquí solo eliges el orden
    }

    [Header("Diccionario de Sintaxis (Define una sola vez)")]
    public DefinicionSintaxis[] diccionarioSintaxis = new DefinicionSintaxis[]
    {
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.Include, palabraClave = "#include ", colorPalabraClave = new Color(0f, 0.6f, 0f, 1f), colorInput = new Color(0.8f, 0.4f, 0f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.Namespace, palabraClave = "\nusing namespace ", colorPalabraClave = new Color(0f, 0f, 0.8f, 1f), colorInput = new Color(1f, 1f, 1f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.Main, palabraClave = "\nint main() {\n\t", colorPalabraClave = new Color(0f, 0f, 0.8f, 1f), colorInput = new Color(0.5f, 0.5f, 0.5f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.ClaseDef, palabraClave = "\nclass ", colorPalabraClave = new Color(0f, 0f, 0.8f, 1f), colorInput = new Color(1f, 0.8f, 0f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.VariableInt, palabraClave = "\tint ", colorPalabraClave = new Color(0f, 0f, 0.8f, 1f), colorInput = new Color(1f, 1f, 1f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.PrintConsola, palabraClave = "\tstd::cout << ", colorPalabraClave = new Color(0f, 0.5f, 0.5f, 1f), colorInput = new Color(1f, 0.5f, 0.5f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.ReturnCero, palabraClave = "\treturn ", colorPalabraClave = new Color(0f, 0f, 0.8f, 1f), colorInput = new Color(0.8f, 0.8f, 0f, 1f) },
        new DefinicionSintaxis { tipo = TipoInstruccionCpp.CierreBloque, palabraClave = "}", colorPalabraClave = new Color(0.5f, 0.5f, 0.5f, 1f), colorInput = new Color(0f, 0f, 0f, 0f) } // Input invisible si no se necesita
    };

    [Header("Tus Programas (Niveles)")]
    public NivelPrograma[] misProgramas = new NivelPrograma[]
    {
        // Programa 1
        new NivelPrograma {
            nombreDelPrograma = "Hola Mundo",
            secuenciaLineas = new TipoInstruccionCpp[] {
                TipoInstruccionCpp.Include,      // Escribirá: <iostream>
                TipoInstruccionCpp.Namespace,    // Escribirá: std;
                TipoInstruccionCpp.Main,         // Escribirá: //inicio
                TipoInstruccionCpp.PrintConsola, // Escribirá: "Hola";
                TipoInstruccionCpp.ReturnCero,   // Escribirá: 0;
                TipoInstruccionCpp.CierreBloque  // Escribirá: //fin
            }
        },
        // Programa 2
        new NivelPrograma {
            nombreDelPrograma = "Clase Jugador",
            secuenciaLineas = new TipoInstruccionCpp[] {
                TipoInstruccionCpp.Include,
                TipoInstruccionCpp.ClaseDef,
                TipoInstruccionCpp.VariableInt,
                TipoInstruccionCpp.VariableInt,
                TipoInstruccionCpp.CierreBloque
            }
        }
    };

    // Control de progreso
    private int programaActualIndex = 0;
    private int lineaActualIndex = 0;
    private bool finished = false;
    public int longitudPrograma;

    [Header("Configuración de Scroll")]
    [SerializeField] private int lineasMaximasAntesDeSubir = 10;
    [SerializeField] private float alturaDeLinea = 25f; // Ajusta según el tamaño de tu fuente

    private RectTransform rectTexto;
    private RectTransform rectIndex;
    private int contadorLineasTotales = 0; // Para saber cuántas líneas reales llevamos
    private int lineasAgregadas;

    private void Start()
    {
        
        wordManager = GetComponent<WordManager>();

        GameManager.Instance.JuegoIniciado = true;
        // Generar sílaba inicial aleatoria (consonante + vocal)
        string consonantes = "bcdfglmnprstv";
        string vocales = "aeiou";

        rectTexto = textoDestino.GetComponent<RectTransform>();
        rectIndex = lineIndex.GetComponent<RectTransform>();

        char c = consonantes[Random.Range(0, consonantes.Length)];
        char v = vocales[Random.Range(0, vocales.Length)];
        wordManager.SilabaActual = (c.ToString() + v.ToString()).ToLower();

        if (scrollbar)
        {
            scrollbarcontroller = scrollbar.GetComponent<ScrollBarController>();
        }
        

        // Generar reglas de tamaño
        wordManager.MinLength = Random.Range(wordManager.minRangeMin, wordManager.minRangeMax);
        wordManager.MaxLength = Random.Range(wordManager.MinLength + wordManager.minExtraRange, wordManager.MinLength + wordManager.maxExtraRange);

        //Debug.Log($"Nueva ronda - Sílaba: {wordManager.SilabaActual} | Min: {wordManager.MinLength} | Max: {wordManager.MaxLength}");


        campoEntrada.onValueChanged.AddListener(ActualizarContador);
        programaActualIndex = Random.Range(0, misProgramas.Length);
        longitudPrograma = misProgramas[programaActualIndex].secuenciaLineas.Length;
        ActualizarUIWordManager();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (wordManager.TryWord(campoEntrada.text))
            {
                EnviarTexto();
            }
            else
            {
                campoEntrada.text = "";
            }
            campoEntrada.ActivateInputField();
        }
    }

    public void ActualizarContador(string text)
    {
        contadorLongitud.text = text.Length.ToString();
    }

    public void EnviarTexto()
    {
        // Si ya terminamos todos los programas, no hacemos nada
        if (programaActualIndex >= misProgramas.Length || finished) return;

        NivelPrograma programaActual = misProgramas[programaActualIndex];

        // Obtenemos qué tipo de instrucción toca en este momento
        TipoInstruccionCpp tipoActual = programaActual.secuenciaLineas[lineaActualIndex];

        // Buscamos la configuración visual de esa instrucción en el diccionario
        DefinicionSintaxis sintaxis = ObtenerSintaxis(tipoActual);

        if (sintaxis != null)
        {
            string hexClave = ColorUtility.ToHtmlStringRGBA(sintaxis.colorPalabraClave);
            string hexInput = ColorUtility.ToHtmlStringRGBA(sintaxis.colorInput);

            string nuevaLinea = $"<color=#{hexClave}>{sintaxis.palabraClave}</color><color=#{hexInput}>{campoEntrada.text}</color>\n";
            textoDestino.text += nuevaLinea;


            string palabraClaveProcesada = sintaxis.palabraClave.Replace("\\n", "\n");
            // Incrementamos el contador global y comprobamos el scroll
            //contadorLineasTotales++;
            lineasAgregadas = palabraClaveProcesada.Count(c => c == '\n') + 1;
            contadorLineasTotales += lineasAgregadas;
            //AplicarScroll();

            if (scrollbarcontroller && contadorLineasTotales > lineasMaximasAntesDeSubir)
            {
                scrollbarcontroller.OnScrollChanged((contadorLineasTotales - lineasMaximasAntesDeSubir) * alturaDeLinea / scrollbarcontroller.distanciaRecorrido);
                //Debug.Log("Nuevo valor scroll" + lineasAgregadas * alturaDeLinea / scrollbarcontroller.distanciaRecorrido);
            }
            else if(scrollbarcontroller)
            {
                scrollbarcontroller.OnScrollChanged(0);
            }
        }

        AvanzarDeLinea(programaActual);

        // Limpieza y reseteo
        campoEntrada.text = "";
        campoEntrada.ActivateInputField();
        ActualizarUIWordManager();
    }

    private void AplicarScroll()
    {
        // Si hemos superado el límite de líneas visibles
        if (contadorLineasTotales > lineasMaximasAntesDeSubir)
        {
            // Calculamos la nueva posición sumando la altura de línea a la Y
            // (En UI, subir significa aumentar el valor de Y si el pivot está arriba)
            float nuevaY = rectTexto.anchoredPosition.y + alturaDeLinea;

            rectTexto.anchoredPosition = new Vector2(rectTexto.anchoredPosition.x, nuevaY);
            rectIndex.anchoredPosition = new Vector2(rectIndex.anchoredPosition.x, nuevaY);
        }
    }

    private void AvanzarDeLinea(NivelPrograma programaActual)
    {
        lineaActualIndex++;
        //lineIndex.text += lineaActualIndex.ToString()+"\n";
        Debug.Log(lineasAgregadas);
        for (int i = 0; i < lineasAgregadas; i++)
        {
            lineIndex.text += (contadorLineasTotales - lineasAgregadas + i + 1).ToString() + "\n";
        }
        // Si hemos llegado al final del programa actual
        if (lineaActualIndex >= programaActual.secuenciaLineas.Length)
        {
            textoDestino.text += "\n<color=#00FF00>--- FIN DEL PROGRAMA: " + programaActual.nombreDelPrograma.ToUpper() + " ---</color>\n\n";
            lineaActualIndex = 0;
            finished = true;
            if (cronoController)
            {
                cronoController.StopCrono();
            }
            if (buttonCompile)
            {
                buttonCompile.SetActive(true);
            }          
            if (dialogue)
            {
                dialogue.LanzarDialogo(TipoDialogo.SeAcaboExamen);
            }
            GameManager.Instance.JuegoIniciado = false;
            GameManager.Instance.Puntuacion = wordManager.calcularNota();
            //programaActualIndex++;
        }
    }

    // Busca en el array de diccionario el tipo de instrucción correspondiente
    private DefinicionSintaxis ObtenerSintaxis(TipoInstruccionCpp tipoBuscado)
    {
        foreach (var def in diccionarioSintaxis)
        {
            if (def.tipo == tipoBuscado) return def;
        }
        Debug.LogWarning("No se encontró configuración para la instrucción: " + tipoBuscado);
        return null;
    }

    private void ActualizarUIWordManager()
    {
        siguienteSilaba.text = "Palabra que empiece por: " + wordManager.SilabaActual;
        rangoLongitud.text = "(Min " + wordManager.MinLength + " - Max " + wordManager.MaxLength + ")";
    }
}