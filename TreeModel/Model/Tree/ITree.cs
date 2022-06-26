using Mars.Interfaces.Agents;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Tree
{
    public interface ITree<in T> : IAgent<T>, IPositionable where T : ILayer
    {
        /// <summary>
        /// Increases wood mass, Age and Fruits.
        /// This action reduces nutrients in the soil
        /// </summary>
        void Grow();

        /// <summary>
        /// Increases the amount of fruits.
        /// Only adult trees produce fruit
        /// </summary>
        void ProduceFruits();
        
        /// <summary>
        /// Increases the amount of woos.
        /// </summary>
        void IncreaseWood();


        /// <summary>
        /// This kills the tree but it's corpse remains until it decays or was harvested
        /// </summary>
        void Die();
        
        /// <summary>
        /// Check If the Tree old enough to change state
        /// </summary>
        void CheckState();
        
    }
}