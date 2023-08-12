using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;

public class CameraMove : ElympicsMonoBehaviour
{
    [SerializeField] Transform target;
    public Vector3 offset;
    public float dumping;

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    public void Start()
    {
        if(Elympics.IsServer) return;
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<ElympicsBehaviour>().PredictableFor == Elympics.Player)
            {
                target = player.transform;
                return;
            }
        }
    }

    void Update()
    {
        if(Elympics.IsServer) return;
        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, dumping);
    }

    public void UpdateTransform()
    {
        if(target == null) return;
        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, dumping);
    }
}
