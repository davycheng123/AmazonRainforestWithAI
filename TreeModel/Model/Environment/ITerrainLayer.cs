using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Environment
{
    public interface ITerrainLayer : IDataLayer
    {
        /// <returns>
        ///  The remaining water in the soil for a given position
        /// </returns>
        double GetWaterLevel(Position position);

        /// <returns>
        ///  The remaining nutrients in the soil for a given position
        /// </returns>
        double GetSoilNutrients(Position position);

        /// <summary>
        /// Removes water from a position
        /// </summary>
        /// <returns>
        ///  The amount of removed water
        /// </returns>
        double RemoveWater(Position position, double amount);

        /// <summary>
        /// Removes nutrients from a position
        /// </summary>
        /// <returns>
        ///  The amount of removed nutrients
        /// </returns>
        double RemoveSoilNutrients(Position position, double amount);
    }
}