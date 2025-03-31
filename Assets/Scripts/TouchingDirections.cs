using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu script, bir nesnenin zeminde mi, duvarda mý yoksa tavanla mý temas halinde olduðunu collider kullanarak kontrol eder.

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter; // Collider'ýn ne tür nesneleri algýlayacaðýný filtrelemek için kullanýlýr.
    public float groundDistance = 0.05f; // Zemin algýlama mesafesi
    public float wallDistance = 0.2f; // Duvar algýlama mesafesi
    public float ceilingDistance = 0.05f; // Tavan algýlama mesafesi

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5]; // Zemin algýlamak için kullanýlacak raycast sonuçlarý
    RaycastHit2D[] wallHits = new RaycastHit2D[5]; // Duvar algýlamak için kullanýlacak raycast sonuçlarý
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5]; // Tavan algýlamak için kullanýlacak raycast sonuçlarý

    [SerializeField] 
    private bool _isGrounded; // Nesnenin zeminde olup olmadýðýný tutan özel deðiþken
    public bool isGrounded 
    { 
        get 
        { 
            return _isGrounded; 
        }
        
        private set 
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value); // Animator'daki isGrounded parametresini günceller
        } 
    }

    [SerializeField]
    private bool _isOnWall; // Nesnenin duvarda olup olmadýðýný tutan özel deðiþken
    public bool isOnWall
    {
        get
        {
            return _isOnWall;
        }

        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value); // Animator'daki isOnWall parametresini günceller
        }
    }

    [SerializeField]
    private bool _isOnCeiling; // Nesnenin tavanla temas halinde olup olmadýðýný tutan özel deðiþken
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left; // Duvar kontrolü için yön vektörü

    public bool isOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }

        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value); // Animator'daki isOnCeiling parametresini günceller
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }
   
    void FixedUpdate()
    {
        // Zeminde olup olmadýðýný kontrol eder
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        // Duvarla temas halinde olup olmadýðýný kontrol eder
        isOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;

        // Tavanla temas halinde olup olmadýðýný kontrol eder
        isOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

}
