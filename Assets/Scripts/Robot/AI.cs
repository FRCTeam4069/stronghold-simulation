using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

	[SerializeField] Transform goal;

	const float groundHeight = -2.490475f;

	List<Vector3> possibleWaypoints = new List<Vector3> ();
	NavMeshAgent agent;

	void Start() {
		W (-5.7280f, 6.0626f);
		W (-0.0402f, -6.8097f);
		W (1.4911f, 0.2069f);
		W (-0.0723f, 7.7578f);
		agent = GetComponent<NavMeshAgent> ();
		agent.destination = possibleWaypoints [0];
	}

	void Update() {
		var delta = transform.position - agent.destination;
		if (delta.sqrMagnitude < 0.3) {
			var index = Random.Range (0, possibleWaypoints.Count - 1);
			agent.destination = possibleWaypoints [index];
		}
	}

	void W(float x, float z) {
		possibleWaypoints.Add (new Vector3 (x, groundHeight, z));
	}
}
