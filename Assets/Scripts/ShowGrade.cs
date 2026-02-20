using UnityEngine;

public class ShowGrade : MonoBehaviour
{
    public float timeToShow;
    private float elapsedTime;

    [SerializeField]
    private GameObject notaPanel;

    [SerializeField]
    private Dialogue dialogue;  

    private void FixedUpdate()
    {
        
        if (GameManager.Instance && GameManager.Instance.JuegoAcabado)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime > timeToShow) { 
                elapsedTime = 0;
                if(notaPanel)notaPanel.SetActive(true);
                if (dialogue)
                {
                    float nota = GameManager.Instance.Puntuacion;
                    Debug.Log("TO NOTA:" + nota);
                    if(nota < 5.0)
                    {
                        dialogue.LanzarDialogo(TipoDialogo.NotaMala);
                    }
                    else if(nota < 8)
                    {
                        dialogue.LanzarDialogo(TipoDialogo.NotaMedia);
                    }
                    else
                    {
                        dialogue.LanzarDialogo(TipoDialogo.BuenaNota);
                    }
                    
                }
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        elapsedTime = 0;
        GameManager.Instance.JuegoAcabado = false;
    }
}
