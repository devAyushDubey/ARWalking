/* -> This is a script written in C# that measures, tracks and calculates the location coordinates of a device in realtime.

   -> The position of the 'player' object is the position of the tracked device. Changing the position with change in device position 
      is handled by 'LocationProviderFactory.cs', 'ImmediatePositionWithLocationProvider.cs', and others in the Mapbox SDK resources.
   
   -> The changes in the position of 'player' when calculated is the distance travelled by the device.
   
   -> Rewards are spawned/ Instantiated/ created at specific distances. */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// DistanceController is called automatically (handled by coroutines) at the start of the application.

public class DistanceController : MonoBehaviour          // MonoBehaviour is the base class from which all unity scripts derive. 
{

    // Declaration of GUI objects of the application. Views in Native Android is similar to GameObjects in Unity.
    
    public Text tx;
    public Transform player;
    public LineRenderer line_renderer;
    public GameObject reward_p1;
    public GameObject spotL;

    // Initialization of variables.
    
    public float timer = 0;
    public float distance;
    public int i_distance;
    public int i_timer = 0;

    // Private Variables
    
    float dist;
    int i;
    int start;
    int k = 0;
    int reward_dist;
    float t_distance;

    Vector3 temp = new Vector3();

    // A private variable that can be accessed through the Unity editor.
    [SerializeField] List<Vector3> pos_list;
    
    // Start is called on the first frame of the application execution.
    
    void Start()
    {
        start = 1;                   // The time interval between collecting coordinates.
        reward_dist = 100;
    }
    
    // Update is called on every frame of the application execution. Generally 60FPS or 60 Calls-per-second.
    void Update()
    {
        temp = player.position;
        timer += Time.deltaTime;    // Gives sense of the real time.
        i_timer = (int)timer;

        if (start == i_timer)       // Condition to ensure the time interval of 'start' seconds for every data point entry.
        {
            start++;
            pos_list.Add(temp);     // Adding the coordinates to a list.
            line_renderer.positionCount = pos_list.Count;

            if (start != 2)         // Need atleast two points to draw a line.
            {
                line_renderer.SetPosition(k, pos_list[pos_list.Count - 1]);
                line_renderer.SetPosition(k+1, pos_list[pos_list.Count - 2]);
                k++;

                // Calculating Distance
                i = pos_list.Count-1;
                dist = (((pos_list[i].x - pos_list[i - 1].x) * (pos_list[i].x - pos_list[i - 1].x)) +
                    ((pos_list[i].y - pos_list[i - 1].y) * (pos_list[i].y - pos_list[i - 1].y)) +
                    ((pos_list[i].z - pos_list[i - 1].z) * (pos_list[i].z - pos_list[i - 1].z)));
                dist = Mathf.Sqrt(dist);
                distance += dist;
                //distance *= 0.201078457f;
                t_distance = distance * 3.709071692f;            // 3.7... is the conversion factor of realtime cordinates to Unity's scene (Virtual 3D) coordinates.
                i_distance = (int)t_distance;

                tx.text = "Distance: " + i_distance.ToString() + "m";   // Updating the GUI with distance
            }

            if (i_distance >= reward_dist-10 && i_distance <= reward_dist + 10)    // Creation of 'Treasure Chest' 3D object using Instantiate at the desired reward positions.
            {
                ChangeDist();
                Vector3 loc = new Vector3();
                loc.x = temp.x + Random.Range(0.5f,0.8f);
                loc.z = temp.z + Random.Range(0.5f,0.8f);
                Instantiate(reward_p1, new Vector3(loc.x,3.0f,loc.z), Quaternion.identity);
                Instantiate(spotL, new Vector3(loc.x, 0.7f, loc.z), Quaternion.identity);
            }


        }
    }

    void ChangeDist()  // Generating rewards distances
    {
        reward_dist = reward_dist + (int)Random.Range((reward_dist * 0.7f), (reward_dist * 0.8f));
        Debug.Log(reward_dist);
    }
    
    //For debugging purposes, increases the distance on a button click rather than GPS.
    
    public void Debuging()
    {
        distance += 2.5f;
    }
}
