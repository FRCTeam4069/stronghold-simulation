using UnityEngine;

sealed class Player : MonoBehaviour {

	[SerializeField] GameObject highlightPointPrefab;
	[SerializeField] RobotShooting shooter;

	const string climbTag = "Climbable";
	const float climbRegisterDist = 1.1f, climbPerTick = 0.02f, climbFailProbPerTick = 0.005f;
	const uint climbTicks = 100, climbCooldownTicks = 150;

	GameManager manager;
	bool climbing = false;
	uint ticksClimbed = 0, currentCooldown = 0;
	Vector3 startingPosition;
    internal bool hasShot;

	void Start() {
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
		InvokeRepeating ("FireTrajectoryMarker", 0f, 0.1f);
		InvokeRepeating ("Climb", 0f, 0.02f);
		startingPosition = transform.position;
	}

	void Update() {
		if(int.Parse(manager.m_SecondsText.text) <= 20)
			CheckForClimb ();

		if (transform.position.y < -10f) {
			transform.position = startingPosition;
			manager.DisplayText ("Achievement get:\nFall out of the world!", 2f);
        } else {
            if (!hasShot){
			    manager.DisplayText("Hold A to shoot far!\nUse the dotted line as a guide.", 0.1f);
			}
        }

		print (Input.GetButton ("Climb"));
	}

	void FireTrajectoryMarker () {
		if (climbing)
			return;
		if (shooter.charging)
			shooter.Fire (highlightPointPrefab, false);
	}

	void CheckForClimb() {
		if (climbing)
			return;
		var climbables = GameObject.FindGameObjectsWithTag (climbTag);
		foreach (var climbable in climbables) {
			var dist = (transform.position - climbable.transform.position).magnitude;
			if (dist <= climbRegisterDist)
				OfferClimb ();
		}
	}

	void OfferClimb() {
		if (climbing || currentCooldown > 0)
			return;

		manager.DisplayText ("Press Y to climb!", 0f);
		if (Input.GetButton("Climb")) {
			climbing = true;
			robotEnabled = false;
		}
	}

	void Climb() {
		if (!climbing) {
			if (currentCooldown > 0) 
				currentCooldown--;
			return;
		}

		if (ticksClimbed >= climbTicks) {
			manager.DisplayText ("Success!", 3f);
			Destroy (this);
		}

		if (Random.value <= climbFailProbPerTick) {
			climbing = false;
			robotEnabled = true;
			ticksClimbed = 0;
			currentCooldown = climbCooldownTicks;
			manager.DisplayText ("Oh no! Climbing failed!", 2f);
		}

		transform.position = transform.position + (Vector3.up * climbPerTick);
		ticksClimbed++;
	}

	bool robotEnabled {
		set {
			GetComponent<RobotMovement> ().enabled = value;
			GetComponent<RobotShooting> ().enabled = value;
			GetComponent<SphereCollider> ().enabled = value;
			GetComponent<Rigidbody> ().useGravity = value;
		}
	}
}
