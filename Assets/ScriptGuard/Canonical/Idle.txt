using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class Idle : MonoBehaviour
{

    public enum IdleActionType
    {
        None,
        Wait,
        Rotate,
        Wander
    }

    [Header("Idle Settings")]
    public bool standEnable=true;
    public bool turnEnable = true;
    public bool lingerEnable = true;
    public Vector2 idleWaitTime = new Vector2(2f, 5f);
    public float rotationSpeed = 90f;
    public float wanderRadius = 5f;
    public Vector2 wanderPauseTime = new Vector2(1f, 3f);

    [Header("Optional Waypoints")]
    [Tooltip("If assigned, AI will wander to these points instead of random NavMesh positions.")]
    public List<Transform> wanderPoints;

    private bool isBusy = false;

    public void HandleIdle(NavMeshAgent agent)
    {
        if (isBusy) return;
        StartCoroutine(IdleRoutine(agent));
    }

    private IEnumerator IdleRoutine(NavMeshAgent agent)
    {
        isBusy = true;

        // Build a list of available actions based on enabled options
        List<IdleActionType> availableActions = new List<IdleActionType>();

        if (standEnable) availableActions.Add(IdleActionType.Wait);
        if (turnEnable) availableActions.Add(IdleActionType.Rotate);
        if (lingerEnable) availableActions.Add(IdleActionType.Wander);

        // If nothing is enabled, do nothing
        if (availableActions.Count == 0)
        {
            isBusy = false;
            yield break;
        }

        // Pick a random action from available options
        IdleActionType action = availableActions[Random.Range(0, availableActions.Count)];

        // Execute chosen action
        switch (action)
        {
            case IdleActionType.Wait:
                yield return WaitAction(agent);
                break;
            case IdleActionType.Rotate:
                yield return RotateAction(agent);
                break;
            case IdleActionType.Wander:
                yield return WanderAction(agent);
                break;
        }

        isBusy = false;
    }


    private IEnumerator WaitAction(NavMeshAgent agent)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(idleWaitTime.x, idleWaitTime.y));
    }

    private IEnumerator RotateAction(NavMeshAgent agent)
    {
        agent.isStopped = true;
        float rotateTime = Random.Range(1f, 3f);
        float dir = Random.value > 0.5f ? 1f : -1f;
        float elapsed = 0f;

        while (elapsed < rotateTime)
        {
            transform.Rotate(Vector3.up * dir * rotationSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WanderAction(NavMeshAgent agent)
    {
        Vector3 target;

        // Use waypoints if assigned
        if (wanderPoints != null && wanderPoints.Count > 0)
        {
            Transform point = wanderPoints[Random.Range(0, wanderPoints.Count)];
            target = point.position;
        }
        else
        {
            // Random NavMesh position
            target = RandomNavSphere(transform.position, wanderRadius);
        }

        // Ensure agent is on NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hitStart, 1f, NavMesh.AllAreas))
            agent.Warp(hitStart.position);

        agent.isStopped = false;
        agent.SetDestination(target);
        //Debug.Log($"Wandering from {transform.position} to {target}");

        float timeout = 10f;
        while (agent.pathPending)
            yield return null;

        while (agent.remainingDistance > agent.stoppingDistance && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(wanderPauseTime.x, wanderPauseTime.y));
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance + origin;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, distance, NavMesh.AllAreas))
            return hit.position;

        return origin;
    }
}
