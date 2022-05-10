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
        private List<Tree> _trees;
        private SpatialHashEnvironment<Tree> Environment;
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle ,
            UnregisterAgent unregisterAgent )
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            Environment = new SpatialHashEnvironment<Tree>(Width,Height);
            _trees= agentManager.Spawn<Tree, TreeLayer>().ToList();
            return true;
        }

        public int GatherWood(Position tree, int amount)
        {
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
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
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
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
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
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
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
                return foundTree.wood;
            }
            return -1;
        }

        public int FruitLeft(Position tree)
        {
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
                return foundTree.fruit;
            }
            return -1;
        }

        public int GetAge(Position tree)
        {
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
                return foundTree.age;
            }
            return -1;
        }

        public bool IsAlive(Position tree)
        {
            var foundTree = _trees.Find(t => t.Position.Equals(tree));
            if (foundTree !=null)
            {
                return foundTree.alive;
            }
            return false;
        }

        public List<Position> ExploreTrees(Position explorer, int distance)
        {
            double xmin = explorer.X - distance;
            double xmax = explorer.X + distance;
            double ymin = explorer.Y - distance;
            double ymax = explorer.Y + distance;
            var result = _trees.Map(t => t.Position).FindAll(position =>
            {
                if (position.X <= xmax && position.X >= xmin && position.Y <= ymax && position.Y >= ymin) return true;
                return false;
            } );
            return result;
        }
    }
}
