using UnityEngine;
using UnityEngine.UI;

sealed class RobotShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;                        // Used to identify the different players.
	public bool m_IsBlueTeam;                             // Is the robot on the blue team or the red team?

	[SerializeField] GameObject m_Ball;                   // Prefab of the ball.
    [SerializeField] Transform m_FireTransform;           // A child of the Robot where the balls are spawned.
    [SerializeField] AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    [SerializeField] float m_MinLaunchForce;              // The force given to the ball if the fire button is not held.
    [SerializeField] float m_MaxLaunchForce;              // The force given to the ball if the fire button is held for the max charge time.
    [SerializeField] float m_MaxChargeTime;               // How long the ball can charge for before it is fired at max force.

    string m_FireButton;                                  // The input axis that is used for launching balls.
    float m_CurrentLaunchForce;                           // The force that will be given to the ball when the fire button is released.
    float m_ChargeSpeed = 1;                                  // How fast the launch force increases, based on the max charge time.
    
	bool m_PossessesBall = true;                          // Whether or not the robot is carrying a ball.
	bool m_DesensitizedToPickup = false;                  // Used to prevent the robot from picking up the ball during the brief collision that happens when it fires the ball.
	bool m_Fired = true;                                  // Whether or not the ball has been launched with this button press.


    void OnEnable()
    {
        // When the Robot is turned on, reset the launch force and the UI
        m_CurrentLaunchForce = m_MinLaunchForce;
    }


    void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire";

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    void Update()
    {
      
        // If the max force has been exceeded and the ball hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... use the max force and launch the ball.
            m_CurrentLaunchForce = m_MaxLaunchForce;
			Fire(m_Ball, true);
        }
        // Otherwise, if the fire button has just started being pressed...
        if (Input.GetButtonDown(m_FireButton) && m_PossessesBall)
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
        // Otherwise, if the fire button is being held and the ball hasn't been launched yet...
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
			print (m_CurrentLaunchForce);
            // Increment the launch force and update the slider.
			m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;            
        }
        // Otherwise, if the fire button is released and the ball hasn't been launched yet...
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            // ... launch the ball.
			Fire(m_Ball, true);
        }
    }


	internal void Fire(GameObject prefab, bool firingBall)
    {
        // Create an instance of the prefab.
        var ballInstance = Instantiate(prefab);
		ballInstance.transform.position = m_FireTransform.position;

        // Set the ball's velocity to the launch force in the fire position's forward direction.
		ballInstance.GetComponent<Rigidbody>().velocity = firingVelocity;

        // Make sure the player object knows it has shot
        GetComponent<Player>().hasShot = true;

		// If the robot is on the blue team, set the name of the ball it is firing accordingly, so it can be recognized by the goals.
		if (m_IsBlueTeam) ballInstance.name = GameManager.blueBallName;

		// Set the fired flag so only Fire is only called once.
		if (firingBall) {
			if (!m_PossessesBall)
				return;
			m_Fired = true;
			m_PossessesBall = false;
			m_DesensitizedToPickup = true;

			// Reset the launch force.  This is a precaution in case of missing button events.
			m_CurrentLaunchForce = m_MinLaunchForce;
		}
    }

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == GameManager.ballTag && !m_PossessesBall)
		{
			if (m_DesensitizedToPickup)
			{
				m_DesensitizedToPickup = false;
			}
			else
			{
				Destroy(collision.gameObject);
				m_PossessesBall = true;
			}
		}
	}

	internal Vector3 firingVelocity {
		get {
			return m_CurrentLaunchForce * m_FireTransform.forward;
		}
	}

	internal bool charging {
		get {
			return !m_Fired && m_PossessesBall;
		}
	}
}
