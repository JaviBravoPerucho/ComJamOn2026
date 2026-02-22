using UnityEngine;

public class HideExit : MonoBehaviour
{
    [SerializeField]
    private GameObject quitButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        quitButton.SetActive(false);
    }
}
