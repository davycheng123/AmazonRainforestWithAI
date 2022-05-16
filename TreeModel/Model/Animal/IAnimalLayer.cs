using System.Collections.Generic;
using Mars.Interfaces.Environments;

namespace TreeModel.Model.Animal
{
    public interface IAnimalLayer
    {
        /// <summary>
        /// Inspection whether it is alive or not
        /// </summary>
        /// <returns>
        /// true if alive, false otherwise
        /// </returns>
        bool IsAlive(Position tree);

        /// <summary>
        /// Exploration from a given position with a view distance to find animals.
        /// </summary>
        /// <returns>
        /// A list that contains all the animals found
        /// </returns>
        //List<Position> ExploreAnimals(Position explorer, int distance);
    }
}
