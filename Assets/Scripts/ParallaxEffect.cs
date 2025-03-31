using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Arkaplandaki farkl� imajlar�n farkl� h�zlarda gitmesini istiyoruz ki oyuna derinlik kats�n
public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Parallax oyun nesnesinin ba�lang�� konumu (x-y)
    Vector2 startingPosition;

    // Parallax oyun nesnesinin ba�lang�� konumu (z)
    float startingZ;

    // Kameran�n parallax nesnesinin ba�lang�� konumundan hareket etti�i mesafe
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    // Parallax nesnesinin takip hedefiyle aras�ndaki z eksenindeki mesafe 
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // Nesne hedefin �n�ndeyse, nearClipPlane kullan. Hedefin arkas�ndaysa, farClipPlane kullan
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Nesne, oyuncudan ne kadar uzaktaysa, ParallaxEffect nesnesi o kadar h�zl� hareket eder. Daha yava� hareket etmesini sa�lamak i�in z-de�erini hedefe yakla�t�rd�m
    float parallaxFactor => MathF.Abs(zDistanceFromTarget) / clippingPlane;

    // Oyun ba�lad���nda Parallax nesnesinin ba�lang�� pozisyonunu ve ba�lang�� z-de�erini kaydeder. 
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.localPosition.z;
    }

    // Parallax nesnesinin yeni pozisyonunu hesaplad�k
    void Update()
    {
        // Hedef hareket etti�inde, Parallax nesnesini de �arpan� kat� kadar hareket ettir. 
        Vector2 newPosition = startingPosition * camMoveSinceStart * parallaxFactor;

        // x-y pozisyonu g�ncellenir ancak z-de�eri sabit kal�r.
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
