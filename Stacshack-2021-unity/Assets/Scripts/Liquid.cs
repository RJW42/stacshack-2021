using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public MeshFilter meshFilter;
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
        this.UpdateVerticies();    
    }


    public void AddLiquid(Color color) {
        // Check last time of update 
        this.timer += Time.deltaTime;

        // Add some liquid to this cup 
        if(this.fill_amount < 1f && this.timer > this.wait_time) {
            // Update the amount of liquid 
            this.fill_amount += 0.01f;
            this.timer = 0f;

            // Update the color of the liquid 
            Material mat = gameObject.GetComponent<Renderer>().material;

            Color current_col = mat.color;
            Color new_col = BlendColors(current_col, color, 0.01f);

            mat.SetColor("_Color", new_col);


        }
    }

    Color BlendColors(Color current_col, Color new_color, float new_liquid) {
        // Calculate the percentages for each color 
        float old_percentage = (this.fill_amount - new_liquid) / this.fill_amount;
        float new_percentage = (new_liquid) / this.fill_amount;

        // Calculate new color value 
        float r = (new_color.r * new_percentage) + (current_col.r * old_percentage);
        float g = (new_color.g * new_percentage) + (current_col.g * old_percentage);
        float b = (new_color.b * new_percentage) + (current_col.b * old_percentage);
        float a = (new_color.a);

        return new Color(r, g, b, a);
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

        // Get the inverse rotation of the cup 
        print(cup_transform.rotation.eulerAngles);

    }
}
