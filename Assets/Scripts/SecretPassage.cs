using UnityEngine;

sealed class SecretPassage : MonoBehaviour
{

    [SerializeField] bool isBlueTeamAllowed;

	GameManager manager;

	void Start() {
		manager = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameManager> ();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GameManager.ballTag)
        {
            var body = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(-body.velocity.x, body.velocity.y, -body.velocity.z);
        }
        var robotComponent = collision.gameObject.gameObject.GetComponent<RobotShooting>();
        if (robotComponent == null)
            return;

        if (robotComponent.m_IsBlueTeam != isBlueTeamAllowed)
        {
            //XXX: This is a fairly broken and dumb way to do this. If we can find out a way to make
            // the robot bounce off of the secret passage instead of being teleported, that would be optimal.
            var movement = collision.gameObject.gameObject.GetComponent<RobotMovement>();
            var vec = movement.m_Rigidbody.velocity;
            movement.m_Rigidbody.MovePosition(movement.transform.forward * -3 * movement.m_Speed * movement.m_Speed * Time.deltaTime);
			manager.DisplayText ("Stay out of the enemy's passage!", 1.5f);
        }
    }
}
