using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Hasar alabilen karakterler ile ilgili ayarlamalar
public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    Animator animator;

    [SerializeField] // Motor içinde güncel deðerini görebilmek için
    private int _maxHealt = 100;
    public int MaxHealth
    {
        get {
            return _maxHealt;
        }
        set {
            _maxHealt = value;
        }
    }

    [SerializeField] // Motor içinde güncel deðerini görebilmek için
    private int _healt = 100;
    public int Health
    {
        get
        {
            return _healt;
        }
        set
        {
            _healt = value;
            if (_healt <= 0) 
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField] // Motor içinde güncel deðerini görebilmek için
    private bool _isAlive = true;

    [SerializeField] // Motor içinde güncel deðerini görebilmek için
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set: " + value);

            if (value == false)
                damageableDeath.Invoke();
        }
    }

    // Hýz, bu deðer true iken deðiþmemeli
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime) // Belirli bir süre sonra hasar almazlýðý kaldýrdým
            {
                // Invincibility'i kaldýr
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }       
    }

    // Hasar alabilen objenin hasar alýp almadýðýný dön
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible) 
        {
            Health -= damage;
            isInvincible = true;

            // Hasar alan objeden miras alan çocuklara hasar alýndýðýný döndür. Bunu geri tepmeyi kontrol etmek için yaptýk
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback); 

            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        // Vurulamaz (Unable to hit)
        return false;
    }

    // Karakterin iyileþip iyileþmediðini dön
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth) // Karakter hayattaysa ve saðlýðý full deðilse
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;   
    }
}
