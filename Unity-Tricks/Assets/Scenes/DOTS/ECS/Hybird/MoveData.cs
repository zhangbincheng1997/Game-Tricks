using Unity.Entities;

[System.Serializable]
public struct MoveData : IComponentData
{
    public float delay;
    public float velocity;
    public float minHeight;
}
