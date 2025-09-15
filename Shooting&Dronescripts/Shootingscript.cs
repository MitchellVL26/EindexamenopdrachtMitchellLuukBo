using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootingscript : MonoBehaviour
{

    public GameObject prefabToInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
                {
            GameObject obj = Instantiate(prefabToInstantiate);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
        }
    }
}
