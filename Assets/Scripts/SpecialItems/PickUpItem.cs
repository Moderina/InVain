using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Transform point;
    public void Update()
    {
        if(point != null)
        {
            Debug.Log("setting new posiotion");
            transform.SetParent(point);
            transform.position = point.position;
            transform.Translate(0,1,0);
            point = null;
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (col.tag == "Player") 
        {
            if (col.transform.childCount > 3) return;
            if (col.transform.GetChild(0).childCount > 3) return;
            this.transform.parent.SetParent(col.transform);
            // this.transform.parent.GetComponent<Die>().LiveTimeStart();
            gameObject.SetActive(false);
            return;
        }
        if (col.tag == "Work")
        {
            if (col.transform.parent.childCount > 3) return;
            if (col.transform.parent.GetChild(0).childCount > 3) return;
            this.transform.parent.SetParent(col.transform.parent);
            // this.transform.parent.GetComponent<Die>().LiveTimeStart();
            gameObject.SetActive(false);
            return;
        }
        
    }
}
