using System;
using Mars.Common.Core.Random;
using Mars.Components.Layers;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Environments;
using TreeModel.Model.Environment;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Spot;

public class Water: IAgent<TerrainLayer>, IPositionable
{
    public int WaterValue { get; set; } = 1000;
    
    public int WaterRate { get; set; }
    
    public void Init(TerrainLayer layer)
    {
        TerrainLayer = layer;;

    }
    private TerrainLayer TerrainLayer { get; set; }

    
    public void Tick()
    {
        // TODO: according on the weather Data
        // Increase Water in Random
        var Pos = FindRandomPosition();
        TerrainLayer[Pos] = TerrainLayer[Pos] + WaterRate;
    }
    
    public Position FindRandomPosition()
    {
        var random = RandomHelper.Random;
        return Position.CreatePosition(random.Next(TerrainLayer.Width - 1), random.Next(TerrainLayer.Height - 1));
    }

    public Guid ID { get; set; }
    public Position Position { get; set; }
}