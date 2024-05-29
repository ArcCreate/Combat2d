using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player;
    public int PlayerNumber;
    public Vector3 offset;

    void Start()
    {
        if(PlayerNumber == 1)
        {
            // Find the first object with the TargetScript attached
            PlayerController targetScript = FindObjectOfType<PlayerController>();
            if (targetScript != null)
            {
                player = targetScript.gameObject.transform;
            }

        }
        else if(PlayerNumber == 2)
        {
            // Find the first object with the TargetScript attached
            PlayerController2 targetScript = FindObjectOfType<PlayerController2>();
            if (targetScript != null)
            {
                player = targetScript.transform;
            }
        }
    }
    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
