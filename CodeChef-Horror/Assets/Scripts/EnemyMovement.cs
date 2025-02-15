using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public enum state { idle, chasing, attack }
    public state State;
    [SerializeField] Transform player;
    float myCollisonRadius, playerCollisionRad;
    [SerializeField] float AttackDistance, radius;
    [SerializeField] LayerMask playerLayer;
    NavMeshAgent pathFinder;

    private void Start()
    {
        pathFinder = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            Debug.Log(player.childCount);
            myCollisonRadius = GetComponent<CapsuleCollider>().radius;
            playerCollisionRad = player.GetComponent<CapsuleCollider>().radius;
        }

        State = state.idle;
    }

    IEnumerator UpdatePath()
    {
        while (player.gameObject != null)
        {
            {
                if (State == state.chasing)
                {
                    Vector3 dest = (player.position - transform.position).normalized;
                    Vector3 destination = player.position - dest * (myCollisonRadius + playerCollisionRad + AttackDistance / 2);
                    pathFinder.SetDestination(destination);
                }
            }
            yield return new WaitForSeconds(.25f);
        }
    }

    private void Update()
    {
        transform.LookAt(player);

        if(Physics.CheckSphere(transform.position, radius, playerLayer) && State != state.chasing){
            State = state.chasing;
            StartCoroutine(UpdatePath());
        }
    }

    int CurrentState()
    {
        switch (State)
        {
            case state.idle:
                return 0;
            case state.chasing:
                return 1;
            case state.attack:
                return 2;
            default:
                return 1;
        }
    }
}
