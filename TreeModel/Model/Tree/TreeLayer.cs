using System.Collections.Generic;
using System.Linq;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace TreeModel.Model.Tree
{
    public class TreeLayer : AbstractLayer, ITreeLayer
    {
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
            UnregisterAgent unregisterAgent = null)
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            agentManager.Spawn<Tree, TreeLayer>().ToList();
            return true;
        }

        public int GatherWood(Position tree, int amount)
        {
            return 0;
        }

        public int GatherFruit(Position tree, int amount)
        {
            return 0;
        }

        public void HurtTree(Position tree, int amount)
        {
        }

        public int WoodLeft(Position tree)
        {
            return 0;
        }

        public int FruitLeft(Position tree)
        {
            return 0;
        }

        public int GetAge(Position tree)
        {
            return 0;
        }

        public bool IsAlive(Position tree)
        {
            return false;
        }

        public List<Position> ExploreTrees(Position explorer, int distance)
        {
            return null;
        }
    }
}
