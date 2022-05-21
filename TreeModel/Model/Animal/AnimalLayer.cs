using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Components.Services;
using Mars.Core.Data;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using ServiceStack;
using TreeModel.Model.Environment;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Animal;

public class AnimalLayer : RasterLayer, IAnimalLayer
{
    public SpatialHashEnvironment<Animal> Environment;


    // How do it work
    [PropertyDescription] public TreeLayer TreeLayer { get; set; }

    public TerrainLayer TerrainLayer { get; set; }

    private IAgentManager _agentManager;

    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
        UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        _agentManager = layerInitData.Container.Resolve<IAgentManager>();
        Environment = new SpatialHashEnvironment<Animal>(Width, Height);


        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var type = this[x, y];
                var position = Position.CreatePosition(x, y);
                CreateAnimal(type, position);
            }
        }

        Console.WriteLine(Environment.Entities.Count() + "Animals spawned");
        return true;
    }

    public void RemoveAnimal(Animal animal)
    {
        Environment.Remove(animal);
    }
    public Animal CreateAnimal(double type, Position position)
    {
        var animal = type switch
        {
            1 => _agentManager.Spawn<Animal, AnimalLayer>(null, t => t.Position = position).Take(1).First(),
            _ => null
        };
        if (animal != null)
        {
            Environment.Insert(animal);
        }

        return animal;
    }

    public bool IsAlive(Position animal)
    {
        if (Environment.Entities.Any(t => t.Position.Equals(animal)))
        {
            return Environment.Entities.First(t => t.Position.Equals(animal)).Alive;
        }

        return false;
    }

    public List<Position> ExploreAnimals(Position explorer, int distance)
    {
        var result = Environment.Explore(explorer, distance).ToList().Map(t => t.Position);
        return result;
    }
}