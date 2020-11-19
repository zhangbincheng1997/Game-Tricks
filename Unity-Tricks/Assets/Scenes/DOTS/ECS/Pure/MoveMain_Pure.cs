using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class MoveMain_Pure : MonoBehaviour
{
    public int entityCount = 1000;
    public int entityRange = 100;
    public Mesh mymesh;
    public Material mymaterial;

    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        NativeArray<Entity> entities = new NativeArray<Entity>(entityCount, Allocator.Temp);
        EntityArchetype entityArchetype = entityManager.CreateArchetype
        (
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(MoveData_Pure),
            typeof(RenderMesh),
            typeof(RenderBounds)
        );
        entityManager.CreateEntity(entityArchetype, entities);

        for (int i = 0; i < entityCount; i++)
        {
            Translation translation = new Translation();
            translation.Value = Random.insideUnitSphere * entityRange;
            translation.Value.y = 0;
            entityManager.SetComponentData(entities[i], translation);
            entityManager.SetComponentData(entities[i], new MoveData_Pure { delay = Random.Range(1, 10), velocity = Random.Range(1, 100), minHeight = -100 });
            // RenderMesh
            entityManager.SetSharedComponentData(entities[i], new RenderMesh { mesh = mymesh, material = mymaterial });
            // RenderBounds
            entityManager.SetComponentData(entities[i], new RenderBounds { });
        }
        entities.Dispose();
    }
}
