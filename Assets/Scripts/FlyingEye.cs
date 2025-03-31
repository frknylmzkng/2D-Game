using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Uçan düþman ile ilgili ayarlamalar
public class FlyingEye : MonoBehaviour
{
    public float flySpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public Collider2D deathCollider;
    public List<Transform> waypoints;


    Animator animator;
    Rigidbody2D rb; // Karakteri haraket ettirmek istiyoruz
    Damageable damageable;

    Transform nextWaypoint;  // Waypointlerin konumunu almak için
    int waypointNumber = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        nextWaypoint = waypoints[waypointNumber];
    }

    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
        }
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Waypointler arasýnda uç ve baþlangýç noktasýna dön
    private void Flight()
    {
        // Sonraki waypointe uç
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized; //normalize ettiðimizde sadece yön aldýk. hýz vs olmadan

        // Waypointe gelip gelmediðini kontrol et
        float distance = Vector2.Distance(nextWaypoint.position, transform.position); // Ýki nokta arasýndaki mesafe

        rb.velocity = directionToWaypoint * flySpeed;
        UpdateDirection();

        // Waypointi deðiþtirip deðiþtirmeyeceðimizi kontrol et
        if (distance <= waypointReachedDistance)
        {
            // Sonraki waypointe geç
            waypointNumber++;
            if (waypointNumber >= waypoints.Count) { waypointNumber = 0; } // Tekrar baþlangýç noktasýna dön

            nextWaypoint = waypoints[waypointNumber];
        }

    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if (transform.localScale.x > 0)
        {
            // Saða dönme kýsmý
            if (rb.velocity.x < 0)
            {
                // Dönme iþlemi
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }

        }
        else
        {
            // Sola dönme kýsmý
            if (rb.velocity.x > 0)
            {
                // Dönme iþlemi
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }

    public void OnDeath()
    {
        rb.velocity = Vector3.zero;
        // Flyer dik bir þekilde yere düþecek
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
