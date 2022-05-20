using System;
using System.Linq;
using System.Linq.Expressions;
using Mars.Interfaces.Environments;
using Microsoft.CodeAnalysis.FlowAnalysis;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<TreeLayer>
    {

        public void Init(TreeLayer layer)
        {
            var random = new Random();
            TreeLayer = layer;
            wood = random.Next(100, 250);
            resilience = random.Next(8, 12)*0.1;
            //Console.WriteLine(Position);
        }


        public void Tick()
        {
            Grow();
            // TODO: tree can get sick?
            // -> higher chance of infection if nearby trees get sick
            // Sick();
            
            // Check on Life Point
            if (LifePoints == 0)
            {
                this.alive = false;
                Die();
            }
            
            // Check on Wood Value
            if (this.wood == 0)
            {
                TreeLayer.Environment.Remove(this);
            }
        }

        public void Grow()
        {
            // Because every Tree have it own grown rate so we have to calculate the rate of it
            // Affect from enviroment are: Nutrient and Water (which are also affected by weather)
            // We have a formula or sth: rate_effect = (Nutrient + Water )* rate
            // TODO: Come up with a formula that calculate how all the element affect the Tree
            // Example
            // var rate = this.growthRate;
            // double rateEffect = rate - this.resilience;
            double growthRate = 1;
            growthRate *= DayPerTick / 365.0;
            var rateEffect = growthRate * this.resilience;


            // Growing Age
            this.age *= growthRate;
            var random = new Random();
            var woodGrowthPerYear = random.Next(100, 250);  // in centemeter
            
            // check of it enough age to change State 
            if (age < matureAge)
            {
                this.state = State.Juvenile;
                this.wood += (int) (woodGrowthPerYear * rateEffect * 0.8);
            } 
            else
            {
                this.state = State.Adult;
                this.wood += (int) (woodGrowthPerYear * rateEffect);
            }

            // Only Adult can produce Fruit
            if (this.state == State.Adult)
            {
                var fruitRate = random.Next(this.fruitRandom[0], this.fruitRandom[1]) * this.fruitConstant;
                ProduceFruits(fruitRate * rateEffect);
            }
        }
        
        

        public void ProduceFruits(double rate)
        {
            this.fruit += (int) rate;
        }

        public void Spread()
        {
            
            var AnimalNearby = TreeLayer.AnimalLayer.ExploreAnimals(Position, 5).Any();
            var Distance = 10;
            if (AnimalNearby == true) Distance += 5;
            
            switch(this.Specie)
            {
                case Specie.NutmegTree:
                    TreeLayer.CreateTree(1, Position.CreatePosition(Position.X + Distance, Position.Y + Distance));
                    break;
                case Specie.PalmTree:
                    TreeLayer.CreateTree(2, Position.CreatePosition(Position.X + Distance, Position.Y + Distance)); 
                    break;
                case Specie.BrazilNutTree:
                    TreeLayer.CreateTree(3, Position.CreatePosition(Position.X + Distance, Position.Y + Distance)); 
                    break;
            }

        }

        public void Die()
        {
            this.state = State.DeadWood;
            // this.growthRate = 0;
            // fruits decay over time
            if (this.fruit < 100) 
                this.fruit = 0;
            else  
                this.fruit = (int) (this.fruit * 0.9);
            this.resilience = 0;
        }

        public int DayPerTick = 1;

        public double resilience { get; set; }
        
        // public int growthRate { get; set; }
        
        public int fruit { get; set; }
        
        public bool alive { get; set; }

        public double age { get; set; }     // in year
        
        public int matureAge { get; set; }
        
        public int fruitConstant { get; set; }
        
        public int[] fruitRandom { get; set; }
        
        public int wood { get; set; }   // in terms of height in centimeter
        
        public Specie Specie { get; set; }
        
        public State state { get; set; }

        public TreeLayer TreeLayer { get; private set; }
        public Position Position { get; set; }
        
        public int LifePoints { get; set; }
            
        // identifies the agent
        public Guid ID { get; set; }
    }
}
