using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class SpawnNumberBlocks : MonoBehaviour
    {
        //public static GameSettings GM;
        public static Texture2D Heightmap;

        public static EntityArchetype BlockArchetype;

        [Header("World = ChunkBase x ChunkBase")]
        // 区块（Chunk）是 Minecraft 世界里一个大小为16×256×16的部分。
        public int ChunkBase = 1;

        [Header("Mesh Info")] public Mesh blockMesh;

        [Header("For Debug")] public Material no0Mat;
        public Material no1Mat;
        public Material no2Mat;
        public Material no3Mat;
        public Material no4Mat;
        public Material no5Mat;
        public Material no6Mat;
        public Material noQMat;


        Material maTemp;

        public EntityManager manager;
        public Entity entities;

        void Start()
        {
            manager = World.Active.EntityManager;

            // Create an archetype for basic blocks.
            BlockArchetype = manager.CreateArchetype(
                typeof(Translation),
                typeof(LocalToWorld)
            );
            //Generate the world
            ChunkGenerator(ChunkBase);
        }

        void ChunkGenerator(int amount)
        {
            // 每个 chunks 1500 个方块， y 方向 15 个，x 方向 10 个，z 方向 10 个
            // ChunkBase 有 10 个，因此一共有 10 * 10 个 chunks
            // 一共是 10 * 10 * 1500 个方块
            int totalamount = (amount * amount) * 1500;
            //int ordernumber = 0;
            int hightlevel;
            bool airChecker;

            // Block ordering from X*0,0,0 to 15,10,10( * Chunk x2)
            for (int yBlock = 0; yBlock < 15; yBlock++)
            {
                for (int xBlock = 0; xBlock < 10 * amount; xBlock++)
                {
                    for (int zBlock = 0; zBlock < 10 * amount; zBlock++)
                    {
                        hightlevel = (int) (Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                        airChecker = false;
                        Vector3 posTemp = new Vector3(xBlock, yBlock, zBlock);

                        switch (hightlevel)
                        {
                            case 0:
                                maTemp = no0Mat;
                                break;
                            case 1:
                                maTemp = no1Mat;
                                break;
                            case 2:
                                maTemp = no2Mat;
                                break;
                            case 3:
                                maTemp = no3Mat;
                                break;
                            case 4:
                                maTemp = no4Mat;
                                break;
                            case 5:
                                maTemp = no5Mat;
                                break;
                            case 6:
                                maTemp = no6Mat;
                                break;
                            default:
                                // 0 - 6 以外的都是空气
                                maTemp = noQMat;
                                airChecker = true;
                                break;
                        }

                        if (!airChecker)
                        {
                            Entity entities = manager.CreateEntity(BlockArchetype);
                            manager.SetComponentData(entities, new Translation
                            {
                                Value = new int3(xBlock, yBlock,
                                    zBlock)
                            });
                            manager.AddComponentData(entities, new BlockTag());

                            manager.AddSharedComponentData(entities, new RenderMesh
                            {
                                mesh = blockMesh,
                                material = maTemp
                            });
                        }
                    }
                }
            }
        }
    }
}