using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float targetAltitude = 10;
    public PIDController altitudePID;

    float maxThrust;
    float maxSpeed;

    private WaypointManager wM;
    CubeThruster cT;
    CubeMover cM;
    OAAgent oAAgent;

    void Start()
    {
        cT = GetComponent<CubeThruster>();
        cM = GetComponent<CubeMover>();
        wM = GetComponent<WaypointManager>();
        oAAgent = GetComponent<OAAgent>();

        maxThrust = cT.maxThrust;
        maxSpeed = cM.maxSpeed;
        oAAgent.speed = maxSpeed;
    }

    void Update()
    {
        //Altitute Control ----------------------------------------------------
        float curAltitude = transform.position.y;

        float altErr = targetAltitude - curAltitude;
        float curAltValue = altitudePID.Update(altErr);

        //Der Ausgabe-Wert des Altitude-PID wird auf den Bereich von 0 bis Maxthrust (im Moment 5) begrenzt
        if (curAltValue <= -maxThrust)
        {
            curAltValue = -maxThrust;
        }
        else if (curAltValue >= maxThrust)
        {
            curAltValue = maxThrust;
        }

        cT.thrust = curAltValue;
        //---------------------------------------------------------------------

    }

}
