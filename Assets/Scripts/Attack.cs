using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vurulabilir olup olmad���na bakt�k
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            // Knockback y�n�n�, sald�r� y�n�ne g�re ayarlad�m
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            // Hedefe vur
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gotHit)
                Debug.Log(collision.name + "hit for" + attackDamage);
        }
    }
}
