using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class MagentaAgentCamera : Agent
{
    [Header("Specific To Magenta Agent")]
    public float timeBetweenDecisionsAtInference;
    public Camera AgentCamera;
    public GameObject SpawnLocation;
    public GameObject GoalLocation;
    public GameObject GoalCorner;
    public Material CornerOFF;
    public Material CornerON;
    public GameObject RedAgent;
    public GameObject RedAgentSpawnLocation;
    public GameObject GreenAgent;
    public GameObject GreenAgentSpawnLocation;
    public GameObject BlueAgent;
    public GameObject BlueAgentSpawnLocation;
    public Text Episodes;
    public Text Collisions;
    public Text Successes;

    private Rigidbody MyRigidbody;
    private Rigidbody RedAgentRigidbody;
    private Rigidbody BlueAgentRigidbody;
    private Rigidbody GreenAgentRigidbody;
    private bool CollisionDetected = false;
    private float m_TimeSinceDecision;
    private int EpisodeCount = -1;
    private int CollisionCount = 0;
    private int SuccessCount = 0;

    void OnCollisionEnter(Collision collision)
    {
        CollisionDetected = true;
    }

    public override void Initialize()
    {
        MyRigidbody = gameObject.GetComponent<Rigidbody>();
        RedAgentRigidbody = RedAgent.GetComponent<Rigidbody>();
        GreenAgentRigidbody = GreenAgent.GetComponent<Rigidbody>();
        BlueAgentRigidbody = BlueAgent.GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Own position
        sensor.AddObservation(gameObject.transform.position.z);
        sensor.AddObservation(gameObject.transform.position.x);
        // Relative position of the goal
        sensor.AddObservation(GoalLocation.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(GoalLocation.transform.position.x - gameObject.transform.position.x);
        /*
        // Position of peer agents
        sensor.AddObservation(RedAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(RedAgent.transform.position.x - gameObject.transform.position.x);
        sensor.AddObservation(BlueAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(BlueAgent.transform.position.x - gameObject.transform.position.x);
        sensor.AddObservation(GreenAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(GreenAgent.transform.position.x - gameObject.transform.position.x);
        */
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var Turn = Mathf.Clamp(vectorAction[0], -Mathf.PI, Mathf.PI);
        var Drive = Mathf.Clamp(vectorAction[1], 0f, 0.05f);

        gameObject.transform.Rotate(new Vector3(0, 1, 0), Turn);
        gameObject.transform.Translate(new Vector3(0, 0, Drive));

        if (CollisionDetected)
        {
            Debug.Log("Magenta Agent Collided");
            CollisionCount += 1;
            Collisions.text = CollisionCount.ToString();
            SetReward(-20f);
            EndEpisode();
        }
        if (Vector3.Distance(gameObject.transform.position, GoalLocation.transform.position) <= 1.0f)
        {
            Debug.Log("Magenta Agent Reached Goal");
            SuccessCount += 1;
            Successes.text = SuccessCount.ToString();
            StartCoroutine(GlowCorner(1f));
            SetReward(20f);
            //gameObject.SetActive(false); // During Inference
            EndEpisode(); // During Training
        }
        else
        {
            SetReward(Mathf.Pow(Vector3.Distance(gameObject.transform.position, GoalLocation.transform.position),2)*(-0.01f));
        }
    }

    public override void OnEpisodeBegin()
    {
        EpisodeCount += 1;
        Episodes.text = EpisodeCount.ToString();
        // Reset own pose and momentum
        MyRigidbody.velocity = Vector3.zero;
        MyRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.position = SpawnLocation.transform.position;
        //gameObject.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), 0.6f, Random.Range(-4.5f, 4.5f)); // Random position
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), 90);
        //gameObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(-180f, 180f)); // Random orientation

        // Reset collision flag
        CollisionDetected = false;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Mathf.PI*Input.GetAxis("Horizontal");
        actionsOut[1] = 0.05f*Input.GetAxis("Vertical");
    }

    IEnumerator GlowCorner(float time)
    {
        GoalCorner.GetComponent<MeshRenderer> ().material = CornerON;
        yield return new WaitForSeconds(time); // Wait for some time
        GoalCorner.GetComponent<MeshRenderer> ().material = CornerOFF;
    }

    public void FixedUpdate()
    {
        WaitTimeInference();
    }

    void WaitTimeInference()
    {
        if (AgentCamera != null)
        {
            AgentCamera.Render();
        }

        if (Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }
}
