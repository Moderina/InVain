using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elympics;

public class Machine : MonoBehaviour, IObservable
{
    public Canvas TasksUI;
    public ElympicsFloat progress = new ElympicsFloat();

    Slider slider;
    private Transform currentPlayer;

    void OnTriggerEnter2D(Collider2D col)
    {
        progress.Value = 0;
        if(currentPlayer == null && col.transform.tag == "Work") 
        {
            currentPlayer = col.transform;
            slider = currentPlayer.Find("Canvas").Find("Slider").GetComponent<Slider>();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform == currentPlayer) 
        {
            currentPlayer.Find("Canvas").gameObject.SetActive(true);
            if(col.transform.parent.GetComponent<Actions>().IsWorking()) 
            {
                progress.Value += Time.deltaTime;
            }
            else 
            {
                progress.Value = 0;
            }
            Debug.Log(progress);
            slider.value = progress.Value / 2;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Work") {
            currentPlayer = null;
            progress.Value = 0;
            slider.value = 0;
            col.transform.Find("Canvas").gameObject.SetActive(false);
            Debug.Log("leftAREA");
        }

    }

    
}
