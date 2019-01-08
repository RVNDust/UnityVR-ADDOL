using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelBehaviour : MonoBehaviour
{
    PlayerControllerVR ci;
    GameObject holdingController = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControllerVR>())
        {
            ci = other.GetComponent<PlayerControllerVR>();
            ci.onGrabPressed += PlayerControllerVR_onGrabPressed;
            ci.onGrabReleased += PlayerControllerVR_onGrabReleased;
        }
    }

    private void PlayerControllerVR_onGrabPressed(GameObject grabbedObject, GameObject controller)
    {
        if(grabbedObject == gameObject && holdingController == null)
        {
            SpringJoint sj = gameObject.AddComponent<SpringJoint>();
            sj.connectedBody = controller.GetComponent<Rigidbody>();
            sj.spring = 50;
            sj.damper = 0;
            sj.breakForce = 20000;
            sj.breakTorque = 20000;
            holdingController = controller;
            Debug.Log("grabbbb");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControllerVR>())
        {
            ci = other.GetComponent<PlayerControllerVR>();
            ci.onGrabPressed -= PlayerControllerVR_onGrabPressed;
            ci.onGrabReleased -= PlayerControllerVR_onGrabReleased;
        }
    }

    private void PlayerControllerVR_onGrabReleased(GameObject grabbedObject, GameObject controller)
    {
        if (grabbedObject == gameObject && controller == holdingController)
        {
            SpringJoint sj = gameObject.GetComponent<SpringJoint>();
            if(sj != null)
            {
                sj.connectedBody = null;
                Destroy(sj);
                Debug.Log("ungrabbbb");
            }
        }
    }
}
