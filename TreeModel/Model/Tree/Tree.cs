using System;
using System.Diagnostics;
using System.Linq;
using Mars.Interfaces.Environments;
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
            resilience = random.Next(8, 12) * 0.1;
            LifePoints = 1000;
            //Console.WriteLine(Position);
        }


        public void Tick()
        {
            
            // TODO: tree can get sick?
            // -> higher chance of infection if nearby trees get sick
            // Sick();
            
            // Change State
            if (age < matureAge)
            {
                state = State.Juvenile;
            }
            
            if (age > matureAge)
            {
                state = State.Adult;
            }
            //Console.Write(state);
            Grow();

            Spread();

            // Check on Life Point
            if (LifePoints == 0)
            {
                alive = false;
                Die();
            }

            // Check on Wood Value
            if (wood == 0)
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
            
            //Console.Write("Growing");
            double growthRate = 1;
            growthRate *= DayPerTick / 365.0;
            var rateEffect = growthRate * resilience;

            // Increase the Life point 

            var nutrient = TreeLayer.TerrainLayer.GetSoilNutrients(Position);
            var water = TreeLayer.TerrainLayer.GetWaterLevel(Position);

            if (nutrient != -1) LifePoints += (int)nutrient;
            
            if (water != -1) LifePoints += (int)water;

            LifePoints += 100;
            
            
            // Growing Age
            age += 0.1;
            var random = new Random();
            var woodGrowthPerYear = random.Next(100, 250); // in centemeter

            // check Age to change wood 
            switch(state)
            {
                case State.Seedling:break;
                case State.Juvenile:
                    wood += (int) (woodGrowthPerYear * rateEffect * 0.8);
                    break;
                case State.Adult:
                    wood += (int) (woodGrowthPerYear * rateEffect);

                    // Only Adult can produce Fruit
                    var fruitRate = random.Next(fruitRandom[0], fruitRandom[1]) * fruitConstant;
                    ProduceFruits(fruitRate * rateEffect);
                    break;
                default:
                    break;
            }

        }


        public void ProduceFruits(double rate)
        {
            fruit += (int) rate;
        }

        public void Spread()
        {
            var animalNearby = TreeLayer.AnimalLayer.ExploreAnimals(Position, 5).Any();
            var distance = 10;
            if (animalNearby) distance += 5;
            
            switch (Specie)
            {
                case Specie.NutmegTree:
                    TreeLayer.CreatSeeding(1, Position.CreatePosition(Position.X + distance, Position.Y + distance));
                    break;
                case Specie.PalmTree:
                    TreeLayer.CreatSeeding(2, Position.CreatePosition(Position.X + distance, Position.Y + distance));
                    break;
                case Specie.BrazilNutTree:
                    TreeLayer.CreatSeeding(3, Position.CreatePosition(Position.X + distance, Position.Y + distance));
                    break;
            }
        }

        public void Die()
        {
            state = State.DeadWood;
            // this.growthRate = 0;
            // fruits decay over time
            if (fruit < 100)
                fruit = 0;
            else
                fruit = (int) (fruit * 0.9);
            resilience = 0;
        }

        public int DayPerTick = 1;

        public double resilience { get; set; }

        // public int growthRate { get; set; }

        public int fruit { get; set; }

        public bool alive { get; set; }

        public double age { get; set; } // in year

        public int matureAge { get; set; }

        public int fruitConstant { get; set; }

        public int[] fruitRandom { get; set; }

        public int wood { get; set; } // in terms of height in centimeter

        public Specie Specie { get; set; }

        public State state { get; set; }

        public TreeLayer TreeLayer { get; private set; }
        public Position Position { get; set; }

        public int LifePoints { get; set; }

        // identifies the agent
        public Guid ID { get; set; }
    }
}