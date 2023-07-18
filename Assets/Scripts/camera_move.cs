using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    private Transform _playerList;  
    private Vector3 dead;

    private bool gameover = false;
    private float left, right;   
    private float cam_size, max_distance;    

    void Start() {
        _playerList = GameObject.Find("Players").transform;
        cam_size = this.GetComponent<Camera>().orthographicSize;
    }


    void Update()
    {
        if (!gameover) {
            max_distance = 0;
            left = 9999;
            right = -9999;
            foreach (Transform player in _playerList) {
                left = MathF.Min(left, player.transform.position.x);
                right = MathF.Max(right, player.transform.position.x);
            }
            // var height = this.transform.position.y - 1;
            // if (Mathf.Abs(height) > 0.01f)
            //     this.transform.position = new Vector3((left+right)/2,
            //      this.transform.position.y + height/100, 
            //      -1);
            // else
                this.transform.position = new Vector3((left+right)/2, 1, -1);

            if(this.GetComponent<Camera>().orthographicSize < 4f) {
                cam_size = this.GetComponent<Camera>().orthographicSize += 0.01f;
            }

            max_distance = MathF.Abs(left - right);
            if (cam_size * 1.7f * 2 - max_distance < 0.1f && cam_size + 0.01f < 7) {
                cam_size = this.GetComponent<Camera>().orthographicSize += 0.01f;
            }
            else if (cam_size * 1.7f * 2 - max_distance > 0.1f * 2 && cam_size + 0.01f > 4f) {
                cam_size = this.GetComponent<Camera>().orthographicSize -= 0.01f;
            }
        }
        else {
            float step = Vector3.Distance(transform.position, dead)/10;
            cam_size = this.GetComponent<Camera>().orthographicSize -= cam_size>2 ? 0.05f: 0f;
            this.transform.position = Vector3.MoveTowards(transform.position, dead, step);
        }
    }

    public void DeadCameraFocus(Vector3 go) {
        if (go.z == 9876) {
            gameover = false;
            return;
        }
        dead = go;
        dead.z = -10;
        gameover = true;
    }
}
