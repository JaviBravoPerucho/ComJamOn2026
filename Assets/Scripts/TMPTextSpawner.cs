using UnityEngine;
using TMPro;
using System.Collections;

public class TMPTextSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float generationsPerSecond = 5f;
    public int textsPerGeneration = 1;
    public Vector3 spawnArea = new Vector3(5, 5, 5);

    [Header("Text Settings")]
    public int minTextLength = 3;
    public int maxTextLength = 10;
    public float fontSize = 3f;

    [Header("Physics Settings")]
    public float forceAmount = 5f;
    public bool useRandomForceDirection = true;

    [Header("Lifetime")]
    public float lifetime = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        float interval = 1f / generationsPerSecond;

        while (timer >= interval)
        {
            timer -= interval;
            GenerateTexts();
        }
    }

    void GenerateTexts()
    {
        for (int i = 0; i < textsPerGeneration; i++)
        {
            CreateTMPObject();
        }
    }

    void CreateTMPObject()
    {
        // Crear GameObject
        GameObject go = new GameObject("TMP_Text_Object");

        // Posición aleatoria dentro del área
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            Random.Range(-spawnArea.z, spawnArea.z)
        );

        go.transform.position = randomPos;

        // Agregar TMP_Text (3D)
        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        tmp.text = GenerateRandomText();
        tmp.fontSize = fontSize;
        tmp.color = Random.ColorHSV();
        tmp.alignment = TextAlignmentOptions.Center;

        // Agregar Collider
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.size = Vector3.one;

        // Agregar Rigidbody
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();

        if (useRandomForceDirection)
        {
            Vector2 randomDir = Random.onUnitSphere;
            rb.AddForce(randomDir * forceAmount, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * forceAmount, ForceMode2D.Impulse);
        }

        // Destruir después de X tiempo
        Destroy(go, lifetime);
    }

    string GenerateRandomText()
    {
        string result = "";

        if (GameManager.Instance)
        {
            result = GameManager.Instance.gameObject.GetComponent<LoadWords>().GetRandomWord();
            return result;
        }

        int length = Random.Range(minTextLength, maxTextLength + 1);
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        for (int i = 0; i < length; i++)
        {
            result += chars[Random.Range(0, chars.Length)];
        }

        return result;
    }
}