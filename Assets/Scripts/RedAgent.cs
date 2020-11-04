using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;

public class RedAgent : Agent
{
    [Header("Specific To Red Agent")]
    public GameObject SpawnLocation;
    public GameObject GoalLocation;
    public GameObject GoalCorner;
    public Material CornerOFF;
    public Material CornerON;
    public GameObject GreenAgent;
    public GameObject GreenAgentSpawnLocation;
    public GameObject BlueAgent;
    public GameObject BlueAgentSpawnLocation;
    public GameObject MagentaAgent;
    public GameObject MagentaAgentSpawnLocation;
    public Text Episodes;
    public Text Collisions;
    public Text Successes;

    private Rigidbody MyRigidbody;
    private Rigidbody GreenAgentRigidbody;
    private Rigidbody BlueAgentRigidbody;
    private Rigidbody MagentaAgentRigidbody;
    private bool CollisionDetected = false;
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
        GreenAgentRigidbody = GreenAgent.GetComponent<Rigidbody>();
        BlueAgentRigidbody = BlueAgent.GetComponent<Rigidbody>();
        MagentaAgentRigidbody = MagentaAgent.GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Own position
        sensor.AddObservation(gameObject.transform.position.z);
        sensor.AddObservation(gameObject.transform.position.x);
        // Relative position of the goal
        sensor.AddObservation(GoalLocation.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(GoalLocation.transform.position.x - gameObject.transform.position.x);
        // Position of peer agents
        sensor.AddObservation(GreenAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(GreenAgent.transform.position.x - gameObject.transform.position.x);
        sensor.AddObservation(BlueAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(BlueAgent.transform.position.x - gameObject.transform.position.x);
        sensor.AddObservation(MagentaAgent.transform.position.z - gameObject.transform.position.z);
        sensor.AddObservation(MagentaAgent.transform.position.x - gameObject.transform.position.x);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var Turn = Mathf.Clamp(vectorAction[0], -Mathf.PI, Mathf.PI);
        var Drive = Mathf.Clamp(vectorAction[1], 0f, 0.05f);

        gameObject.transform.Rotate(new Vector3(0, 1, 0), Turn);
        gameObject.transform.Translate(new Vector3(0, 0, Drive));

        if (CollisionDetected)
        {
            Debug.Log("Red Agent Collided");
            CollisionCount += 1;
            Collisions.text = CollisionCount.ToString();
            SetReward(-20f);
            EndEpisode();
        }
        if (Vector3.Distance(gameObject.transform.position, GoalLocation.transform.position) <= 1.0f)
        {
            Debug.Log("Red Agent Reached Goal");
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


        // GO-TO-GOAL WITH COLLISION AVOIDANCE
        // Reset own pose and momentum
        MyRigidbody.velocity = Vector3.zero;
        MyRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.position = SpawnLocation.transform.position;
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);


        /*
        // ANTIPODAL EXCHANGE
        // Reset own pose and momentum
        MyRigidbody.velocity = Vector3.zero;
        MyRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.position = SpawnLocation.transform.position;
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), 45);
        */

        /*
        // RANDOM INITIALIZATION
        // Reset own pose and momentum
        MyRigidbody.velocity = Vector3.zero;
        MyRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.position = new Vector3(Random.Range(-4.0f, -1.0f), 0.6f, Random.Range(-4.0f, -1.0f)); // Random position
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(0f, 90f)); // Random orientation
        */


        /*
        // Reset other agents
        GreenAgentRigidbody.velocity = Vector3.zero;
        GreenAgentRigidbody.angularVelocity = Vector3.zero;
        GreenAgent.transform.position = GreenAgentSpawnLocation.transform.position;
        GreenAgent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        GreenAgent.transform.Rotate(new Vector3(0, 1, 0), -90);

        BlueAgentRigidbody.velocity = Vector3.zero;
        BlueAgentRigidbody.angularVelocity = Vector3.zero;
        BlueAgent.transform.position = BlueAgentSpawnLocation.transform.position;
        BlueAgent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        BlueAgent.transform.Rotate(new Vector3(0, 1, 0), 180);

        MagentaAgentRigidbody.velocity = Vector3.zero;
        MagentaAgentRigidbody.angularVelocity = Vector3.zero;
        MagentaAgent.transform.position = MagentaAgentSpawnLocation.transform.position;
        MagentaAgent.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        MagentaAgent.transform.Rotate(new Vector3(0, 1, 0), 90);
        */

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
}
