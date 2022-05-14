using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Animal;

public class AnimalLayer : RasterLayer,IAnimalLayer
{
    public SpatialHashEnvironment<Animal> Environment;

    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
        UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        var agentManager = layerInitData.Container.Resolve<IAgentManager>();
        Environment = new SpatialHashEnvironment<Animal>(Width, Height);
            
            
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var type = this[x, y];
                var position = Position.CreatePosition(x,y);
                var animal = CreateAnimal(agentManager, type, position);
                if ( animal != null)
                {
                    Environment.Insert(animal);
                }
            }
        }

        Console.WriteLine(Environment.Entities.Count() +"Animals spawned");
        return true;
    }
        
    private Animal CreateAnimal(IAgentManager agentManager,double type, Position position)
    {
        return type switch
        {
            1 => agentManager.Spawn<Animal, AnimalLayer>(null, t => t.Position = position).Take(1).First(),
            _ => null
        };
    }

    public bool IsAlive(Position tree)
    {
        //TODO
        throw new System.NotImplementedException();
    }

    public List<Position> ExploreAnimals(Position explorer, int distance)
    {
        //TODO
        throw new System.NotImplementedException();
    }
}