using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

// [DisableAutoCreation]
public class MoveSystem_Chunk : JobComponentSystem
{
    EntityQuery entityQuery;

    protected override void OnCreate()
    {
        entityQuery = GetEntityQuery(typeof(Translation), typeof(MoveData_Job));
    }

    [BurstCompile]
    struct MoveJob : IJobChunk
    {
        public float deltaTime;
        public ComponentTypeHandle<Translation> translationType;
        public ComponentTypeHandle<MoveData_Job> moveDataType;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            var translationArray = chunk.GetNativeArray(translationType);
            var moveDataArray = chunk.GetNativeArray(moveDataType);

            for (int i = 0; i < chunk.Count; i++)
            {
                // get
                var translation = translationArray[i];
                var moveData = moveDataArray[i];

                if (moveData.delay > 0)
                {
                    moveData.delay -= deltaTime;
                }
                else
                {
                    if (translation.Value.y < moveData.minHeight)
                    {
                        translation.Value.y = 0;
                    }
                    else
                    {
                        translation.Value.y -= moveData.velocity * deltaTime;
                    }
                }

                // set
                translationArray[i] = translation;
                moveDataArray[i] = moveData;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var translationType = GetComponentTypeHandle<Translation>();
        var moveDataType = GetComponentTypeHandle<MoveData_Job>();

        JobHandle jobHandle = new MoveJob()
        {
            deltaTime = Time.DeltaTime,
            translationType = translationType,
            moveDataType = moveDataType,
        }.Schedule(entityQuery, inputDeps);
        return jobHandle;
    }
}
