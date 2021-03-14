using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Dead))]
public class DeadEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector(); // Draw deafult inspector vairables 

		Dead skinUpdator = (Dead)target; // Get access to the skin updator 

		if (GUILayout.Button("Refresh")) {
			skinUpdator.Refresh();
		}
	}
}