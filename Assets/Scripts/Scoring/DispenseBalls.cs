using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenseBalls : MonoBehaviour {

	[SerializeField] GameObject ballPrefab;
	[SerializeField] Vector3 relativeSpawnPos, spawnVelocity;

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Player") {
			var ball = Instantiate (ballPrefab);
			ball.transform.position = transform.position + relativeSpawnPos;
			ball.GetComponent<Rigidbody> ().velocity = spawnVelocity;
		}
	}
}
