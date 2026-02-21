using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PanelNota : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nota;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nota.text = GameManager.Instance.Puntuacion.ToString("0.#");
        float mejorPuntuacion = PlayerPrefs.GetFloat("MejorNotaLevel" + GameManager.Instance.Level.ToString());
        Debug.Log("MejorNotaLevel" + GameManager.Instance.Level.ToString());
        if (GameManager.Instance.Puntuacion > mejorPuntuacion) PlayerPrefs.SetFloat("MejorNotaLevel" + GameManager.Instance.Level.ToString(), GameManager.Instance.Puntuacion);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
