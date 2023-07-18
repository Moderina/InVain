using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    [SerializeField] private bool isWorking = false;

    public void UpdateActions(bool work) 
    {
        isWorking = work;
    }

    public bool isPlayerWorking() {
        return isWorking;
    }
}
