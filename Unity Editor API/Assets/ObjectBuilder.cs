using UnityEngine;
using System.Collections;
using System.Numerics;

public class ObjectBuilder : MonoBehaviour
{
    public GameObject obj;
    public UnityEngine.Vector3 spawnPoint;


    public void BuildObject()
    {
        Instantiate(obj, spawnPoint, UnityEngine.Quaternion.identity);
    }
}