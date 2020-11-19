using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class MoveProxy_Job : MonoBehaviour, IConvertGameObjectToEntity
{
    public MoveData_Job moveData;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, moveData);
    }
}
