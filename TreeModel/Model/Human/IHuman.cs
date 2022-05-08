using Mars.Interfaces.Agents;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Human
{
    public interface IHuman<in T> : IAgent<T> where T : ILayer
    {
    }
}