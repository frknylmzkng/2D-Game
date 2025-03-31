using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))] // Bu componentleri bu script için zorunlu tuttum
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
    
    // Karakterin anlýk hareket hýzýný hesaplar ve döner
    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.isOnWall) // Duvar temasý varsa hareket edemiycez
                {
                    if (touchingDirections.isGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else return walkSpeed;
                    }
                    else return airWalkSpeed; // Havadaki hýzý
                }
                else return 0; // Idle hýzý 0
            }
            else return 0; // Hareket yok
        } 
    }

    // Bileþenleri ilk burda aldýk
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    [SerializeField] // Ýlgili bool deðerini private olsa bile motor içinde görebilmemiz için
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

    [SerializeField] // Ýlgili bool deðerini private olsa bile motor içinde görebilmemiz için
    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; animator.SetBool(AnimationStrings.isRunning, value); } }


    public bool _isFacingRight = true; 
    public bool IsFacingRight { get 
        { return _isFacingRight; } 
        private set 
        {
            // "value" yeni deðer ise dön
            if (_isFacingRight != value)
            {
                // Karakteri zýt yöne çevirmek için Local Scale'i çevir. 
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        } 
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity) // Hareket ediyor
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y); // Hýz deðerini aldýk
         
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y); // Ýlgili animator parametresine bu deðeri atadýk
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput); // Hareket yönüne göre karakterin baktýðý yönü deðiþtiriyor. 
        }
        else IsMoving = false;
     
    }

    // Karakterin haraket ettiði yöne dönmesi için
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) //Halihazýrda ilgili yöne dönüp dönmediðini de sorguladýk 
        {
            // Saða dön yüzünü
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Sola dön
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

    // Koþma durumu
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

    // Zýplama durumu
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.isGrounded && CanMove) // Ýlgili tuþa basýldýysa ve karakter zemindeyse ve hareket edebiliyorsa
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity=new Vector2 (rb.velocity.x, jumpImpulse); 
        }
    }

    // Saldýrý (Kýlýç)
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    //Saldýrý (Ok)
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
