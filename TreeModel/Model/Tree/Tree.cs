using System;

using Mars.Interfaces.Environments;

using TreeModel.Model.Environment;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<ForestLayer>
    {
        public string Name { get; set; }
        public int MaxAge { get; set; }
        public int MatureAge { get; set; }          
        public double ConsumptionRate { get; set;}  //For the Nutrient and the Water
        public double GrowRate { get; set; }        //For the Wood  
        public double ProductionRate { get; set; }  //For the Fruit 
        
        
        public ForestLayer ForestLayer { get; set; }
        public State State { get; set; } = State.Juvenile;
        public int Age { get; set; } = 1;
        public double Wood { get; set; } 
        
        public bool Alive { get; set; }
        public double Fruit { get; set; }
        
        public double SpreadRate { get; set; }
        
        public double LifePoints { get; set; } = 100;
        public void Init(ForestLayer layer)
        {
            ForestLayer = layer;
            Alive = true;

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
                var value = rnd.NextDouble();
            
                if( value< SpreadRate ) ForestLayer.Spread(this, ForestLayer.NewRandomLocation());
            }
            // Check if Tree over max Age, or Life points < 0. If Die then how much the Wood is Left
            CheckAlive();

            }
        

        public Guid ID { get; set; }
        public Position Position { get; set; }

        public void CheckState()
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
                    Wood += NutrAndWaterEffect(Position) * GrowRate * 0.5;
                    break;
                case State.Adult:
                    Wood += NutrAndWaterEffect(Position) * GrowRate;
                    break;
            }
        }
    
        public void ProduceFruits()
        {
            
            if (State == State.Adult) Fruit += NutrAndWaterEffect(Position) * ProductionRate * 99 ;
        }

        private void CheckAlive()
        {
            if (LifePoints < 0 || Age >MaxAge) Die();
        }
        

        public void Die()
        {
            ForestLayer.TreeEnvironment.Remove(this);
            Alive = false;
            // Check on Wood Value
            if (Wood == 0)
            {
                
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

        private double NutrAndWaterEffect(Position position)
        {
            return ((ForestLayer.TerrainLayer.GetSoilNutrients(position) + ForestLayer.TerrainLayer.GetWaterLevel(position))/100) +1;
        }
    }
}
