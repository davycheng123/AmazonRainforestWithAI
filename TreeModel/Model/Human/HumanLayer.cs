using System;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using TreeModel.Model.Environment;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Human;

public class HumanLayer: RasterLayer, IHumanLayer
{
    public SpatialHashEnvironment<Human> Environment;

    // How do it work
    [PropertyDescription]
    public TreeLayer TreeLayer { get; set; }
    
    public TerrainLayer TerrainLayer { get; set; }
    
    
    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
        UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        var agentManager = layerInitData.Container.Resolve<IAgentManager>();
        Environment = new SpatialHashEnvironment<Human>(Width, Height);
            
            
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var type = this[x, y];
                var position = Position.CreatePosition(x,y);
                var human = CreateHuman(agentManager, type, position);
                if ( human != null)
                {
                    Environment.Insert(human);
                }
            }
        }

        Console.WriteLine(Environment.Entities.Count() +"Human spawned");
        return true;
    }
        
    private Human CreateHuman(IAgentManager agentManager,double type, Position position)
    {
        var human =  type switch
        {
            1 => agentManager.Spawn<Human, HumanLayer>(null, t => t.Position = position).Take(1).First(),
            _ => null
        };
        
        if (human != null)
        {
            Environment.Insert(human);
        }

        return human;
    }

    public bool IsAlive(Position human)
    {
        if (Environment.Entities.Any(t => t.Position.Equals(human)))
        {
            return Environment.Entities.First(t => t.Position.Equals(human)).alive;
        }

        return false;
    }

}