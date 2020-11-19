using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class MoveProxy : MonoBehaviour, IConvertGameObjectToEntity
{
    public MoveData moveData;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, moveData);
    }
}
