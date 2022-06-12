using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using ServiceStack;
using TreeModel.Model.Animal;
using TreeModel.Model.Human;
using TreeModel.Model.Shared;
using TreeModel.Model.Tree;

namespace TreeModel.Model.Environment;

public class ForestLayer : RasterLayer
{
    public SpatialHashEnvironment<Tree.Tree> TreeEnvironment;
    public SpatialHashEnvironment<Animal.Animal> AnimalEnvironment;
    public SpatialHashEnvironment<Human.Human> HumanEnvironment;
    //
    public TerrainLayer TerrainLayer { get; set; }
    public IAgentManager _agentManager;
    public IEntityManager _entityManager;
    

    [PropertyDescription]
    public List<string> AnimalTypes { get; set; }
    
    [PropertyDescription]
    public List<string> TreeTypes { get; set; }
    
    [PropertyDescription]
    public List<string> HumanTypes { get; set; }

    public InitialPositionLayer InitialPositionLayer { get; set; }
    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle, UnregisterAgent unregisterAgent)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        _agentManager = layerInitData.Container.Resolve<IAgentManager>();
        _entityManager = layerInitData.Container.Resolve<IEntityManager>();
        TreeEnvironment = new SpatialHashEnvironment<Tree.Tree>(Width, Height);
        AnimalEnvironment = new SpatialHashEnvironment<Animal.Animal>(Width, Height);
        HumanEnvironment = new SpatialHashEnvironment<Human.Human>(Width, Height);
        
        SpawnAnimals();
        SpawnTrees();
        //SpawnHumans();
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
                a.LifePoints = at.LifePoints;
                a.Energy = rnd.Next(0, (int)at.Energy);
                a.ConsumptionRate = at.ConsumptionRate;
                a.MatureAge= at.MatureAge * 365;
                a.MaxAge = at.MaxAge * 365;
                a.LifePoints = at.LifePoints;
                a.Poop2Tree = at.Poop2Tree;
                a.ReproduceRate = at.ReproduceRate;
                if (InitialPositionLayer.SpawnPositionsAnimal.IsEmpty())
                {
                    var x = rnd.Next(AnimalEnvironment.DimensionX);
                    var y = rnd.Next(AnimalEnvironment.DimensionY);
                    a.Position = Position.CreatePosition(x, y);
                }
                else
                {
                    a.Position = InitialPositionLayer.SpawnPositionsAnimal[0];
                    InitialPositionLayer.SpawnPositionsAnimal.RemoveAt(0);
                }
            }).Take(at.AmountToSpawn);
            foreach (var animal in animals)
            {
                AnimalEnvironment.Insert(animal);
            }
            Console.WriteLine(animals.Count()+" "+ at.Name + "s spawned");
        });
        Console.WriteLine(AnimalEnvironment.Entities.Count()+" animals inserted");
    }
    
    public void Reproduce(Animal.Animal inputAnimal)
    {
        Position newpos = inputAnimal.Position; 
        
        var animal = _agentManager.Spawn<Animal.Animal, ForestLayer>(null, a =>
        {
            Random rnd = new Random();
            a.Age = 1;
            a.MovementSpeed = inputAnimal.MovementSpeed;
            a.Carnivore = inputAnimal.Carnivore;
            a.Herbivore = inputAnimal.Herbivore;
            a.Name = inputAnimal.Name;
            a.LifePoints = inputAnimal.LifePoints;
            a.Energy = 50;
            a.ConsumptionRate = inputAnimal.ConsumptionRate;
            a.MatureAge= inputAnimal.MatureAge * 365;
            a.MaxAge = inputAnimal.MaxAge * 365;
            a.LifePoints =100;
            a.Poop2Tree = inputAnimal.Poop2Tree;
            a.Position = newpos;
        } ) ;
        if (animal != null)
        {
            AnimalEnvironment.Insert(animal.First());
        }

    }
    
    public List<Position> ExploreAnimals(Position explorer, int distance)
    {
        var result = AnimalEnvironment.Explore(explorer, distance).ToList().Map(t => t.Position);
        return result;
    }

    //___________________________________________________________________________________________________________________________________
    public void SpawnTrees()
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
                t.Fruit = rnd.Next(0,(int)tt.Fruits);
                t.Wood = rnd.Next(0,(int)tt.Woods);
                t.GrowRate = tt.GrowRate;
                t.MatureAge = tt.MatureAge * 365;
                t.MaxAge = tt.MaxAge * 365;
                if (InitialPositionLayer.SpawnPositionsTree.IsEmpty())
                {
                    var x = rnd.Next(TreeEnvironment.DimensionX);
                    var y = rnd.Next(TreeEnvironment.DimensionY);
                    t.Position = Position.CreatePosition(x, y);
                }
                else
                {
                    t.Position = InitialPositionLayer.SpawnPositionsTree[0];
                    InitialPositionLayer.SpawnPositionsTree.RemoveAt(0);
                }
            }).Take(tt.AmountToSpawn);
            foreach (var tree in trees)
            {
                TreeEnvironment.Insert(tree);
            }
            Console.WriteLine(trees.Count()+" "+ tt.Name + "s spawned");
            
        });
        Console.WriteLine(TreeEnvironment.Entities.Count()+" trees inserted");

    }
    
    public void Spread(Tree.Tree inputTree, Position position)
    {
        Position newpos; 
        // We ask first if the posotion we want to spawn tree on already have tree on it 
        if (TreeEnvironment.Entities.Any(t => t.Position.Equals(position)))
        {
            // If it not free
            // RandomeSpot
            newpos = NewRandomeLocation();
        }
        else
        {
            newpos = position; 
        }
        var tree = _agentManager.Spawn<Tree.Tree, ForestLayer>(null, t =>
            {
                t.Name = inputTree.Name;
                t.ProductionRate = inputTree.ProductionRate;
                t.ConsumptionRate = inputTree.ConsumptionRate;
                t.GrowRate = inputTree.GrowRate;
                t.MatureAge = inputTree.MatureAge * 365;
                t.MaxAge = inputTree.MaxAge * 365;
                t.Position = newpos;

            } ) ;

        if (tree != null)
        {
            TreeEnvironment.Insert(tree.First());
        }

    }
    
     public double GatherWood(Position tree, double amount)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = TreeEnvironment.Entities.First(t => t.Position.Equals(tree));
                foundTree.Wood -= amount;
                if (foundTree.Wood > 0)
                {
                    return amount;
                }

                var amountOfCutWood = amount - Math.Abs(foundTree.Wood);
                foundTree.Wood = 0;
                return amountOfCutWood;
            }

            return 0;
        }

        public double GatherFruit(Position tree, double amount)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = TreeEnvironment.Entities.First(t => t.Position.Equals(tree));
                foundTree.Fruit -= amount;
                if (foundTree.Fruit > 0)
                {
                    return amount;
                }

                var amountOfCutFruit = amount - Math.Abs(foundTree.Fruit);
                foundTree.Fruit = 0;
                return amountOfCutFruit;
            }

            return 0;
        }

        public void HurtTree(Position tree, double amount)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = TreeEnvironment.Entities.First(t => t.Position.Equals(tree));
                foundTree.LifePoints -= amount;
                if (foundTree.LifePoints < 1)
                {
                    foundTree.LifePoints = 0;
                    foundTree.Die();
                }
            }
        }

        public double WoodLeft(Position tree)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(tree)).Wood;
            }

            return -1;
        }

        public double FruitLeft(Position tree)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(tree)).Fruit;
            }

            return -1;
        }

        public int GetAge(Position tree)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(tree)).Age;
            }

            return -1;
        }

        public Tree.Tree GetTree(Position pos)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(pos)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(pos));
            }
            return null;
        }

        public State GetState(Position tree)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(tree)).State;
            }

            return State.Nothing;
        }
        

        public bool IsAlive(Position tree)
        {
            if (TreeEnvironment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return TreeEnvironment.Entities.First(t => t.Position.Equals(tree)).Alive;
            }

            return false;
        }

        public List<Position> ExploreTrees(Position explorer, int distance)
        {
            var result = TreeEnvironment.Explore(explorer, distance).ToList().Map(t => t.Position);
            return result;
        }
        
        
