using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Arkaplandaki farklý imajlarýn farklý hýzlarda gitmesini istiyoruz ki oyuna derinlik katsýn
public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Parallax oyun nesnesinin baþlangýç konumu (x-y)
    Vector2 startingPosition;

    // Parallax oyun nesnesinin baþlangýç konumu (z)
    float startingZ;

    // Kameranýn parallax nesnesinin baþlangýç konumundan hareket ettiði mesafe
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    // Parallax nesnesinin takip hedefiyle arasýndaki z eksenindeki mesafe 
    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // Nesne hedefin önündeyse, nearClipPlane kullan. Hedefin arkasýndaysa, farClipPlane kullan
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Nesne, oyuncudan ne kadar uzaktaysa, ParallaxEffect nesnesi o kadar hýzlý hareket eder. Daha yavaþ hareket etmesini saðlamak için z-deðerini hedefe yaklaþtýrdým
    float parallaxFactor => MathF.Abs(zDistanceFromTarget) / clippingPlane;

    // Oyun baþladýðýnda Parallax nesnesinin baþlangýç pozisyonunu ve baþlangýç z-deðerini kaydeder. 
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.localPosition.z;
    }

    // Parallax nesnesinin yeni pozisyonunu hesapladýk
    void Update()
    {
        // Hedef hareket ettiðinde, Parallax nesnesini de çarpaný katý kadar hareket ettir. 
        Vector2 newPosition = startingPosition * camMoveSinceStart * parallaxFactor;

        // x-y pozisyonu güncellenir ancak z-deðeri sabit kalýr.
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
