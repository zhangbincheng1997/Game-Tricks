using Unity.Entities;

[System.Serializable]
public struct MoveData_Job : IComponentData
{
    public float delay;
    public float velocity;
    public float minHeight;
}
