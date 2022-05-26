using System.Collections.Generic;
using System.Linq;
using Mars.Common.Core;
using Mars.Common.Data;
using Mars.Components.Layers;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Environment;

public class InitialPositionLayer : AbstractDataLayer
{
    public List<Position> SpawnPositionsAnimal = new List<Position>();
    public List<Position> SpawnPositionsTree= new List<Position>();
    public List<Position> SpawnPositionsHuman= new List<Position>();

    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle, UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        
        var dataEnumerator = layerInitData.LayerInitConfig.Inputs.Import().OfType<StructuredData>().GetEnumerator();
        while (dataEnumerator.MoveNext())
        {
            var current = dataEnumerator.Current;
            if (current == null) continue;
            var data = current.Data;
            switch (data["type"])
            {
                case "animal":
                {
                    SpawnPositionsAnimal.Add(new Position(data["x"].To<int>(),data["y"].To<int>()));
                    break;
                }
                case "human":
                {
                    SpawnPositionsHuman.Add(new Position(data["x"].To<int>(),data["y"].To<int>()));
                    break;
                }
                case "tree":
                {
                    SpawnPositionsTree.Add(new Position(data["x"].To<int>(),data["y"].To<int>()));
                    break;
                }
            }
        }
        dataEnumerator.Dispose();
        return true;
    }
}