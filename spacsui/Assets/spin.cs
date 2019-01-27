using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    [Range(0,1)]
    public float speed;
    public GameObject about;
    // Start is called before the first frame update
    void Start()
    {
        if (!about)
        {
            about = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(about.transform.position, Vector3.up, speed);
    }
}
