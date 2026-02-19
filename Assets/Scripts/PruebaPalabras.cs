using Unity.VisualScripting;
using UnityEngine;

public class PruebaPalabras : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public string palabra;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Test();
        }
    }

    void Test()
    {
        Debug.Log("Buscando: " + palabra);
        if (LoadWords.Instance.Existe(palabra))
        {
            Debug.Log("Encontrada");
        }
        else {
            Debug.Log("NoEncontrada");
        }
    }
}
