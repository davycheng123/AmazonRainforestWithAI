using Mars.Interfaces.Agents;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Human
{
    public interface IHuman<in T> : IAgent<T> where T : ILayer
    {
        /// <summary>
        /// This moves the humans to a different location
        /// </summary>
        void Move();

        /// <summary>
        /// Cut down the tree Remove it our of existence
        /// </summary>
        void KillTree();

        /// <summary>
        /// There are a chance that Human harvest fruits
        /// </summary>
        void FruitHarvest();

        /// <summary>
        /// There are a chance that Human plan tree
        /// </summary>
        void PlantingTree();
    }
}
