using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Hitboxlar aras�ndaki etkile�imler
public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;

    public List<Collider2D> detectedColliders = new List<Collider2D>();
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �lgili alana girdi�inde karakteri takip etmek istiyoruz
        detectedColliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hitboxlar birbirinden ayr�ld���nda �a��r�l�r. 
        detectedColliders.Remove(collision);
        if (detectedColliders.Count <= 0)
            noCollidersRemain.Invoke();
    }
}
