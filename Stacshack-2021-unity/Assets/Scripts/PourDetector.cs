using System;
using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour {
    public int pourThreshold = 45;
    public Transform origin = null;
    public GameObject streamPrefab = null;
    public Color liquid_color;

    private bool isPouring = false;
    private Stream currentStream = null;


    void Start() {
        // Set the color of the bottle 
        this.SetColor();
    }


    void Update() {
        // Check if we are pouring 
        bool pour_check = this.CalculatePourAngle() < this.pourThreshold;

        // Start or stop pouring 
        if(this.isPouring != pour_check) {
            this.isPouring = pour_check;

            if (this.isPouring) {
                this.StartPour();
            }
            else {
                this.EndPour();
            }
        }
    }

    
    void StartPour() {
        currentStream = CreateStream();
        currentStream.Begin();
        currentStream.SetColor(this.liquid_color);
    }


    void EndPour() {
        currentStream.End();
        currentStream = null;
    }


    float CalculatePourAngle() {
        return transform.forward.y * Mathf.Rad2Deg;
    }


    Stream CreateStream() {
        // Create stream gameobject 
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);

        return streamObject.GetComponent<Stream>();
    }

    public void SetColor() {
        // Get the material of the bottle logo color 
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        Material mat = mesh_renderer.materials[2];

        // Update the mats color 
        mat.SetColor("_Color", this.liquid_color);
    }
}