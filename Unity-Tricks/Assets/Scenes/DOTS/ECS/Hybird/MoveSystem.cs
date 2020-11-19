using Unity.Entities;
using Unity.Transforms;

public class MoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation translation, ref MoveData moveData) =>
        {
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
        });
    }
}
