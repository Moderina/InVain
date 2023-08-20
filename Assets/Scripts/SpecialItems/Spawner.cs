using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Spawner : ElympicsMonoBehaviour, IUpdatable
{
    public List<GameObject> spawnable = new List<GameObject>();
    [SerializeField] private Transform point;
    public float timer = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void ElympicsUpdate()
    {
        if (timer > 10)
        {
            if(point.childCount == 0)
            {
                int rand = Random.Range(0, spawnable.Count);
                string nazwa = "weapons/" + spawnable[rand].name;
                GameObject item = ElympicsInstantiate(nazwa, ElympicsPlayer.World);
                item.transform.SetParent(point);
            }
            timer = 0;
        }
        timer += Elympics.TickDuration;
    }
}
