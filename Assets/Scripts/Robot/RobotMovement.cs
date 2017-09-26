using UnityEngine;

sealed class RobotMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;      // Used to identify which Robot belongs to which player.  This is set by this Robot's manager.

    [SerializeField] public float m_Speed;     // How fast the Robot moves forward and back.
    [SerializeField] float m_TurnSpeed; // How fast the Robot turns in degrees per second.
 
    string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    string m_TurnAxisName;              // The name of the input axis for turning.
    public Rigidbody m_Rigidbody;              // Reference used to move the Robot.
    float m_MovementInputValue;         // The current value of the movement input.
    float m_TurnInputValue;             // The current value of the turn input.
   

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void OnEnable()
    {
        // When the Robot is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;           
    }


    void OnDisable()
    {
        // When the Robot is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;

      
    }


    void Start()
    {
        // The axes names are based on player number.
		m_MovementAxisName = "Vertical";// + m_PlayerNumber;
		m_TurnAxisName = "Horizontal";// + m_PlayerNumber;

      
    }


    void Update()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

      
    }
  
    void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
    }


    void Move()
    {
        // Create a vector in the direction the Robot is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}
