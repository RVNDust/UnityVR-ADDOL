using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCommand : NetworkBehaviour
{

    [SerializeField] GameObject FirePrefab;
    [SerializeField] Transform fireSpawner;

    [Command]
    public void CmdStartFire()
    {
        GameObject fire = null;
        // Instantiate the Snowball from the Snowball Prefab at the position of the Spawner
        fire = GameObject.Instantiate(FirePrefab);

        // Spawn the bullet to update in all client. Use the NetworkServer class to do it
        fire.transform.position = fireSpawner.transform.position;

        NetworkServer.Spawn(fire);
    }
}
