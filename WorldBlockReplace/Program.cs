using OrangeNBT.Data;
using OrangeNBT.Data.Anvil.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldBlockReplace
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] worldPath = Environment.GetCommandLineArgs();

            if (worldPath.Length <= 1)
            {
                Console.WriteLine("ソフトウェアの実行ファイル(exe)にワールドのディレクトリをドラッグ&ドロップしてください");
                Console.ReadKey();
                return;
            }


            // ワールドを読み込み
            using (var w = OrangeNBT.World.Anvil.AnvilWorld.Load(worldPath[1]))
            {
                // 全てのディメンションを順に読み込み
                foreach (var dim in w.Dimensions)
                {
                    var coords = dim.Value.Chunks.ListAllCoords();
                    int count = coords.Count();

                    foreach (var coord in coords)
                    {
                        Console.WriteLine("ディメンション:{0} 残り{1}チャンク", dim.Key, count);

                        var chunk = dim.Value.Chunks.GetChunk(coord);

                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 256; y++)
                            {
                                for (int z = 0; z < 16; z++)
                                {


                                    // 全ブロック取得
                                    BlockSet block = chunk.GetBlock(x, y, z);

                                    switch (block.Name)
                                    {
                                        case "minecraft:coal_ore":
                                        case "minecraft:iron_ore":
                                        case "minecraft:gold_ore":
                                        case "minecraft:diamond_ore":
                                        case "minecraft:redstone_ore":
                                        case "minecraft:emerald_ore":
                                        case "minecraft:lapis_ore":
                                            // チャンク座標からワールドのX座標に変換
                                            var worldX = (coord.X << 4) + x;

                                            // チャンク座標からワールドのZ座標に変換
                                            var worldZ = (coord.Z << 4) + z;
                                            dim.Value.Blocks.SetBlock(worldX, y, worldZ, "minecraft:stone");
                                            break;
                                    }
                                }
                            }
                        }
                        count--;
                    }
                    Console.WriteLine("ディメンション:{0} 保存中...", dim.Key);
                    dim.Value.Save();
                }
            }

            Console.WriteLine("完了!エンターを押して終了してください");
            Console.ReadKey();
        }
    }
}
