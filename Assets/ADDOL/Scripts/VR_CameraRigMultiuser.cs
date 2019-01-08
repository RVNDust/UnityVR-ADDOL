using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

// Make the SteamVR available in multiplayer by deactivate script for UserOther
// Supports version # SteamVR Unity Plugin - v2.0.1
public class VR_CameraRigMultiuser : NetworkBehaviour
{
    // Reference to SteamController
    public GameObject SteamVRLeft, SteamVRRight, SteamVRCamera;
    public GameObject UserOtherLeftHandModel, UserOtherRightHandModel;
    private GameObject goFreeLookCameraRig;

    void Start()
    {
        updateGoFreeLookCameraRig();
        steamVRactivation();
        if (isLocalPlayer)
        {
            gameObject.tag = "VRLocalPlayer";
        }
    }

    // Deactivate de FreeLookCameraRig since we are uisng the HTC version
    // Execute only in client side
    protected void updateGoFreeLookCameraRig()
    {
        // Client execution ONLY LOCAL
        if (!isLocalPlayer) return;
        goFreeLookCameraRig = null;
        try
        {
            // Get the Camera to set as the follow camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            goFreeLookCameraRig.SetActive(false);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    // If we are the client who is using the HTC, activate component of SteamVR in the client using it
    // If we are not the client using this specific HTC, deactivate some scripts
    protected void steamVRactivation()
    {
        // Client exceution for ALL
        if (!isLocalPlayer)
        {
            // Left && right activatino if userMe, Deactivate if UserOther
            // Left && Right SteamVR_Rendermodel activationn if USerme, deactivate if UserOther
            Destroy(SteamVRLeft.GetComponent<SteamVR_Behaviour_Pose>());
            Destroy(SteamVRLeft.GetComponentInChildren<SteamVR_RenderModel>());

            Destroy(SteamVRRight.GetComponent<SteamVR_Behaviour_Pose>());
            Destroy(SteamVRRight.GetComponentInChildren<SteamVR_RenderModel>());

            //Destroy(SteamVRRight.GetComponent<ControllerInput>());
            //Destroy(SteamVRLeft.GetComponent<ControllerInput>());

            // Camera activation if userme, deactivate if userother
            Destroy(GetComponentInChildren<Camera>());
        }
        else
        {
            Destroy(UserOtherLeftHandModel);
            Destroy(UserOtherRightHandModel);
        }
    }

}