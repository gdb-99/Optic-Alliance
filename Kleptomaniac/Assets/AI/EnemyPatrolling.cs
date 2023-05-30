using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    public float speed;
    public Transform[] points;
    private int current;
    private bool isPatrolling = true;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatrolling)
        {
            if (Mathf.Abs(transform.position.x - points[current].position.x) > 0.1f &&
                Mathf.Abs(transform.position.z - points[current].position.z) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
                transform.LookAt(points[current].position);
            }
            else
            {
                current = (current + 1) % points.Length;
            }
        }
    }

    public void StopPatrolling()
    {
        isPatrolling = false;
    }
}

