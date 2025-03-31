using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu script, bir nesnenin zeminde mi, duvarda m� yoksa tavanla m� temas halinde oldu�unu collider kullanarak kontrol eder.

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter; // Collider'�n ne t�r nesneleri alg�layaca��n� filtrelemek i�in kullan�l�r.
    public float groundDistance = 0.05f; // Zemin alg�lama mesafesi
    public float wallDistance = 0.2f; // Duvar alg�lama mesafesi
    public float ceilingDistance = 0.05f; // Tavan alg�lama mesafesi

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5]; // Zemin alg�lamak i�in kullan�lacak raycast sonu�lar�
    RaycastHit2D[] wallHits = new RaycastHit2D[5]; // Duvar alg�lamak i�in kullan�lacak raycast sonu�lar�
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5]; // Tavan alg�lamak i�in kullan�lacak raycast sonu�lar�

    [SerializeField] 
    private bool _isGrounded; // Nesnenin zeminde olup olmad���n� tutan �zel de�i�ken
    public bool isGrounded 
    { 
        get 
        { 
            return _isGrounded; 
        }
        
        private set 
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value); // Animator'daki isGrounded parametresini g�nceller
        } 
    }

    [SerializeField]
    private bool _isOnWall; // Nesnenin duvarda olup olmad���n� tutan �zel de�i�ken
    public bool isOnWall
    {
        get
        {
            return _isOnWall;
        }

        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value); // Animator'daki isOnWall parametresini g�nceller
        }
    }

    [SerializeField]
    private bool _isOnCeiling; // Nesnenin tavanla temas halinde olup olmad���n� tutan �zel de�i�ken
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Duvar kontrol� i�in y�n vekt�r�

    public bool isOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }

        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value); // Animator'daki isOnCeiling parametresini g�nceller
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
   
    void FixedUpdate()
    {
        // Zeminde olup olmad���n� kontrol eder
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Duvarla temas halinde olup olmad���n� kontrol eder
        isOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;

        // Tavanla temas halinde olup olmad���n� kontrol eder
        isOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

}
