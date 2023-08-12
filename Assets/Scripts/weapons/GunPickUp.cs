using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player") 
        {
            this.transform.parent.SetParent(col.transform.GetChild(0));
        }
        else if (col.tag == "Work")
        {
            this.transform.parent.SetParent(col.transform.parent.GetChild(0));
        }
    }
}
