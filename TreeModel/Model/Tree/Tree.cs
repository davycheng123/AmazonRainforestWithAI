using System;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<TreeLayer>
    {
        public void Init(TreeLayer layer)
        {
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

        // identifies the agent
        public Guid ID { get; set; }
    }
}
