using TMPro;
using UnityEngine;

public class ShowPoints : MonoBehaviour
{
    public GameObject FedeNote;
    public GameObject ToniNote;
    public GameObject PedroNote;

    public TMP_Text FedeText;
    public TMP_Text ToniText;
    public TMP_Text PedroText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float fedeNote = PlayerPrefs.GetFloat("MejorNotaLevel1");
        float toniNote = PlayerPrefs.GetFloat("MejorNotaLevel2");
        float pedroNote = PlayerPrefs.GetFloat("MejorNotaLevel3");

        if (fedeNote > 0)
        {
            FedeNote.SetActive(true);
            FedeText.text = fedeNote.ToString("0.#");
        }
        if (toniNote > 0)
        {
            ToniNote.SetActive(true);
            ToniText.text = toniNote.ToString("0.#");
        }
        if (pedroNote > 0)
        {
            PedroNote.SetActive(true);
            PedroText.text = pedroNote.ToString("0.#");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
