using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRadius = 5f;
    public int damageAmount = 50;
    public GameObject explosionPrefab;
    public float throwForce = 10f;
    public float throwArcHeight = 1f;

    private bool exploded = false;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = CalculateThrowVelocity();
    }

    private Vector3 CalculateThrowVelocity()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 throwDirection = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;
        float throwAngle = Mathf.Atan(throwArcHeight / Vector3.Distance(transform.position, Camera.main.transform.position));
        float throwSpeed = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) * throwArcHeight);

        return throwDirection * throwSpeed + Vector3.up * throwSpeed * Mathf.Sin(throwAngle);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!exploded && collision.gameObject.CompareTag("Floor"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        exploded = true;

        StartCoroutine(HandleExplosionPrefab());

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                BaseEnemy enemy = collider.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.HandleDamage(damageAmount);
                }
            }
        }
    }

    private IEnumerator HandleExplosionPrefab()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(explosion);
        Destroy(gameObject);
    }
}
