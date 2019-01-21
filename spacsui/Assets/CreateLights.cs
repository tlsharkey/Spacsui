using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLights : MonoBehaviour
{

    public GameObject sprite;
    public SurfaceMeshesToPlanes PlaneFinder;
    public Material LightMaterial;

    private List<GameObject> Particles = new List<GameObject>();
    private Vector3 ConvergencePoint = new Vector3(0, 3, 0);
    private System.Random rand;

    private List<Vector3> OriginalPositions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        // TODO: Add Event Listener
        PlaneFinder.MakePlanesComplete += PlaneFinder_MakePlanesComplete;
        rand = new System.Random();
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
                Vector3 scale = surface.lossyScale;
                //for (int j = 0; j < 10; j++)
                //{
                //    GameObject s = Instantiate(sprite, surface);
                //    s.transform.position = center;
                //    s.transform.Translate(0, 0, -j/4.0f);
                //    s.transform.lossyScale.Set(1, 1, 1);
                //}

                // Get Each Surface's child spheres and reset their scales
                for (int j = surface.childCount-1; j >= 0; j--)
                {
                    Vector3 newScale = new Vector3(0.1f / scale.x, 0.1f / scale.y, 0.1f / scale.z);
                    surface.GetChild(j).localScale = newScale;

                    surface.GetChild(j).Translate(rand.Next(-100,100)/100.0f, rand.Next(-100,100)/100.0f, 0.5f); // sink into wall

                    this.Particles.Add(surface.GetChild(j).gameObject);
                    this.OriginalPositions.Add(surface.GetChild(j).transform.position);
                    //surface.GetChild(j).GetComponent<Material>() = LightMaterial;

                    // Move to LightCont rol GameObject
                    surface.GetChild(j).parent = this.gameObject.transform;
                }
                //surface.GetComponent<MeshRenderer>().enabled = false;
            }

            /// Deactivate all unused stuff
            GameObject.Find("SpatialMapping").transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Failed to find SurfacePlanes");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<this.Particles.Count; i++)
        {
            
            GameObject particle = this.Particles[i];


            Vector3 scaleBy = new Vector3(0.004f, 0.004f, 0.004f);
            Vector3 target = this.ConvergencePoint; //this.Targets[i];

            Vector3 c_step = (target - particle.transform.position).normalized; //c_step towards convergence point
            Vector3 e_step = (Vector3.zero - particle.transform.position).normalized; // e_step towards exponetial origin
            c_step.Scale(scaleBy);
            e_step.Scale(scaleBy);
           

            // When approaching target, curve toward a new point.
            switch(Math.Abs((particle.transform.position - this.ConvergencePoint).magnitude))
            {
                case float n when (n < 0.1f): // FIXME: particle will be moving away from the target... Need to figure out what this number needs to be.
                    // Reset Target to New Target
                    this.Particles[i].transform.position = this.OriginalPositions[i];
                    //Debug.Log("Set to back to start");
                    break;
                case float n when (n < 4):
                    /// TODO: Fade out
                default:
                    float weightValue = Math.Abs((particle.transform.position - this.ConvergencePoint).magnitude);
                    float maxDist = Math.Abs((this.OriginalPositions[i] - this.ConvergencePoint).magnitude);
                    Vector3 c_weight = new Vector3(weightValue / maxDist, weightValue / maxDist, weightValue / maxDist);
                    Vector3 e_weight = new Vector3(1 - weightValue / maxDist, 1 - weightValue / maxDist, 1 - weightValue / maxDist);
                    
                    //c_step.Scale(c_weight);
                    //e_step.Scale(e_weight);

                    
                    //particle.transform.Translate(e_step, Space.World);
                    particle.transform.Translate(c_step, Space.World);
                    break;
            }
        }

        // Rotate All of them (via parent)
        transform.Rotate(0, 0.15f, 0);
    }
}
