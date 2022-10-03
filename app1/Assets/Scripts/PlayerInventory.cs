using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCube { get; private set; }

    public UnityEvent<PlayerInventory> OnCubeCollected;

    public void CubeCollected()
    {
        NumberOfCube++;
        OnCubeCollected.Invoke(this);
    }
}
