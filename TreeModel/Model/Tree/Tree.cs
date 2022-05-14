using System;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<TreeLayer>
    {
        public void Init(TreeLayer layer)
        {
            TreeLayer = layer;
            //Console.WriteLine(Position);
        }


        public void Tick()
        {
        }

        public void Grow()
        {
        }

        public void ProduceFruits()
        {
        }

        public void Spread()
        {
        }

        public void Die()
        {
        }

        public Resilience resilience { get; set; }
        
        public int growthRate { get; set; }
        
        public int fruit { get; set; }
        
        public bool alive { get; set; }

        public int age { get; set; }

        public int wood { get; set; }

        public TreeLayer TreeLayer { get; private set; }
        public Position Position { get; set; }
        
        public int LifePoints { get; set; }
            
        // identifies the agent
        public Guid ID { get; set; }
    }
}
