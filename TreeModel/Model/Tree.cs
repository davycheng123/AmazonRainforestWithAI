using System;
using Mars.Common;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using RunnerModel.Model.Shared;

namespace RunnerModel.Model
{
    public class Tree : IAgent<TreeLayer>, IPositionable
    {
        // PropertyDescription are for the .csv file, that store the data
        [PropertyDescription (Name ="xpos")]
        public double PosX {get; set;}

        [PropertyDescription (Name ="ypos")]
        public double PosY {get; set;}
        
        [PropertyDescription (Name ="specie")]
        public Specie specie {get; set;}
        
        
        /*  Attribute from Tree are:
         *      - Position: X,Y
         *      - Enum: specie
         *      - int: wood
         *      - int: age
         *      - int: growthRate
         *      - int: fruit
         *      - bool: alive
         *      - Enum: resilience
         */     
        
        public void Init(TreeLayer layer)
        {
            // declare Layer
            TreeLayer = layer;
            //____________Location and Specie will be declared in a csv Data______________
            // Location 
            Position = Position.CreatePosition(PosX, PosY);
            layer.Environment.Insert(this);
            // Switches Case for the specie
            switch (specie)
            {
                case Specie.PalmTree:
                {
                    wood = 1;
                    age = 1;
                    growthRate = 1;
                    fruit = 1;
                    alive = true;
                    resilience = Resilience.Medium;
                    break;
                }
                    ;
                case Specie.NutmegTree:
                {
                    wood = 1;
                    age = 1;
                    growthRate = 1;
                    fruit = 1;
                    alive = true;
                    resilience = Resilience.Medium;
                    break;
                }
                    ;
                case Specie.BrazilNutTree:
                {
                    wood = 1;
                    age = 1;
                    growthRate = 1;
                    fruit = 1;
                    alive = true;
                    resilience = Resilience.Medium;
                    break;
                }
            }
        }
        
        public Resilience resilience { get; set; }
        
        public int growthRate { get; set; }
        
        public int fruit { get; set; }
        
        public bool alive { get; set; }

        public int age { get; set; }

        public int wood { get; set; }

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


        public Guid ID { get; set; }
    }
}
