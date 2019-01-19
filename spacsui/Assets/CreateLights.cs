using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLights : MonoBehaviour
{

    public GameObject sprite;
    public SurfaceMeshesToPlanes PlaneFinder;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: Add Event Listener
        PlaneFinder.MakePlanesComplete += PlaneFinder_MakePlanesComplete;
    }

    private void PlaneFinder_MakePlanesComplete(object source, EventArgs args)
    {
        Debug.Log(string.Format("HAHAHA Finished finding planes \n{0}\n{1}", source, args));

        GameObject SurfacePlanes = GameObject.Find("SurfacePlanes");
        if (SurfacePlanes)
        {
            Debug.Log("Found SurfacePlanes");
            for (int i=0; i<SurfacePlanes.transform.childCount; i++)
            {
                Transform surface = SurfacePlanes.transform.GetChild(i);
                Vector3 center = surface.localPosition;
                for (int j = 0; j < 10; j++)
                {
                    GameObject s = Instantiate(sprite, surface);
                    s.transform.position = center;
                    s.transform.Translate(0, 0, -j/4.0f);
                }
            }
        }
        else
        {
            Debug.Log("Failed to find SurfacePlanes");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
