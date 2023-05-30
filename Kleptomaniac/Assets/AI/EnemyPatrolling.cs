using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    public Transform[] points;
    int current;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
            transform.LookAt(points[current].position); // Aggiunta: Orienta l'EnemyCharacter verso il punto di destinazione
        }
        else
        {
            current = (current + 1) % points.Length;
        }
    }
}
