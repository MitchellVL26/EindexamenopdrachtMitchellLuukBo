using UnityEngine;
using UnityEngine.SceneManagement;

public class Bulletscript : MonoBehaviour
{
    public float speed = 50f;
    public float lifeTime = 15f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            Animator anim = collision.transform.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("Death"); // Must match your Animator state name
            }

            Collider col = collision.transform.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Destroy(gameObject);              // kill bullet
            

            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                SceneManager.LoadSceneAsync(0);
            }
        }
    }
}
