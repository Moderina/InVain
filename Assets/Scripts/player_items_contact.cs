using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_items_contact : MonoBehaviour
{
    private BoxCollider2D player_collider;
    private GameObject key;


    void Start()
    {
        player_collider = GetComponent<BoxCollider2D>();
        key = null;
    }

    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col);

        if (col.tag == "Traps") {
            //GetComponent<responsive_movement>().enabled = false;
            //GameObject.Find("System").GetComponent<lvl_manager>().onDeath(transform.position);
            StartCoroutine(restart());
        }
        if (col.tag == "Collectables")
        {
            Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
            col.gameObject.transform.SetParent(this.transform);
            col.gameObject.transform.localPosition = new Vector3(-0.6f, 0.7f, 0);
            key = col.gameObject;
            Destroy(col);
        }
        if (col.gameObject.name == "Target" && key != null) {
            GameObject.Find("Target").transform.GetChild(0).gameObject.SetActive(false);
            //GameObject.Find("System").GetComponent<lvl_manager>().open_door = true;
            Destroy(key);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        // if (col.gameObject.name == "Target" && GameObject.Find("System").GetComponent<lvl_manager>().open_door) {
        //     if (Input.GetKeyDown(KeyCode.W)) {
        //         Debug.Log("wygrana");
        //         GetComponent<responsive_movement>().enabled = false;
        //         GameObject.Find("System").GetComponent<lvl_manager>().onWin();
        //     }
        // }
    }

        IEnumerator restart() {
            yield return new WaitForSeconds(4);

//            GetComponent<responsive_movement>().enabled = true;

        }

}
