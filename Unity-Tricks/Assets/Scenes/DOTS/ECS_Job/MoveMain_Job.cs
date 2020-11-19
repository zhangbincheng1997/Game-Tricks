using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class MoveMain_Job : MonoBehaviour
{
    public int entityCount = 1000;
    public int entityRange = 100;
    public GameObject prefab;

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        NativeArray<Entity> entities = new NativeArray<Entity>(entityCount, Allocator.Temp);
        Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null));
        entityManager.Instantiate(entity, entities);

        for (int i = 0; i < entityCount; i++)
        {
            Translation translation = new Translation();
            translation.Value = Random.insideUnitSphere * entityRange;
            translation.Value.y = 0;
            entityManager.SetComponentData(entities[i], translation);
            entityManager.SetComponentData(entities[i], new MoveData_Job { delay = Random.Range(1, 10), velocity = Random.Range(1, 100), minHeight = -100 });
        }
        entities.Dispose();
    }
}
