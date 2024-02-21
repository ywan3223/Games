using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    //Components
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;
    CarLayerHandler carLayerHandler;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        //controller
        topDownCarController = GetComponentInParent<TopDownCarController>();
        carLayerHandler = GetComponentInParent<CarLayerHandler>();
        //trail renderer
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
