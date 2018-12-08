using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMoveListener : MonoBehaviour {

	public float speed;

	private MapController mapController;
	private Camera c;

	// Use this for initialization
	void Start () {
		this.mapController = GameObject.FindWithTag("MapController").GetComponent<MapController>();
		this.c = this.transform.Find("Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.D)) {
			// Move right
			this.moveRight();
		} else if (Input.GetKey(KeyCode.A)) {
			// Move left
			this.moveLeft();
		}

		if (Input.GetKey(KeyCode.W)) {
			// Move Up
			this.moveUp();
		} else if (Input.GetKey(KeyCode.S)) {
			// Move down
			this.moveDown();
		}
	}



	private void moveRight() {
		// On the order to move right, update the positon of every tile to x += 1
		this.mapController.moveAllTiles(new Vector3(-1 * this.speed, 0, 0));
	}
	private void moveLeft() {
		this.mapController.moveAllTiles(new Vector3(1 * this.speed, 0, 0));
	}

	
	private void moveUp() {
		this.mapController.moveAllTiles(new Vector3(0, -1 * this.speed, 0));
		
	}
	private void moveDown() {
		this.mapController.moveAllTiles(new Vector3(0, 1 * this.speed, 0));
	}

}
