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

    [SerializeField] // Motor i�inde g�ncel de�erini g�rebilmek i�in
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

    [SerializeField] // Motor i�inde g�ncel de�erini g�rebilmek i�in
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

    [SerializeField] // Motor i�inde g�ncel de�erini g�rebilmek i�in
    private bool _isAlive = true;

    [SerializeField] // Motor i�inde g�ncel de�erini g�rebilmek i�in
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

    // H�z, bu de�er true iken de�i�memeli
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
            if (timeSinceHit > invincibilityTime) // Belirli bir s�re sonra hasar almazl��� kald�rd�m
            {
                // Invincibility'i kald�r
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }       
    }

    // Hasar alabilen objenin hasar al�p almad���n� d�n
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible) 
        {
            Health -= damage;
            isInvincible = true;

            // Hasar alan objeden miras alan �ocuklara hasar al�nd���n� d�nd�r. Bunu geri tepmeyi kontrol etmek i�in yapt�k
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback); 

            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        // Vurulamaz (Unable to hit)
        return false;
    }

    // Karakterin iyile�ip iyile�medi�ini d�n
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth) // Karakter hayattaysa ve sa�l��� full de�ilse
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
