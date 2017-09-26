using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

sealed class GameManager : MonoBehaviour
{
    const int m_SecondsInARound = 120;
	const int timeStartClimbing = 25;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;

    public Text m_MessageText;
    public Text m_SecondsText;

    public GameObject m_RobotPrefab;
    public RobotManager[] m_Robots;

    internal const string blueBallName = "BlueBall", ballTag = "Ball";

	const string clear = "ClearText";

    WaitForSeconds m_StartWait;
    int m_SecondsRemaining = 120;
    float m_TimePassed = 0.0f;
	string defaultText = string.Empty;

    uint bluePoints = 0, redPoints = 0;
    void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        StartCoroutine(GameLoop());
    }
    void SpawnAllRobots()
    {


        // For all the Robots...
        for (int i = 0; i < m_Robots.Length; i++)
        {
            // ... create them, set their player number and references needed for control.
            m_Robots[i].m_Instance =
                Instantiate(m_RobotPrefab, m_Robots[i].m_SpawnPoint.position, m_Robots[i].m_SpawnPoint.rotation) as GameObject;
            m_Robots[i].m_PlayerNumber = i + 1;
            m_Robots[i].Setup();
        }


    }
    void SetCameraTargets()
    {

    }
    IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
		RoundEnding();
    }
    IEnumerator RoundStarting()
    {

        m_MessageText.text = "STRONGHOLD\nYou Have " + m_SecondsInARound + " Seconds!";
        m_TimePassed = 0.0f;
        yield return m_StartWait;
    }

    IEnumerator RoundPlaying()
    {
        // While there is not one Robot left...
		while (m_TimePassed < (float) m_SecondsInARound)
        {
			// Clear the text from the screen.
			m_MessageText.text = defaultText;

            m_TimePassed += Time.deltaTime;
            int timeleft = m_SecondsInARound - (int)m_TimePassed;

			if (timeleft == timeStartClimbing)
				m_MessageText.text = "25 seconds left! Start climbing!";
            m_SecondsText.text = "" + timeleft;
            // ... return on the next frame.
            yield return null;
        }

        yield return null;
    }


    void RoundEnding()
    {
		var player = GameObject.FindGameObjectWithTag ("Player");
		Destroy (player.GetComponent<RobotShooting> ());
		Destroy (player.GetComponent<RobotMovement> ());
		m_MessageText.text = "Game Over!\nYour final score: " + bluePoints;
		Invoke ("QuitGame", m_EndDelay);
    }

	void QuitGame() {
		SceneManager.LoadScene (Application.loadedLevel);
	}

	internal void DisplayText(string text, float cooldown) {
		defaultText = text;
		Invoke (clear, cooldown);
	}

	void ClearText() {
		defaultText = string.Empty;
	}

    public void ScorePoint(bool isBlueTeam)
    {
        if (isBlueTeam)
            bluePoints++;
        else
            redPoints++;
		defaultText = "Blue score: " + bluePoints + "\nRed score: " + redPoints;
		Invoke (clear, 1.5f);
    }
}
