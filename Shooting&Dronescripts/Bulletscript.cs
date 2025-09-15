using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Bulletscript : MonoBehaviour
{
    public float speed;


    public enum BulletType
    {
        BULLET,
        ROCKET,
        LAZORPEWPEW
    }

    public BulletType type;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 15.0f);
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;

       
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.tag == "Enemy")
        {
           
            Destroy(collision.gameObject);
          
        }
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            
            SceneManager.LoadSceneAsync(0);
        }


    }
    private void OnTriggerEnter(Collider other)
    {

    }
    public void DoDamage(float damage)
    {

    }


}
