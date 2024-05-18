using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //refrences
    public Camera camera;
    public Transform player;

    //variables
    Vector2 startingPos;
    float startingZ;

    Vector2 travel => (Vector2) camera.transform.position - startingPos;
    Vector2 parallexFactor;
    float distance => transform.position.z - player.position.z;
    float clippingPlane => (camera.transform.position.z + (distance > 0 ? camera.farClipPlane : camera.nearClipPlane));
    float factor => Mathf.Abs(distance)/clippingPlane;

    public void Start()
    {
        startingPos = transform.position;
        startingZ = transform.position.z;
    }

    public void Update()
    {
        Vector2 newPos = startingPos + travel * factor;
        transform.position = new Vector3(newPos.x, newPos.y, startingZ);
    }
}
