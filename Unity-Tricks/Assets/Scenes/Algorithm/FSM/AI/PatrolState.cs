using UnityEngine;

public class PatrolState : FSMState
{
    private Transform[] pathPoints;
    private GameObject npc;
    private GameObject player;
    private Rigidbody rigidbody;

    private float minDistance = 0.5f;
    private float maxDistance = 2f;
    private float speed = 2f;
    private int targetPoint = 0;

    public PatrolState(Transform[] pathPoints, GameObject npc, GameObject player)
    {
        stateId = StateId.Patrol;
        this.pathPoints = pathPoints;
        this.npc = npc;
        this.player = player;
        rigidbody = npc.GetComponent<Rigidbody>();
    }

    public override void DoBeforeEnter()
    {
        Debug.Log("DoBeforeEnter - PatrolState");
    }

    public override void DoBeforeExit()
    {
        Debug.Log("DoBeforeExit - PatrolState");
    }

    public override void DoUpdate()
    {
        // Debug.Log("DoUpdate - PatrolState");

        CheckTransition();  // 检查条件

        Patrol();  // 巡逻
    }

    private void CheckTransition()
    {
        float distance = Vector3.Distance(npc.transform.position, player.transform.position);
        if (distance <= maxDistance)
        {
            fsm.DoTransition(Transition.FindPlayer);
        }
    }

    private void Patrol()
    {
        rigidbody.velocity = npc.transform.forward * speed;
        Vector3 targetPos = pathPoints[targetPoint].position;
        targetPos.y = npc.transform.position.y;
        npc.transform.LookAt(targetPos);  // 望向目标

        if (Vector3.Distance(npc.transform.position, targetPos) <= minDistance)  // 到达目标，下一个目标
        {
            ++targetPoint;
            targetPoint %= pathPoints.Length;
        }
    }
}
