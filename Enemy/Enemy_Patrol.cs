using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Patrol : MonoBehaviour
{
    [SerializeField] List<Vector3> PatrolPoints = new List<Vector3>();
    [SerializeField] NavMeshAgent m_Agent;
    private Vector3 DestinationPoint;
    public int DestinationPointIndex;
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.stoppingDistance = 0;
        DestinationPointIndex = 0;
        m_Agent.SetDestination(PatrolPoints[DestinationPointIndex]);
        DestinationPoint = PatrolPoints[DestinationPointIndex];
    }
    public void DoPatrol()
    {
        if (Vector3.Distance(transform.position, DestinationPoint) < 1)
        {
            m_Agent.stoppingDistance = 0;
            DestinationPointIndex++;
            if (DestinationPointIndex == PatrolPoints.Count)
                DestinationPointIndex = 0;
            DestinationPoint = PatrolPoints[DestinationPointIndex];
            m_Agent.SetDestination(DestinationPoint);
        }
        if (DestinationPoint.x != m_Agent.destination.x && DestinationPoint.z != m_Agent.destination.z)
        {
            m_Agent.stoppingDistance = 0;
            m_Agent.isStopped = false;
            m_Agent.SetDestination(DestinationPoint);
        }
    }
}
