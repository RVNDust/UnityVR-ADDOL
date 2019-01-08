using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonControllerMultiUser : NetworkBehaviour {

    public Material PlayerLocalMat;
    /// <summary>
    /// Represents the GameObject on which to change the color for the local player
    /// </summary>
    public GameObject GameObjectLocalPlayerColor;

    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;

    /// <summary>
    /// The Transform from which the snow ball is spawned
    /// </summary>
    [SerializeField] Transform snowballSpawner;
    /// <summary>
    /// The prefab to create when spawning
    /// </summary>
    [SerializeField] GameObject SnowballPrefab;

    // Use to configure the throw ball feature
    [Range(0.2f, 100.0f)] public float MinSpeed;
    [Range(0.2f, 100.0f)] public float MaxSpeed;
    [Range(0.2f, 5.0f)] public float MaxSpeedForPressDuration = 1.5f;
    private float pressDuration = 0;



    // Use this for initialization
    void Start()
    {
        Debug.Log("isLocalPlayer:" + isLocalPlayer);
        updateGoFreeLookCameraRig();
        followLocalPlayer();
        activateLocalPlayer();
    }

    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        try
        {
            // Get the Camera to set as the followed camera
            //...
            goFreeLookCameraRig = GameObject.FindGameObjectWithTag("CameraRig");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void followLocalPlayer()
    {
        if (isLocalPlayer)
        {
            if (goFreeLookCameraRig != null)
            {
                // find Avatar EthanHips
                Transform transformFollow = null;
                transformFollow = transform.Find("EthanSkeleton/EthanHips");
                // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
            }
        }
    }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        //...
        gameObject.GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            try
            {
                // Change the material of the Ethan Glasses
                GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
            }
            catch (System.Exception)
            {

            }
        }
        else
        {
            
        }
    }

    #region Snwoball Spawn
    // Update is called once per frame
    void Update()
    {
        // Don't do anything if we are not the UserMe isLocalPlayer
        //...
        if(isLocalPlayer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // Start Loading time when fire is pressed
                pressDuration = 0.0f;
            }
            else if (Input.GetButton("Fire1"))
            {
                // count the time the Fire1 is pressed
                //pressDuration += ???; 
                pressDuration += Time.deltaTime;
            }

            else if (Input.GetButtonUp("Fire1"))
            {
                // When releasing Fire1, spawn the ball
                // Define the initial speed of the Snowball between MinSpeed and MaxSpeed according to the duration the button is pressed
                var speed = MinSpeed;
                if (pressDuration > MaxSpeedForPressDuration) { speed = MaxSpeed; }
                else { speed = MinSpeed + (pressDuration / MaxSpeedForPressDuration) * (MaxSpeed - MinSpeed); } //... update with the right value
                Debug.Log(string.Format("time {0:F2} <  {1} => speed {2} < {3} < {4}", pressDuration, MaxSpeedForPressDuration, MinSpeed, speed, MaxSpeed));
                CmdThrowBall(speed);
            }
        }
    }

    [Command]
    void CmdThrowBall(float speed)
    {
        GameObject snowball = null;
        // Instantiate the Snowball from the Snowball Prefab at the position of the Spawner
        snowball = GameObject.Instantiate(SnowballPrefab);

        // Set velocity to the bulletRigidBody direction and speed
        snowball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, speed);

        // Spawn the bullet to update in all client. Use the NetworkServer class to do it
        snowball.transform.position = snowballSpawner.transform.position;

        // Destroy the Snowball after 5 seconds
        StartCoroutine(destroyAfterCountdown(snowball, 5.0f));

        NetworkServer.Spawn(snowball);
    }
    #endregion

    IEnumerator destroyAfterCountdown(GameObject goToDestroy, float timer)
    {
        yield return new WaitForSeconds(timer);
        NetworkServer.Destroy(goToDestroy);
    }

}
