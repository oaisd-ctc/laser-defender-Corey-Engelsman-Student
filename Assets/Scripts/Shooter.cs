using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float BaseFiringRate = .2f;
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingrateVariance = 0f;
    [SerializeField] float minimumFiringrate = .1f;

    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;
    Audioplayer audioplayer;

    void Awake()
    {
        audioplayer = FindObjectOfType<Audioplayer>();
    }

    void Start()
    {
        if(useAI)
        {
            isFiring = true;
        }
    }
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if(!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy(instance, projectileLifetime);

            float timeToNextProjectile = Random.Range(BaseFiringRate - firingrateVariance, BaseFiringRate + firingrateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringrate, float.MaxValue);
            audioplayer.PlayShootingClip();
            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
