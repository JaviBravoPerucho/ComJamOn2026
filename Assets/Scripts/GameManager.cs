using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instancia pública accesible desde cualquier script
    public static GameManager Instance { get; private set; }

    // Variables globales de ejemplo
    public int Puntuacion;

    public int Level;

    public bool JuegoIniciado;
    public bool JuegoAcabado;

    private void Awake()
    {
        // Si ya existe una instancia y no somos nosotros, nos destruimos
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        JuegoIniciado = false;
        JuegoAcabado = false;
        // Asignamos la instancia
        Instance = this;

        // Evita que se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Inicialización de variables o lógica de inicio
        Level = 0; // 1-fede / 2-Tony / 3-Pedro
        Puntuacion = 0;
    }

    private void Update()
    {
    }

    private void ResetPoints()
    {
        Puntuacion = 0;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void LoadGame(int level)
    {
        JuegoAcabado = false;
        Level = level;
        ResetPoints(); // Opcional: reiniciar puntos al empezar partida
        SceneManager.LoadScene("Game");
    }

    public void LoadConfigMenu()
    {
        ResetPoints(); // Opcional: reiniciar puntos al empezar partida
        SceneManager.LoadScene("Configuration");
    }

    public void GameOver(bool endedProgram)
    {
        if (!endedProgram) Puntuacion = 5;
        else Puntuacion = 0;

        JuegoAcabado = true;
    }
}