using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] 
public class Knight : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;

    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;

    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left} // Karakterin yönünü belirler
    private WalkableDirection _walkDirection; // Mevcut yürüme yönü
    private Vector2 walkDirectionVector = Vector2.right; // Baþlangýçtaki yürüme yönü

    // Karakterin yürüme yönünü çevirmek için
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            // Yeni "value"ya eþit deðilse
            if (_walkDirection != value) 
            {
                // Yönünü döndür
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right) 
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                    walkDirectionVector = Vector2.left;
            }            
            _walkDirection = value; 
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } private set { 
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        } 
    }

    // Knight saldýrýrken hareket etmeyecek
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // Saldýrýlar arasý bekleme süresi olacak
    public float AttackCooldown { get {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        } private set {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    // Knight bir hedefe sahip mi onu kontrol ediyor her framede
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;
    }

    // Knight'ýn hangi þartlarda döneceðini belirlemek için
    private void FixedUpdate()
    {
        if (touchingDirections.isGrounded && touchingDirections.isOnWall) // Knight yerde ve duvara deðiyorsa yönünü deðiþtir
        {
            FlipDirection();
        }
        if(!damageable.LockVelocity) // Karakter hareket edebiliyorsa
        {
            if (CanMove && touchingDirections.isGrounded) // &&engele çarptýðýnda engele sýkýþmasýný engelledik
            {
                // Max hýza ivmelendir   
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), 
                    -maxSpeed, maxSpeed), rb.velocity.y);
                // Mathf.Clamp(): Hýzýn -maxSpeed ve maxSpeed deðerleri arasýnda kalmasýný saðlar, karakterin hýzýný sýnýrlar.
            }
            else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            // Karakterin hareketini zamanla sýfýra doðru yavaþlatarak durmasýný simüle eder.
            // Mathf.Lerp(...): Mevcut hýz (rb.velocity.x) ile sýfýr arasýnda lineer bir interpolasyon yaparak karakterin durmasýný saðlar.
        }
    }

    // Karakterin yönünü deðiþtirdiðimiz metot
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else Debug.LogError("Geçerli yürünebilir yön geçerli bir sað veya sol deðerlerine ayarlanmamýþ");
    }

    // Karakteri hasar aldýðýnda geri tepmesini saðlar
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // Uçurum kenarýndan düþmemesi için
    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
            FlipDirection();
    }
}
