using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    private Transform player;

    public float zOffset = 10;
    public float yOffset = 10;

    void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerScript>().transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z + zOffset);
    }
}
