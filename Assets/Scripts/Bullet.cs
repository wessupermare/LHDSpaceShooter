using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
            Destroy(gameObject);

        Health h = collision.gameObject.GetComponent<Health>();
        if (h != null)
            h.TakeDamage(Damage);
    }
}
