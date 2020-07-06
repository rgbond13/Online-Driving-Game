using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterSpin : MonoBehaviour
{
    MiCarro carScript;
    Transform player;
    float angle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        carScript = gameObject.GetComponentInParent<MiCarro>();
        
        player = GetComponent<Transform>();
        // player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var multi = 1f;
        var horizInput = carScript.GetHoriz();
        
        if(horizInput != 0)
        {
            angle += horizInput * multi;
        }
        else
        {
            angle -= Mathf.Sign(angle) * (1f);
        }
        angle = Mathf.Clamp(angle, -30, 30 );
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
