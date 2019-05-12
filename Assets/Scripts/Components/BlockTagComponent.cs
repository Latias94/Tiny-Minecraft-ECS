using System;
using Unity.Entities;

[Serializable]
public struct BlockTag : IComponentData
{
}

public class BlockTagComponent : ComponentDataProxy<BlockTag>
{
}