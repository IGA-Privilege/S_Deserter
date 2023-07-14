using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    
    private Dictionary<Transform, bool> patrolInfos;
    private float waitSec = 0f;
    private float patrolInterval = 5f;
    private Transform nextWaypoint;
    private GuardState state;
    private bool _isWalking { get { return state == GuardState.Patrol; } }
    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        patrolInfos = new Dictionary<Transform, bool>();
        foreach (Transform waypoint in waypoints)
        {
            patrolInfos.Add(waypoint, false);
        }
    }

    private void Start()
    {
        state = GuardState.Static;
        nextWaypoint = waypoints[0];
    }

    private void Update()
    {

        _animator.SetBool("isWalking", _isWalking);
    }

    private void FixedUpdate()
    {
        if (state == GuardState.Static)
        {
            waitSec += Time.fixedDeltaTime;
            if (waitSec > patrolInterval)
            {
                waitSec = 0f;
                bool allWaypointsPatrolled = true;
                for (int i = 0; i < waypoints.Count; i++)
                {
                    if (patrolInfos[waypoints[i]] == false)
                    {
                        nextWaypoint = waypoints[i];
                        state = GuardState.Patrol;
                        allWaypointsPatrolled = false;
                        break;
                    }
                }

                if (allWaypointsPatrolled)
                {
                    for (int i = 0; i < waypoints.Count; i++)
                    {
                        patrolInfos[waypoints[i]] = false;
                    }
                }
            }
        }

        else if (state == GuardState.Patrol)
        {
            if (Vector3.Distance(transform.position, nextWaypoint.position) < 0.05f)
            {
                state = GuardState.Static;
                patrolInfos[nextWaypoint] = true;
            }
            else
            {
                if (Vector3.Angle(transform.up, nextWaypoint.position - transform.position) < 10f)
                {
                    float moveSpeed = 0.01f;
                    transform.position += (nextWaypoint.position - transform.position).normalized * moveSpeed;
                }
                float turningSpeed = 0.02f;
                transform.up = Vector3.Lerp(transform.up, nextWaypoint.position - transform.position, turningSpeed);
            }
        }
    }


}

public enum GuardState
{
    Static, Patrol, ChasePlayer
}
