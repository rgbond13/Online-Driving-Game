using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speed : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI speedDis;
    [SerializeField]
    TextMeshProUGUI lapCount;

    MiCarro carScript;

    // Start is called before the first frame update
    void Start()
    {
        //speedDis = GetComponent<TextMeshProUGUI>();
        carScript = GetComponentInParent<MiCarro>();
    }

    // Update is called once per frame
    void Update()
    {
        speedDis.SetText("{0} KPH", carScript.GetSpeed());
        lapCount.SetText("Lap {0}/3", carScript.GetLaps());
    }
}
