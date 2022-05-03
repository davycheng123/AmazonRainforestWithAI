using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace RunnerModel.Model
{
    public class TreeLayer : AbstractLayer
    {
        [PropertyDescription]
        public int MaxX {get; set;}

        [PropertyDescription]
        public int MaxY {get; set;}

        public SpatialHashEnvironment<Tree> Environment {get; private set;}
        
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
            UnregisterAgent unregisterAgent = null)
        {
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);
        
            Environment = new SpatialHashEnvironment<Tree>(MaxX -1, MaxY -1);
        
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();
            agentManager.Spawn<Tree, TreeLayer>().ToList();
        
            var random = new System.Random();
            Goal = Position.CreatePosition(random.Next(MaxX), random.Next(MaxY));
        
            return true;
        }

        public Position Goal {get; private set;}
    }
}


