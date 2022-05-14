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

namespace TreeModel.Model.Tree
{
    public class TreeLayer : RasterLayer, ITreeLayer
    {

        public SpatialHashEnvironment<Tree> Environment;

        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
            UnregisterAgent unregisterAgent)
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            Environment = new SpatialHashEnvironment<Tree>(Width, Height);
            
            
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var type = this[x, y];
                    var position = Position.CreatePosition(x,y);
                    var tree = CreateTree(agentManager, type, position);
                    if ( tree != null)
                    {
                        Environment.Insert(tree);
                    }
                }
            }

            Console.WriteLine(Environment.Entities.Count() +"Trees spawned");
            return true;
        }
        
        private Tree CreateTree(IAgentManager agentManager,double type, Position position)
        {
            return type switch
            {
                1 => agentManager.Spawn<Tree, TreeLayer>(null, t => t.Position = position).Take(1).First(),
                _ => null
            };
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

        public int GetAge(Position tree)
        {
            if (Environment.Entities.Any(t => t.Position.Equals(tree)))
            {
                return Environment.Entities.First(t => t.Position.Equals(tree)).age;
            }

            return -1;
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