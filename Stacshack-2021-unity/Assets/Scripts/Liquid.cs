using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public GameObject cup;
    public float fill_amount = 0.5f;

    float max_height = -1f;
    float radious = 0.25f;

    float wait_time = 0.01f;
    float timer = 0f; 

    Vector4[] top_verticies;

    void Start(){
        // Get the cup height 
        Mesh parent_mesh = this.cup.GetComponent<MeshFilter>().mesh;

        for(var i = 0; i < parent_mesh.vertices.Length; i++) {
            if(max_height == -1 || parent_mesh.vertices[i].y > max_height) {
                this.max_height = parent_mesh.vertices[i].y;
            }
        }

        this.max_height = this.max_height - 1;

        // Get the top verticies for the liquid  
        Mesh mesh = this.meshFilter.mesh;

        var j = 0;
        Vector4[] vertices = new Vector4[mesh.vertices.Length / 2]; 
        

        for(var i = 0; i < mesh.vertices.Length; i++) {
            if(mesh.vertices[i].y == 1) {
                vertices[j] = new Vector4(
                    mesh.vertices[i].x,
                    mesh.vertices[i].y,
                    mesh.vertices[i].z,
                    i
                );

                j++;
            }
        }

        this.top_verticies = vertices;

        // Update verticies 
        this.UpdateVerticies();
    }

    void Update() {
        // Check if liduid is visible
        if (this.fill_amount <= 0.01) {
            this.meshRenderer.enabled = false;
        }
        else {
            if (this.meshRenderer.enabled == false) {
                this.meshRenderer.enabled = true;
            }
            // Check for updates 
            this.UpdateVerticies();
        }
    }


    public void AddLiquid(Color color, float fill_speed) {
        // Check last time of update 
        this.timer += Time.deltaTime;

        // Add some liquid to this cup 
        if(this.fill_amount < 1f && this.timer > this.wait_time) {
            // Normolise fill speed 
            fill_speed *= 0.02f;

            // Update the amount of liquid 
            this.fill_amount += fill_speed;
            this.timer = 0f;

            // Update the color of the liquid 
            Material mat = gameObject.GetComponent<Renderer>().material;

            Color current_col = mat.color;
            Color new_col = BlendColors(current_col, color, fill_speed);

            mat.SetColor("_Color", new_col);


        }
    }

    Color BlendColors(Color current_col, Color new_color, float new_liquid) {
        // Calculate the percentages for each color 
        float new_percentage = (new_liquid) / this.fill_amount;

        return Color.Lerp(current_col, new_color, new_percentage);
    }

    void UpdateVerticies() {
        // Get mesh verticies 
        Vector3[] verts = this.meshFilter.mesh.vertices;

        // Set verticies to fill height 
        for(int i = 0; i < this.top_verticies.Length; i++) {
            this.top_verticies[i].y = this.max_height * this.fill_amount;
        }

        // Slosh the cup 
        this.Slosh();

        // Update the mesh 
        for(int i = 0; i < this.top_verticies.Length; i++) {
            verts[(int)this.top_verticies[i].w] = this.top_verticies[i];
        }

        this.meshFilter.mesh.vertices = verts;
    }

    void Slosh() {
        // Get the parents transform 
        Transform cup_transform = this.cup.GetComponent<Transform>();

    }
}
