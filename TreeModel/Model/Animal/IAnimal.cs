using Mars.Interfaces.Agents;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Animal
{
    public interface IAnimal<in T> : IAgent<T>, IPositionable where T : ILayer
    {
        /// <summary>
        /// This moves the animal to a different location
        /// </summary>
        void Move();

        /// <summary>
        /// Eats fruits
        /// </summary>
        void Consume();

        /// <summary>
        /// Fertilizes the soil and helps the spread of trees
        /// </summary>
        void Poop();

        /// <summary>
        /// This kills the animal but it's corpse remains until it decays
        /// </summary>
        void Die();
    }
}