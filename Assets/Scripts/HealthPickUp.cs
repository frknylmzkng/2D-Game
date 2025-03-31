using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthRestore = 20; // Verece�i can

    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0); // y-ekseninde 180 derece d�necek

    AudioSource pickupSource; // Ses 
    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }

    // Karakterin hitboxu objenin hitboxu ile temasa ge�ti�inde �al��acak
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if(damageable) // Karakter full canda de�ilse can� artabilir
        {
            bool hasHealed = damageable.Heal(healthRestore);

            if (hasHealed) // �yile�me olduysa sesi �alar
            {
                if (pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                }
                Destroy(gameObject); // Al�nd�ktan sonra objeyi yok et
            }                
        }
    }

    // Objenin oyun boyunca belirtilen h�zda d�nmesi i�in
    void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
