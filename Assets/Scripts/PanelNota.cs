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
        nota.text = GameManager.Instance.Puntuacion.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
