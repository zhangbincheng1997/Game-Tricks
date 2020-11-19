//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Transforms;

//public class MoveSystem_Job : JobComponentSystem
//{
//    protected override JobHandle OnUpdate(JobHandle inputDeps)
//    {
//        float deltaTime = Time.DeltaTime;
//        JobHandle jobHandle = Entities.ForEach((ref Translation translation, ref MoveData_Job moveData) =>
//        {
//            if (moveData.delay > 0)
//            {
//                moveData.delay -= deltaTime;
//            }
//            else
//            {
//                if (translation.Value.y < moveData.minHeight)
//                {
//                    translation.Value.y = 0;
//                }
//                else
//                {
//                    translation.Value.y -= moveData.velocity * deltaTime;
//                }
//            }
//        }).Schedule(inputDeps);
//        return jobHandle;
//    }
//}
