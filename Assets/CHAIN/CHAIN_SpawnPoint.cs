using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CHAIN_SpawnPoint : MonoBehaviour
{
    //place an empty gameobject with this script (or the prefab in the package) in your scene where you want to spawn
    //the player will spawn at this point if the spawnID matches the doorID
    //to make the player spawn, attach a script and use the OnSpawn event (or edit this script) with logic to set the player position and rotation to this object

    public string DoorID; //set this to the ID of the door this spawn point is associated with
    [SerializeField] private UnityEvent _onSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CHAIN_SceneSelector.DoorID == DoorID)
        {
            _onSpawn.Invoke(); //call your logic to set the player here

            CHAIN_SceneSelector.DoorID = ""; //clear the doorID so the player doesn't spawn at the same point again
        }
    }
}
