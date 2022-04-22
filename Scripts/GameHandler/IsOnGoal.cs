using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGoal : MonoBehaviour
{
    public float goal = 20;
    public GameObject player;
    private Vector3 goalPosition;
    public bool isOnGoal = false;
    public bool drawGizmos = false;

    // Update is called once per frame
    void Update()
    {
        if ((player.GetComponent<Transform>().position - transform.position).sqrMagnitude < goal * goal)
            isOnGoal = true;
    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, goal);
        }
    }
}
