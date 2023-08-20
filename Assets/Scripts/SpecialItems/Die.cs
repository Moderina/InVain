using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class Die : ElympicsMonoBehaviour, IUpdatable
{
    public int live = 5;
    public bool kill = false;

    public void ElympicsUpdate()
    {
        if(kill)
        ElympicsDestroy(gameObject);
    }

    public void LiveTimeStart()
    {
        StartCoroutine(LiveTime());
    }
    private IEnumerator LiveTime()
    {
        yield return new WaitForSeconds(live);
        transform.SetParent(null);
        kill = true;
    }
}
