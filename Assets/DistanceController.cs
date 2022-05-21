using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceController : MonoBehaviour
{

    public Text tx;
    public Transform player;
    public LineRenderer lr;
    public GameObject reward_p1;
    public GameObject spotL;

    public float timer = 0;
    public float distance;
    public int i_distance;
    public int i_timer = 0;

    float dist;
    int i;
    int start;
    int k = 0;
    int reward_dist;
    float t_distance;

    Vector3 temp = new Vector3();

    [SerializeField] List<Vector3> pos_list;
    

    void Start()
    {
        start = 1;
        reward_dist = 100;
    }
    
    void Update()
    {
        temp = player.position;
        timer += Time.deltaTime;
        i_timer = (int)timer;

        if (start == i_timer)
        {
            start++;
            pos_list.Add(temp);
            lr.positionCount = pos_list.Count;

            if (start != 2)
            {
                lr.SetPosition(k, pos_list[pos_list.Count - 1]);
                lr.SetPosition(k+1, pos_list[pos_list.Count - 2]);
                k++;

                i = pos_list.Count-1;
                dist = (((pos_list[i].x - pos_list[i - 1].x) * (pos_list[i].x - pos_list[i - 1].x)) +
                    ((pos_list[i].y - pos_list[i - 1].y) * (pos_list[i].y - pos_list[i - 1].y)) +
                    ((pos_list[i].z - pos_list[i - 1].z) * (pos_list[i].z - pos_list[i - 1].z)));
                dist = Mathf.Sqrt(dist);
                distance += dist;
                //distance *= 0.201078457f;
                t_distance = distance * 3.709071692f;
                i_distance = (int)t_distance;

                tx.text = "Distance: " + i_distance.ToString() + "m";
            }

            if (i_distance >= reward_dist-10 && i_distance <= reward_dist + 10)
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

    void ChangeDist()
    {
        reward_dist = reward_dist + (int)Random.Range((reward_dist * 0.7f), (reward_dist * 0.8f));
        Debug.Log(reward_dist);
    }
    //
    public void Debuging()
    {
        distance += 2.5f;
    }
}
