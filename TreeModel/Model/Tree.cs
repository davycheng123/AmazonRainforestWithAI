using System;
using Mars.Common;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;

namespace RunnerModel.Model
{
    public class Tree : IAgent<TreeLayer>, IPositionable
    {
        [PropertyDescription (Name ="xpos")]
        public double PosX {get; set;}

        [PropertyDescription (Name ="ypos")]
        public double PosY {get; set;}

        public void Init(TreeLayer layer)
        {
            TreeLayer = layer;
            Position = Position.CreatePosition(PosX, PosY);
            layer.Environment.Insert(this);
        }

        public TreeLayer TreeLayer { get; private set; }
        public Position Position { get; set; }
        
        public void Tick()
        { 
            var goal = TreeLayer.Goal;
            if(!goal.Equals(Position)) {
                var bearing = PositionHelper.CalculateBearingCartesian(Position.X, Position.Y, goal.X, goal.Y);
                TreeLayer.Environment.MoveTowards(this, bearing, 1);
            }
        }

        // identifies the agent
        public Guid ID { get; set; }
    }
}
