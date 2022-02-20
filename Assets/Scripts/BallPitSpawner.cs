using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPitSpawner : MonoBehaviour
{

    //Skript um zufällig Bälle in der Umgebung zu Spawnen und zu resetten

    public GameObject MediumBall;
    public GameObject BigBall;

    private Transform parentTransform;
    private List<GameObject> ballPit;

    private void Start()
    {
        ballPit = new List<GameObject>();
        parentTransform = GetComponent<Transform>();
    }

    public void SpawnBallPit(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject randomBallSize = (Random.Range(0, 2) < 1) ? MediumBall : BigBall;
            ballPit.Add(Instantiate(randomBallSize, transform.position + new Vector3(randomStepRange(-35,36,5), randomStepRange(5, 21, 5), randomStepRange(-35, 36, 5)),Quaternion.identity, parentTransform));
        }
    }

    public void resetBallPit()
    {
        foreach(GameObject ball in ballPit)
        {
            Destroy(ball);
        }
        ballPit.Clear();
    }

    private int randomStepRange(int lowerBound, int upperBound, int step)
    {
        int randomValue = Random.Range(lowerBound / step,upperBound / step) * step;
        return randomValue;
    }
}
