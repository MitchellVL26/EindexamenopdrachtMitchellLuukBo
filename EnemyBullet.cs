using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // hoeveel schade dit projectiel doet

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Playerhealth ph = other.GetComponent<Playerhealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage); // speler neemt schade
            }

            Destroy(gameObject); // kogel verdwijnt bij hit
        }
        else if (!other.CompareTag("Enemy") && !other.isTrigger)
        {
            // raakt iets anders dat geen enemy is → ook kogel weg
            Destroy(gameObject);
        }
    }
}
