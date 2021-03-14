using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Dead : MonoBehaviour {

	public GameObject player64x32;
	public GameObject player64x64;
	public GameObject playerSlim;

	public float creation_time = 0f;
	public float time_before_transition = 2f;

	public string transition_name;
	public string username;
	public float speed = 2f;
	public bool loaded_scene = false;

	private Texture2D skin;
	private string lastVerifiedUsername;

	void Start() {
		this.Refresh();
	}

	void Update() {
		if(this.creation_time >= this.time_before_transition) {
			if (this.loaded_scene) {
				// Transition scene 
				SceneManager.LoadScene(this.transition_name);
				print(this.transition_name);
				this.loaded_scene = true;
			}
		}
		else {
			this.creation_time += Time.deltaTime;
		}
	}

	public void Refresh() {
		StartCoroutine(GetTexture(this.username));
	}

	public void Die() {

		// Destroy All non active skins 
		if (!playerSlim.activeSelf) {
			Destroy(playerSlim);
		}
		else {
			// Add momentum 
			AddMomentum(playerSlim);
		}

		if (!player64x64.activeSelf) {
			Destroy(player64x64);
		}
		else {
			// Add momentum 
			AddMomentum(player64x64);
		}

		if (!player64x32.activeSelf) {
			Destroy(player64x32);
		}
		else {
			// Add momemtum 
			AddMomentum(player64x32);
		}
	}

	public void AddMomentum(GameObject player_type) {
		// Loop through all children 
		foreach(Transform child in player_type.transform) {
			GameObject child_obj = child.gameObject;

			// Check if child has rigidbody 
			if (child_obj.GetComponent<Rigidbody>() != null) {
				// Add momentum 
				child_obj.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * speed;
			}
		}
	}

	IEnumerator GetTexture(string username) {
		UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://minotar.net/skin/" + username);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError) {
			Debug.Log("Username invalid or not found!");
		}
		else {
			lastVerifiedUsername = username;
			skin = ((DownloadHandlerTexture)www.downloadHandler).texture;
			skin.filterMode = FilterMode.Point;

			if (skin.height == 64) {
				if (skin.GetPixel(50, 44).a == 0) {
					for (int i = 0; i < 6; i++) {
						playerSlim.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
					}
					playerSlim.SetActive(true);
					player64x64.SetActive(false);
					player64x32.SetActive(false);
				}
				else {
					for (int i = 0; i < 6; i++) {
						player64x64.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
					}
					player64x64.SetActive(true);
					player64x32.SetActive(false);
					playerSlim.SetActive(false);
				}
			}
			else {
				for (int i = 0; i < 6; i++) {
					player64x32.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.mainTexture = skin;
				}
				player64x32.SetActive(true);
				player64x64.SetActive(false);
				playerSlim.SetActive(false);
			}
		}

		Die();
	}

	public void Download() {
		Application.OpenURL("https://minotar.net/download/" + lastVerifiedUsername);
	}
}
