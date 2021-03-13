using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SkinUpdator))]
public class SkinUpdatorEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector(); // Draw deafult inspector vairables 

		SkinUpdator skinUpdator = (SkinUpdator)target; // Get access to the skin updator 

		if (GUILayout.Button("Refresh")) {
			skinUpdator.Refresh();
		}
	}
}
