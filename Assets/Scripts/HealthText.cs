using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Oyunda etkile�im sonucu ortaya ��kan metinler �zerinde yapt���m ayarlar
public class HealthText : MonoBehaviour
{
    // Yaz�n�n yukar� ��kma h�z�
    public Vector3 moveSpeed = new Vector3 (0, 20, 0);   
    public float timeToFade = 1f;

    RectTransform textTransform;
    TextMeshProUGUI textMeshPro;

    private float timeElapsed = 0f;
    private Color startColor;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime; // Metni hareket ettirdik
        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToFade) // Metnin fade i�lemi
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else Destroy(gameObject);
    }
}
