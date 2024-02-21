using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GetKey : MonoBehaviour
{ 
    public PlayerMovement character;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UnityEngine.Debug.Log("collide");
            Destroy(gameObject);
            character.keynumber += 1;
        }
    }

}
