using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Common.Core;
using Mars.Common.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;


namespace TreeModel.Model.Environment;

public class TerrainLayer : ITerrainLayer
{
    private long _tick = 0;

    private Dictionary<Position, double[]> _dict = new Dictionary<Position, double[]>();

    public bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
        UnregisterAgent unregisterAgent = null)
    {
        var dataEnumerator = layerInitData.LayerInitConfig.Inputs.Import().OfType<StructuredData>().GetEnumerator();
        while (dataEnumerator.MoveNext())
        {
            var current = dataEnumerator.Current;
            if (current == null) continue;
            var data = current.Data;
            var pos = new Position(data["x"].To<int>(), data["y"].To<int>());
            _dict.Add(pos, new double[] {data["waterlevel"].To<double>(), data["nutrients"].To<double>()});
        }

        dataEnumerator.Dispose();
        Console.WriteLine("terrain ok");
        return true;
    }

    public long GetCurrentTick()
    {
        return _tick;
    }

    public void SetCurrentTick(long currentStep)
    {
        _tick = currentStep;
    }

    public double GetWaterLevel(Position position)
    {
        if (position == null || !_dict.ContainsKey(position))
        {
            return -1;
        }

        return _dict[position][0];
    }

    public double GetSoilNutrients(Position position)
    {
        if (position == null || !_dict.ContainsKey(position))
        {
            return -1;
        }

        return _dict[position][1];
    }

    public double RemoveWater(Position position, double amount)
    {
        if (position == null || !_dict.ContainsKey(position))
        {
            return -1;
        }

        var water = _dict[position];
        water[0] -= amount;
        if (water[0] > 0) return amount;
        return amount + water[0];
    }

    public double RemoveSoilNutrients(Position position, double amount)
    {
        if (position == null || !_dict.ContainsKey(position))
        {
            return -1;
        }

        var nutrients = _dict[position];
        nutrients[1] -= amount;
        if (nutrients[1] > 0) return amount;
        return amount + nutrients[0];
    }

    public void AddSoilNutrients(Position position, double amount)
    {
        if (position == null || !_dict.ContainsKey(position)) return;
        var nutrients = _dict[position];
        nutrients[1] += amount;
    }

    public void AddWater(Position position, double amount)
    {
        if (position == null || !_dict.ContainsKey(position)) return;
        var water = _dict[position];
        water[0] += amount;
    }

    public void AddWaterEverywhere(double amount)
    {
        foreach (var dictValue in _dict.Values)
        {
            dictValue[0] += amount;
        }
    }
}