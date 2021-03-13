using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour {
    public float flow_distance = 2.0f;
    public float stream_speed = 1.75f;

    private LineRenderer lineRenderer = null;
    private ParticleSystem splashParticle = null;

    private Coroutine pourRoutine = null;
    private Vector3 targetPosition = Vector3.zero;
    private Color color;
    private float pour_speed;

    void Awake() {
        // Called on instantion 
        this.lineRenderer = GetComponent<LineRenderer>();
        this.splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    void Start() {
        // Make sure line render in correct place 
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void SetColor(Color color) {
        // Update the stream color 
        Material material = GetComponent<Renderer>().material;
        material.SetColor("_Color", color);

        // Update the particles color 
        var main = splashParticle.main;
        main.startColor = color;

        // Set this color 
        this.color = color;
    }

    public void Begin() {
        StartCoroutine(this.UpdateParticle());

        this.pourRoutine = StartCoroutine(this.BeginPour());
    }

    IEnumerator BeginPour() {
        while (gameObject.activeSelf) {
            // While active 
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }
    }

    public void End() {
        StopCoroutine(this.pourRoutine);
        this.pourRoutine = StartCoroutine(EndPour());
    }

    IEnumerator EndPour() {
        while (!HasReachedPosition(0, this.targetPosition)) {
            // Move both ends of stream to target poition 
            AnimateToPosition(0, this.targetPosition);
            AnimateToPosition(1, this.targetPosition);

            yield return null;
        }

        // Once starting point reached ground destroy stream 
        Destroy(gameObject);
    }

    Vector3 FindEndPoint() {
        // Init some variables 
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        // Find end position of stream 
        Physics.Raycast(ray, out hit, flow_distance);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(flow_distance);

        // Check if ray cast hit is a liquid 
        if (hit.collider && hit.transform.gameObject.GetComponentInChildren<Liquid>()) {
            // Calculate the fill percentage 
            print("hello world");

            // Add to the cups liquid 
            hit.transform.gameObject.GetComponentInChildren<Liquid>().AddLiquid(this.color, pour_speed);
        }

        return endPoint;
    }

    void MoveToPosition(int index, Vector3 target_position) {
        lineRenderer.SetPosition(index, target_position);
    }

    void AnimateToPosition(int index, Vector3 target_position) {
        Vector3 current_point = this.lineRenderer.GetPosition(index);
        Vector3 new_position = Vector3.MoveTowards(current_point, target_position, Time.deltaTime * this.stream_speed);

        lineRenderer.SetPosition(index, new_position);
    }

    bool HasReachedPosition(int index, Vector3 target_position) {
        Vector3 current_position = lineRenderer.GetPosition(index);

        return current_position == target_position;
    }

    IEnumerator UpdateParticle() {
        while (gameObject.activeSelf) {
            splashParticle.gameObject.transform.position = this.targetPosition;

            bool is_hitting = HasReachedPosition(1, this.targetPosition);
            splashParticle.gameObject.SetActive(is_hitting);

            yield return null;
        }
    }

    public void SetPourAngle(float pour_angle) {
        // Calculate the pour speed 
         this.pour_speed = (45 - pour_angle) / 120;
        
    }
}
