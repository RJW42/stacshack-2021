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
