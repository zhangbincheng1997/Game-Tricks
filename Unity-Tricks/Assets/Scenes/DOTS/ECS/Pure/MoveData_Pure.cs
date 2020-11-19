﻿using Unity.Entities;

[System.Serializable]
public struct MoveData_Pure : IComponentData
{
    public float delay;
    public float velocity;
    public float minHeight;
}
