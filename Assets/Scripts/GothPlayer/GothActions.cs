using UnityEngine;

public class GothActions : MonoBehaviour {
    [SerializeField] private bool isWorking = false;
    public void UpdateActions(bool work) { isWorking = work; }
    public bool IsWorking() { return isWorking; }
}