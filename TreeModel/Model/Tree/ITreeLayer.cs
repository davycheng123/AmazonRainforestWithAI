using System.Collections.Generic;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Tree
{
    public interface ITreeLayer : ILayer
    {
        /// <summary>
        /// Removes x amount of wood from a tree
        /// </summary>
        /// <returns>
        /// The removed amount
        /// </returns>
        int GatherWood(Position tree, int amount);

        /// <summary>
        /// Removes x amount of fruit from a tree
        /// </summary>
        /// <returns>
        /// The removed amount
        /// </returns>
        int GatherFruit(Position tree, int amount);

        /// <summary>
        /// Deals damage to a tree
        /// </summary>
        void HurtTree(Position tree, int amount);

        /// <summary>
        /// Inspection of the remaining wood amount of a tree
        /// </summary>
        /// <returns>
        /// The remaining amount
        /// </returns>
        int WoodLeft(Position tree);

        /// <summary>
        /// Inspection of the remaining  fruit amount of a tree
        /// </summary>
        /// <returns>
        /// The remaining amount
        /// </returns>
        int FruitLeft(Position tree);

        /// <summary>
        /// Inspection of the age of a tree
        /// </summary>
        /// <returns>
        /// The age
        /// </returns>
        int GetAge(Position tree);

        /// <summary>
        /// Inspection whether it is alive or not
        /// </summary>
        /// <returns>
        /// true if alive, false otherwise
        /// </returns>
        bool IsAlive(Position tree);

        /// <summary>
        /// Exploration from a given position with a view distance to find trees.
        /// </summary>
        /// <returns>
        /// A list that contains all the trees found
        /// </returns>
        List<Position> ExploreTrees(Position explorer, int distance);
    }
}