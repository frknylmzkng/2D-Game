using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthRestore = 20; // Vereceði can

    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0); // y-ekseninde 180 derece dönecek

    AudioSource pickupSource; // Ses 
    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }

    // Karakterin hitboxu objenin hitboxu ile temasa geçtiðinde çalýþacak
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if(damageable) // Karakter full canda deðilse caný artabilir
        {
            bool hasHealed = damageable.Heal(healthRestore);

            if (hasHealed) // Ýyileþme olduysa sesi çalar
            {
                if (pickupSource)
                {
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                }
                Destroy(gameObject); // Alýndýktan sonra objeyi yok et
            }                
        }
    }

    // Objenin oyun boyunca belirtilen hýzda dönmesi için
    void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}
