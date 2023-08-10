using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Elympics;

public class PlayersWorkInfo : ElympicsMonoBehaviour
{
    [SerializeField] private GameObject canvas;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Work") return;
        var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
        if(Elympics.Player != player.PredictableFor) return;
        canvas.SetActive(true);

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag != "Work") return;
        var player = col.transform.parent.GetComponent<ElympicsBehaviour>();
        if(Elympics.Player != player.PredictableFor) return;
        canvas.SetActive(false);
    }
}
