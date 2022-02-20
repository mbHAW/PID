using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{
    GameObject cube;
    public GameObject follower;

    //Hier wird das Prefab des Wegpunkts eingefügt, welcher später erzeugt wird
    public GameObject waypoint;

    //Layermask, damit nur der Boden angeklickt werden kann und die Wegpunkte und der Würfel ignoriert werden
    public LayerMask clickMask;

    WaypointManager wM;

    void Awake()
    {
        cube = GameObject.FindGameObjectWithTag("Cube");
    }

    void Start()
    {
        wM = cube.GetComponent<WaypointManager>();

        GameObject firstWaypoint = Instantiate(waypoint);
        firstWaypoint.transform.position = new Vector3(Random.Range(-39, 39), 11.5f, Random.Range(-39, 39));
        wM.waypoints.Add(firstWaypoint);
    }

    void Update()
    {
        //Hier wird die Position ermittelt, wo auf dem Boden geklickt wurde. Dann wird an dieser Stelle ein Wegpunkt mit Instantiate erzeugt.
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = -Vector3.one;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast (ray, out hit, 100f, clickMask))
            {
                clickPos = hit.point;
            }

            if(wM.waypoints.Count > 0)
            {
                GameObject curWaypoint = wM.waypoints.Last();
                wM.waypoints.Remove(curWaypoint);
                Destroy(curWaypoint);
            }

            GameObject lastWaypoint = Instantiate(waypoint, new Vector3(clickPos.x, 6.25f, clickPos.z), Quaternion.identity);
            
            //Der erzeugte Wegpunkt wird an letzter Stelle in die Liste im WaypointManager eingefügt
            wM.waypoints.Add(lastWaypoint);

        }
    }
}