//_______________________________________________________________________________________________________________________
    private void SpawnHumans()
    {
        var types = new List<HumanType>();
        HumanTypes.ForEach(s => types.Add( _entityManager.Create<HumanType>("Name", s)));
        Random rnd = new Random();
        types.ForEach(ht =>
        {
            var humans =_agentManager.Spawn<Human.Human,ForestLayer>(null, h =>
            {
                h.Alive = ht.Alive;
                h.Movement = ht.Movement;
                h.WoodConsumption = ht.WoodConsumption;
                h.WoodStorage = ht.WoodStorage;
                h.PlantingRate = ht.PlantingRate;
                h.HarvestRate = ht.HarvestRate;
                h.Damage = ht.Damage;
                if (InitialPositionLayer.SpawnPositionsHuman.IsEmpty())
                {
                    var x = rnd.Next(HumanEnvironment.DimensionX);
                    var y = rnd.Next(HumanEnvironment.DimensionY);
                    h.Position = Position.CreatePosition(x, y);
                }
                else
                {
                    h.Position = InitialPositionLayer.SpawnPositionsHuman[0];
                    InitialPositionLayer.SpawnPositionsHuman.RemoveAt(0);
                }
            }).Take(ht.AmountToSpawn);
            foreach (var human in humans)
            {
                HumanEnvironment.Insert(human);
            }
            Console.WriteLine(humans.Count()+" "+ ht.Name + "s spawned");
        });
        Console.WriteLine(HumanEnvironment.Entities.Count()+" humans inserted");
    }


    public Position NewRandomeLocation()
    {
        var rnd = new Random();
        var x = TreeEnvironment.DimensionX;
        x = rnd.Next(x);
        var y = TreeEnvironment.DimensionY;
        y = rnd.Next(y);
         return new Position(x, y);
    }
}