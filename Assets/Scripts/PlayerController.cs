using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] // Bu componentleri bu script i�in zorunlu tuttum
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;

    Vector2 moveInput; 
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;
    
    // Karakterin anl�k hareket h�z�n� hesaplar ve d�ner
    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.isOnWall) // Duvar temas� varsa hareket edemiycez
                {
                    if (touchingDirections.isGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else return walkSpeed;
                    }
                    else return airWalkSpeed; // Havadaki h�z�
                }
                else return 0; // Idle h�z� 0
            }
            else return 0; // Hareket yok
        } 
    }

    // Bile�enleri ilk burda ald�k
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    [SerializeField] // �lgili bool de�erini private olsa bile motor i�inde g�rebilmemiz i�in
    private bool _isMoving= false;
    public bool IsMoving 
    { 
        get
        {
            return _isMoving;
        } 
        private set 
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField] // �lgili bool de�erini private olsa bile motor i�inde g�rebilmemiz i�in
    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; animator.SetBool(AnimationStrings.isRunning, value); } }


    public bool _isFacingRight = true; 
    public bool IsFacingRight { get 
        { return _isFacingRight; } 
        private set 
        {
            // "value" yeni de�er ise d�n
            if (_isFacingRight != value)
            {
                // Karakteri z�t y�ne �evirmek i�in Local Scale'i �evir. 
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity) // Hareket ediyor
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y); // H�z de�erini ald�k
         
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y); // �lgili animator parametresine bu de�eri atad�k
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput); // Hareket y�n�ne g�re karakterin bakt��� y�n� de�i�tiriyor. 
        }
        else IsMoving = false;
     
    }

    // Karakterin haraket etti�i y�ne d�nmesi i�in
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) //Halihaz�rda ilgili y�ne d�n�p d�nmedi�ini de sorgulad�k 
        {
            // Sa�a d�n y�z�n�
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Sola d�n
            IsFacingRight = false;
        }
    }

    public bool CanMove { get { 
        return animator.GetBool(AnimationStrings.canMove);
        } 
    }

    public bool IsAlive{
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public bool LockVelocity { get {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set{
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    // Ko�ma durumu
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    // Z�plama durumu
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.isGrounded && CanMove) // �lgili tu�a bas�ld�ysa ve karakter zemindeyse ve hareket edebiliyorsa
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity=new Vector2 (rb.velocity.x, jumpImpulse); 
        }
    }

    // Sald�r� (K�l��)
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    //Sald�r� (Ok)
    public void OnRangeAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    // Geri tepme
    public void OnHit(int damage, Vector2 knockback)
    {       
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
