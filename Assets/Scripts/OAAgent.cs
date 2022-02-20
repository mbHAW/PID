using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using System.Linq;

public class OAAgent : Agent
{

    //public GameObject curWaypoint;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody mRigidbody;
    private CubeController cC;
    public BallPitSpawner bPS;

    public float speed;

    public override void Initialize()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localRotation;
        mRigidbody = GetComponent<Rigidbody>();
        cC = GetComponent<CubeController>();
    }

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector3(Random.Range(-39, 39), 10, Random.Range(-39, 39));
        transform.localPosition = startPosition;
        transform.localRotation = startRotation;
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.angularVelocity = Vector3.zero;
        bPS.resetBallPit();
        bPS.SpawnBallPit(40);

        //curWaypoint.transform.localPosition = new Vector3(Random.Range(-39, 39), 11.5f, Random.Range(-39, 39));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        int altitude = Mathf.RoundToInt(Input.GetAxisRaw("Altitude"));
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        

        ActionSegment<int> actions = actionsOut.DiscreteActions;
        actions[0] = horizontal >= 0 ? horizontal : 2;
        actions[1] = altitude >= 0 ? altitude : 2;
        actions[2] = vertical >= 0 ? vertical : 2;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = actions.DiscreteActions[0];
        int moveY = actions.DiscreteActions[1];
        int moveZ = actions.DiscreteActions[2];

        moveX = actions.DiscreteActions[0] <= 1 ? actions.DiscreteActions[0] : -1;
        moveY = actions.DiscreteActions[1] <= 1 ? actions.DiscreteActions[1] : -1;
        moveZ = actions.DiscreteActions[2] <= 1 ? actions.DiscreteActions[2] : -1;

        cC.targetAltitude += moveY;
        if(cC.targetAltitude >= 23)
        {
            cC.targetAltitude = 23;
        }
        else if(cC.targetAltitude <= 0)
        {
            cC.targetAltitude = 0;
        }

        mRigidbody.AddForce(new Vector3(moveX, 0, moveZ) * speed, ForceMode.Impulse);
        mRigidbody.velocity = Vector3.ClampMagnitude(mRigidbody.velocity, speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            AddReward(1f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Wall")
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

}
