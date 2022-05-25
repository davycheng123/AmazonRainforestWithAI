using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Mars.Common.Data;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Components.Services;
using Mars.Core.Data;
using Mars.Interfaces;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using Mars.Interfaces.Model;
using TreeModel.Model.Animal;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Environment;

public class ForestLayer : RasterLayer
{
    public SpatialHashEnvironment<Tree.Tree> TreeEnvironment;
    public SpatialHashEnvironment<Animal.Animal> AnimalEnvironment;
    //public SpatialHashEnvironment<Human.Human> HumanEnvironment;

    private IAgentManager _agentManager;
    private IEntityManager _entityManager;
    
    [PropertyDescription]
    public List<string> AnimalTypes { get; set; }
    
    [PropertyDescription]
    public List<string> TreeTypes { get; set; }
    
    [PropertyDescription]
    public List<string> HumanTypes { get; set; }
    
    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle, UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        _agentManager = layerInitData.Container.Resolve<IAgentManager>();
        _entityManager = layerInitData.Container.Resolve<IEntityManager>();
        TreeEnvironment = new SpatialHashEnvironment<Tree.Tree>(Width, Height);
        AnimalEnvironment = new SpatialHashEnvironment<Animal.Animal>(Width, Height);
        //HumanEnvironment = new SpatialHashEnvironment<Human.Human>(Width, Height);
        SpawnAnimals();
        SpawnTrees();
        SpawnHumans();
        return true;
    }

    private void SpawnAnimals()
    {
        var types = new List<AnimalType>();
        AnimalTypes.ForEach(s => types.Add( _entityManager.Create<AnimalType>("Name", s)));
        Random rnd = new Random();
        types.ForEach(at =>
        {
            var animals =_agentManager.Spawn<Animal.Animal,ForestLayer>(null, a =>
            {
                a.Age = rnd.Next(0, at.MaxAge * 365);
                a.MovementSpeed = at.MovementSpeed;
                a.Carnivore = at.Carnivore;
                a.Herbivore = at.Herbivore;
                a.Name = at.Name;
                a.ConsumptionRate = at.ConsumptionRate;
                a.MatureAge= at.MatureAge;
                a.MaxAge = at.MaxAge;
                var x = rnd.Next(AnimalEnvironment.DimensionX);
                var y = rnd.Next(AnimalEnvironment.DimensionY);
                a.Position = Position.CreatePosition(x,y);
            }).Take(at.AmountToSpawn);
            foreach (var animal in animals)
            {
                AnimalEnvironment.Insert(animal);
            }
            Console.WriteLine(animals.Count()+" "+ at.Name + "s spawned");
        });
        Console.WriteLine(AnimalEnvironment.Entities.Count()+" animals inserted");
    }

    private void SpawnTrees()
    {
        
        var types = new List<TreeType>();
        TreeTypes.ForEach(s => types.Add( _entityManager.Create<TreeType>("Name", s)));
        Random rnd = new Random();
        types.ForEach(tt =>
        {
            var trees =_agentManager.Spawn<Tree.Tree,ForestLayer>(null, t =>
            {
                t.Name = tt.Name;
                t.ProductionRate = tt.ProductionRate;
                t.ConsumptionRate = tt.ConsumptionRate;
                t.GrowRate = tt.GrowRate;
                t.MatureAge = tt.MaxAge;
                t.MaxAge = tt.MaxAge;
                var x = rnd.Next(TreeEnvironment.DimensionX);
                var y = rnd.Next(TreeEnvironment.DimensionY);
                t.Position = Position.CreatePosition(x,y);
            }).Take(tt.AmountToSpawn);
            foreach (var tree in trees)
            {
                TreeEnvironment.Insert(tree);
            }
            Console.WriteLine(trees.Count()+" "+ tt.Name + "s spawned");
            
        });
        Console.WriteLine(TreeEnvironment.Entities.Count()+" trees inserted");

    }
    
    private void SpawnHumans(){}
}