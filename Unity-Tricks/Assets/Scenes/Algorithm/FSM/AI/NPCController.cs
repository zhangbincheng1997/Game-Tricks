using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform[] pathPoints;
    public GameObject npc;
    public GameObject player;

    private FSMSystem fsm;

    void Start()
    {
        InitFSM();
    }

    void Update()
    {
        fsm.currentState.DoUpdate();
    }

    void InitFSM()
    {
        fsm = new FSMSystem();

        PatrolState patrolState = new PatrolState(pathPoints, npc, player);
        patrolState.AddTransition(Transition.FindPlayer, StateId.Chase);

        ChaseState chaseState = new ChaseState(npc, player);
        chaseState.AddTransition(Transition.LosePlayer, StateId.Patrol);

        fsm.AddState(patrolState);
        fsm.AddState(chaseState);

        fsm.StartState(StateId.Patrol);
    }
}
