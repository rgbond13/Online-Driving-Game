using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    MiCarro carScript;
    // Start is called before the first frame update
    void Start()
    {
        carScript = gameObject.GetComponentInParent<MiCarro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(carScript.GetSpeed(), 0, 0);
    }
}
