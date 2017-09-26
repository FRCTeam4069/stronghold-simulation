using UnityEngine;

sealed class Goal : MonoBehaviour
{
	GameManager manager;

	void Start() {
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

	void OnTriggerEnter (Collider other)
    {
		if (other.tag == GameManager.ballTag)
        {
			Destroy (other);
			manager.ScorePoint (other.name == GameManager.blueBallName);
		}
	}
}
