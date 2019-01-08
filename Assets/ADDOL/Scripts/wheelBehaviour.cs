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
            FixedJoint sj = gameObject.AddComponent<FixedJoint>();
            sj.connectedBody = controller.GetComponent<Rigidbody>();
            sj.breakForce = Mathf.Infinity;
            sj.breakTorque = Mathf.Infinity;
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
        }
    }

    private void PlayerControllerVR_onGrabReleased(GameObject controller)
    {
        if (controller == holdingController)
        {
            FixedJoint sj = gameObject.GetComponent<FixedJoint>();
            if(sj != null)
            {
                sj.connectedBody = null;
                Destroy(sj);
                Debug.Log("ungrabbbb");
            }

            ci.onGrabReleased -= PlayerControllerVR_onGrabReleased;
            holdingController = null;
        }
    }
}
