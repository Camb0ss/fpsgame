using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
//using System.Runtime.CompilerServices;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform player, destination;
    [SerializeField] private GameObject playerg; 
    [SerializeField] private float teleportTime = 3f;
    private bool canTeleport = true;

    private void OnTriggerEnter(CollectionBuilderAttribute other)
    {
        if(other.CompareTag("Player"))
        {
            if(canTeleport == true)
            {
             Invoke(nameof(Teleport), 3f);
             canTeleport = false;
            }
            else if (canTeleport != true)
            {
                return;
            }
        }
    }

    private void Teleport()
    {
     canTeleport = false;
    playerg.SetActive(false);
    player.position = destination.position;
    playerg.SetActive(true);
    }
}
