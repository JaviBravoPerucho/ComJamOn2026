using UnityEngine;

public class ShowGrade : MonoBehaviour
{
    public float timeToShow;
    private float elapsedTime;

    [SerializeField]
    private GameObject notaPanel;

    private void FixedUpdate()
    {
        
        if (GameManager.Instance && GameManager.Instance.JuegoAcabado)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (elapsedTime > timeToShow) { 
                elapsedTime = 0;
                if(notaPanel)notaPanel.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    private void Start()
    {
        elapsedTime = 0;
    }
}
