using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animation hingehere1;
    public Animator anim;
    public Animator anim3;
  


    // Update is called once per frame
    public void move()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("door1");
            hingehere1.Play();
        }
        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("door2");
            anim.SetTrigger("Open");
        }
        if (Input.GetKey(KeyCode.T))
        {
            Debug.Log("door3");
            anim3.SetTrigger("Open");
        }
    }

}
