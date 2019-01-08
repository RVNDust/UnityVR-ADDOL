using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControllerVR : MonoBehaviour
{

    SteamVR_Input_Sources inputSource;
    Rigidbody rb;
    bool canGrab = false;
    List<GameObject> grabbableObjects = new List<GameObject>();

    public bool isGrabbingPinch;

    public delegate void OnGrabPressed(GameObject grabbedObject, GameObject controller);
    public event OnGrabPressed onGrabPressed;

    public delegate void OnGrabReleased(GameObject grabbedObject, GameObject controller);
    public event OnGrabReleased onGrabReleased;


    void Awake()
    {
        inputSource = GetComponent<SteamVR_Behaviour_Pose>().inputSource;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(inputSource))
        {
            if (canGrab && onGrabPressed != null)
            {
                onGrabPressed(grabbableObjects[grabbableObjects.Count-1], gameObject);
            }
        }
        else if (SteamVR_Input._default.inActions.GrabGrip.GetStateUp(inputSource))
        {
            if (onGrabReleased != null)
            {
                onGrabReleased(grabbableObjects[grabbableObjects.Count - 1], gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Grabbable"))
        {
            grabbableObjects.Add(other.gameObject);
        }
        if (grabbableObjects.Count > 0)
            canGrab = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            grabbableObjects.Remove(other.gameObject);
        }
        if (grabbableObjects.Count <= 0)
            canGrab = false;
    }
}
