using UnityEngine;

public class ChaseState : FSMState
{
    private GameObject npc;
    private GameObject player;
    private Rigidbody rigidbody;

    private float minDistance = 0.5f;
    private float maxDistance = 2f;
    private float speed = 4f;

    public ChaseState(GameObject npc, GameObject player)
    {
        stateId = StateId.Chase;
        this.npc = npc;
        this.player = player;
        rigidbody = npc.GetComponent<Rigidbody>();
    }

    public override void DoBeforeEnter()
    {
        Debug.Log("DoBeforeEnter - ChaseState");
    }

    public override void DoBeforeExit()
    {
        Debug.Log("DoBeforeExit - ChaseState");
    }

    public override void DoUpdate()
    {
        // Debug.Log("DoUpdate - ChaseState");

        CheckTransition();  // 检查条件

        Chase();  // 追逐
    }

    private void CheckTransition()
    {
        float distance = Vector3.Distance(npc.transform.position, player.transform.position);
        if (distance >= maxDistance)
        {
            fsm.DoTransition(Transition.LosePlayer);
        }
    }

    private void Chase()
    {
        rigidbody.velocity = npc.transform.forward * speed;
        Vector3 targetPos = player.transform.position;
        targetPos.y = npc.transform.position.y;
        npc.transform.LookAt(targetPos); // 望向主角

        if (Vector3.Distance(npc.transform.position, targetPos) <= minDistance)  // 到达主角，游戏结束
        {
            rigidbody.velocity = Vector3.zero;
            Debug.Log("LOSE~~~");
        }
    }
}
