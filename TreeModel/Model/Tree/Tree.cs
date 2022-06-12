using System;

using System.Linq;
using Mars.Interfaces.Environments;
using ServiceStack;

using TreeModel.Model.Environment;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<ForestLayer>
    {
        public string Name { get; set; }
        public int AmountToSpawn { get; set; } = 365;
        public int MaxAge { get; set; }
        public int MatureAge { get; set; }          
        public double ConsumptionRate { get; set;}  //For the Nutrient and the Water
        public double GrowRate { get; set; }        //For the Wood  
        public double ProductionRate { get; set; }  //For the Fruit 
        
        
        public ForestLayer ForestLayer { get; set; }
        public State State { get; set; } = State.Seedling;
        public int Age { get; set; } = 1;
        public double Wood { get; set; } 
        
        public bool Alive { get; set; }
        public double Fruit { get; set; }

        public bool nowSpread { get; set; }
        public double LifePoints { get; set; } = 100;
        public void Init(ForestLayer layer)
        {
            ForestLayer = layer;
            Alive = true;
            nowSpread = false;
        }

        public void Tick()
        {
            if (Alive)
            {

            // The State will be check after every day to see if any changes occurs
            CheckState();
            
            // It will growth after every Ticks
            Grow();
            
            // The GrowRate only affect the wood amount 
            IncreaseWood();
                        
            // The ProductionRate only affect the Fruits amount
            ProduceFruits();
            
            // Spread the Tree
            Random rnd = new Random();
            var value = rnd.Next(10000000);
            
           if( value< 5 ) ForestLayer.Spread(this, ForestLayer.newRandomeLocation());
            }
            

                // Check if Tree over max Age, or Life points < 0. If Die then how much the Wood is Left
                CheckAlive();

            }
        

        public Guid ID { get; set; }
        public Position Position { get; set; }

        private void CheckState()
        {
            // We say that after a year itr will change state from seeding to Juvenile
            if (365<Age && Age< MatureAge)
            {
                State = State.Juvenile;
            }
            if (Age > MatureAge)
            {
                State = State.Adult;
            }
        }
        
        public void Grow()
        {
            // ConsumptionRate affect Water and Nutrient will increase LifePoints
            // Increase Age
            Age ++;
            // Increase the Life points
            LifePoints += ConsumptionRate;
            ForestLayer.TerrainLayer.RemoveWater(Position, ConsumptionRate);
            ForestLayer.TerrainLayer.RemoveWater(Position, ConsumptionRate);

        }
        

        public void IncreaseWood()
        {
            switch(State)
            {
                case State.Seedling:break;
                case State.Juvenile:
                    Wood += 1 * GrowRate * 0.5;
                    break;
                case State.Adult:
                    Wood += 1 * GrowRate;
                    break;
            }
        }
    
        // TODO: Affect of Water and Nutrient
        public void ProduceFruits()
        {
            
            if (State == State.Adult) Fruit += 1 * ProductionRate * 99 ;
        }

        private void CheckAlive()
        {
            if (LifePoints < 0 || Age >MaxAge) Die();
        }
        
        public void Spread(Position position)
        {
            if (nowSpread)
            {
                // Because the Animal alway on a tree so it should be planting a tree close to it 
                Random rand = new Random();
                var newX = position.X + rand.Next(3);
                var newY = position.Y + 1;
                Position newPos = new Position(newX, newY);

                // Ask if any Tree are at the location
                var positionTree = ForestLayer.TreeEnvironment.Entities.Any(t => t.Position.Equals(newPos));
                if (positionTree != true)
                {
                    var tree = ForestLayer._agentManager.Spawn<Tree, ForestLayer>(null, t =>
                    {
                        t.Name = Name;
                        t.ProductionRate = ProductionRate;
                        t.ConsumptionRate = ConsumptionRate;
                        t.GrowRate = GrowRate;
                        t.MatureAge = MatureAge * 365;
                        t.MaxAge = MaxAge * 365;
                        t.Position = newPos;

                    });

                    if (tree != null)
                    {
                        ForestLayer.TreeEnvironment.Insert(tree.First());
                    }
                }

                nowSpread = false;
            }
            
        }


        public void Die()
        {
            Alive = false;
            // Check on Wood Value
            if (Wood == 0)
            {
                ForestLayer.TreeEnvironment.Remove(this);
            }
            
            State = State.DeadWood;
            GrowRate = 0;
            ProductionRate = 0;
            ConsumptionRate = -5;
            
            // fruits decay over time
            if (Fruit < 100)
                Fruit = 0;
            else
                Fruit = (int) (Fruit * 0.9);
            
        }
    }
}
