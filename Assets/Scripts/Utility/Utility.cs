using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {


	public static GameObject getTileAtPoint(float unityX, float unityY) {
		/*
			given a point in Unity space, return a tile. If there is no game object at the point, return null
		 */

		 // TODO: what happens if you click off a tile? 
		 // 2 cases: 
		 //			* way out in nowhere
		 //						return null
		 //     * inbetween tiles
		 //						Try again moving left or right a little bit

		Vector2 origin = new Vector2(unityX, unityY);
		Vector3 direction = Vector3.forward;  // Note, the camera is at 0,0,0 looking towards positive Z
		RaycastHit hit;
		int distance = 40; // TODO: make this a var?
		int layerMask = 1 << 8;

		// TODO: Make distnace a var? 
		var result = Physics.Raycast(origin, direction, out hit, distance, layerMask);
		if (result) {
			return hit.transform.gameObject;
		}
		return null;
	}


}
