using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Hitboxlar arasındaki etkileşimler
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
        // İlgili alana girdiğinde karakteri takip etmek istiyoruz
        detectedColliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hitboxlar birbirinden ayrıldığında çağırılır. 
        detectedColliders.Remove(collision);
        if (detectedColliders.Count <= 0)
            noCollidersRemain.Invoke();
    }
}
