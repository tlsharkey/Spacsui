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
    public GameObject ForceField;

    private List<ParticleSystem> Particles = new List<ParticleSystem>();
    private Vector3 ConvergencePoint = new Vector3(0, 5, 0);
    private System.Random rand;

    private GameObject Floor;

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

                // Set Floor to have a Particle System Generator
                if (!surface.gameObject.GetComponent<Renderer>().material.name.Equals("Floor (Instance)")) { continue; }
                Floor = surface.gameObject;

                // Get Plane Size
                float width = surface.localScale.x;
                float height = surface.localScale.y;
                float radius = Math.Max(width, height);
                
                // Create and position Particle System 
                GameObject p = Instantiate(sprite, surface);
                p.transform.Rotate(90, 0, 0);
                p.transform.Translate(0, 0.1f, 0, Space.World);

                // Set Partcle System to same size as plane
                ParticleSystem ps = p.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                var shape = ps.shape;
                shape.radius = radius/2;

                // Start
                ps.Play();
            }

            /// Deactivate all unused stuff
            GameObject.Find("SpatialMapping").transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Failed to find SurfacePlanes");
        }
        
    }
}
