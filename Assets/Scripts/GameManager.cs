using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public float Puntuacion;
    public int Level;
    public bool JuegoIniciado;
    public bool JuegoAcabado;

    public WordManager wordManager;

    [Header("Audio")]
    public EventReference Music;
    public EventReference Button;

    private EventInstance musicInstance;
    private bool musicStarted;

    public void ButtonSound()
    {
        RuntimeManager.PlayOneShot(Button);
    }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        JuegoIniciado = false;
        JuegoAcabado = false;
        Puntuacion = 0;

        // Escuchar cambios de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        bool isGameScene = sceneName.StartsWith("Game");
        bool isMenuScene = sceneName == "Menu";

        if (isGameScene)
        {
            StopMusic();
        }
        else
        {
            if (!musicStarted)
            {
                StartMusic();
            }
        }
    }

    #region Music Control

    private void StartMusic()
    {
        if (musicStarted) return;

        musicInstance = RuntimeManager.CreateInstance(Music);
        musicInstance.start();
        musicStarted = true;
    }

    private void StopMusic()
    {
        if (!musicStarted) return;

        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        musicStarted = false;
    }

    #endregion

    #region Scene Management

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

    public void LoadConfigMenu()
    {
        ResetPoints();
        SceneManager.LoadScene("Configuration");
    }

    public void LoadGame(int level)
    {
        JuegoAcabado = false;
        GameManager.Instance.Level = level;
        Debug.Log("LEVEL: " + level);
        ResetPoints();

        switch (level)
        {
            case 1:
                SceneManager.LoadScene("GameFede");
                break;

            case 2:
                SceneManager.LoadScene("GameToni");
                break;

            case 3:
                SceneManager.LoadScene("GamePedro");
                break;
        }
    }

    public void GameOver()
    {
        JuegoAcabado = true;
    }

    #endregion
}