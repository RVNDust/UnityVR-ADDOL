using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerVR : MonoBehaviour
{

    public delegate void OnGrabPressed(GameObject Controller);
    public static event OnGrabPressed onGrabPressed;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (onGrabPressed != null)
        {
            onGrabPressed(gameObject);
        }
    }
}
