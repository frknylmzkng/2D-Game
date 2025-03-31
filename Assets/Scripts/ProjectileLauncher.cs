using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Oku f�rlatmak i�in ayarlar
public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;

        // Okun gitti�i y�n� karakterin o anki do�rultusu y�n�nde yapt�m
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1,
            origScale.y, origScale.z
            );
    }
}
