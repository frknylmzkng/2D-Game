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

    public enum WalkableDirection { Right, Left} // Karakterin y�n�n� belirler
    private WalkableDirection _walkDirection; // Mevcut y�r�me y�n�
    private Vector2 walkDirectionVector = Vector2.right; // Ba�lang��taki y�r�me y�n�

    // Karakterin y�r�me y�n�n� �evirmek i�in
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            // Yeni "value"ya e�it de�ilse
            if (_walkDirection != value) 
            {
                // Y�n�n� d�nd�r
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

    // Knight sald�r�rken hareket etmeyecek
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    // Sald�r�lar aras� bekleme s�resi olacak
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

    // Knight'�n hangi �artlarda d�nece�ini belirlemek i�in
    private void FixedUpdate()
    {
        if (touchingDirections.isGrounded && touchingDirections.isOnWall) // Knight yerde ve duvara de�iyorsa y�n�n� de�i�tir
        {
            FlipDirection();
        }
        if(!damageable.LockVelocity) // Karakter hareket edebiliyorsa
        {
            if (CanMove && touchingDirections.isGrounded) // &&engele �arpt���nda engele s�k��mas�n� engelledik
            {
                // Max h�za ivmelendir   
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), 
                    -maxSpeed, maxSpeed), rb.velocity.y);
                // Mathf.Clamp(): H�z�n -maxSpeed ve maxSpeed de�erleri aras�nda kalmas�n� sa�lar, karakterin h�z�n� s�n�rlar.
            }
            else rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            // Karakterin hareketini zamanla s�f�ra do�ru yava�latarak durmas�n� sim�le eder.
            // Mathf.Lerp(...): Mevcut h�z (rb.velocity.x) ile s�f�r aras�nda lineer bir interpolasyon yaparak karakterin durmas�n� sa�lar.
        }
    }

    // Karakterin y�n�n� de�i�tirdi�imiz metot
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
        else Debug.LogError("Ge�erli y�r�nebilir y�n ge�erli bir sa� veya sol de�erlerine ayarlanmam��");
    }

    // Karakteri hasar ald���nda geri tepmesini sa�lar
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // U�urum kenar�ndan d��memesi i�in
    public void OnCliffDetected()
    {
        if (touchingDirections.isGrounded)
            FlipDirection();
    }
}
