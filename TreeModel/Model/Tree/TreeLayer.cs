using System;
using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using ServiceStack;
using TreeModel.Model.Animal;
using TreeModel.Model.Environment;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class TreeLayer : RasterLayer, ITreeLayer
    {
        public SpatialHashEnvironment<Tree> Environment;
        private IAgentManager agentManager;

        public AnimalLayer AnimalLayer { get; set; }
        public TerrainLayer TerrainLayer { get; set; }


        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
            UnregisterAgent unregisterAgent)
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
            agentManager = layerInitData.Container.Resolve<IAgentManager>();
            Environment = new SpatialHashEnvironment<Tree>(Width, Height);


            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var type = this[x, y];
                    var position = Position.CreatePosition(x, y);
                    CreateTree(type, position);
                }
            }

            Console.WriteLine(Environment.Entities.Count() + "Trees spawned");
            return true;
        }

        // We can improve the Parameter: with more Attribute from the Tree
        public Tree CreateTree(double type, Position position)
        {
            Random rdm = new Random();
            var tree = type switch
            {
                //PalmTree max Age 30
                1 => agentManager.Spawn<Tree, TreeLayer>(null, t =>
                {
                    t.age = rdm.Next(31);
                    t.Position = position;
                    t.matureAge = rdm.Next(5, 10);
                    t.fruitConstant = 2128;
                    t.fruitRandom = new[] {5, 9};
                }).Take(1).First(),

                // BrazilNutTree max Age 500
                2 => agentManager.Spawn<Tree, TreeLayer>(null, t =>
                {
                    t.age = rdm.Next(501);
                    t.Position = position;
                    t.matureAge = rdm.Next(6, 20);
                    t.fruitConstant = 26250;
                    t.fruitRandom = new[] {12, 25};
                }).Take(2).First(),
                // NutmegTree max Age 60
                3 => agentManager.Spawn<Tree, TreeLayer>(null, t =>
                {
                    t.age = rdm.Next(61);
                    t.Position = position;
                    t.matureAge = rdm.Next(18, 22);
                    t.fruitConstant = 213;
                    t.fruitRandom = new[] {8, 12};
                }).Take(3).First(),
                _ => null
            };
            if (tree != null)
            {
                Environment.Insert(tree);
            }

            return tree;
        }

        public Tree CreatSeeding(double type, Position position)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(position))) return null;
            var tree = CreateTree(type, position);
            if (tree != null) tree.age = 0;
            return tree;
        }


        public int GatherWood(Position tree, int amount)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = Environment.Entities.First(t => t.Position.Equals(tree));
                foundTree.wood -= amount;
                if (foundTree.wood > 0)
                {
                    return amount;
                }

                var amountOfCutWood = amount - Math.Abs(foundTree.wood);
                foundTree.wood = 0;
                return amountOfCutWood;
            }

            return 0;
        }

        public int GatherFruit(Position tree, int amount)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = Environment.Entities.First(t => t.Position.Equals(tree));
                foundTree.fruit -= amount;
                if (foundTree.fruit > 0)
                {
                    return amount;
                }

                var amountOfCutFruit = amount - Math.Abs(foundTree.fruit);
                foundTree.fruit = 0;
                return amountOfCutFruit;
            }

            return 0;
        }

        public void HurtTree(Position tree, int amount)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                var foundTree = Environment.Entities.First(t => t.Position.Equals(tree));
                foundTree.LifePoints -= amount;
                if (foundTree.LifePoints < 1)
                {
                    foundTree.LifePoints = 0;
                    foundTree.Die();
                }
            }
        }

        public int WoodLeft(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).wood;
            }

            return -1;
        }

        public int FruitLeft(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).fruit;
            }

            return -1;
        }

        public double GetAge(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).age;
            }

            return -1;
        }

        public State GetState(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).state;
            }

            return State.Nothing;
        }

        public Specie GetSpecie(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).Specie;
            }

            return Specie.NotATree;
        }

        public bool IsAlive(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).alive;
            }

            return false;
        }

        public List<Position> ExploreTrees(Position explorer, int distance)
        {
            var result = Environment.Explore(explorer, distance).ToList().Map(t => t.Position);
            return result;
        }
    }
}