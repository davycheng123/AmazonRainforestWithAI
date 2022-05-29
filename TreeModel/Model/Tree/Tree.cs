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
        public int AmountToSpawn { get; set; } = 9;
        public int MaxAge { get; set; }
        public int MatureAge { get; set; }          
        public double ConsumptionRate { get; set;}  //For the Nutrient and the Water
        public double GrowRate { get; set; }        //For the Wood  
        public double ProductionRate { get; set; }  //For the Fruit 
        
        
        public ForestLayer ForestLayer { get; set; }
        public State State { get; set; } = State.Seedling;
        public int Age { get; set; } = 1;
        public double Wood { get; set; } = 0;
        
        public bool Alive { get; set; }
        public double Fruit { get; set; } = 0;

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
            //if(rnd.Next(10000) % AmountToSpawn == 0) Spread();
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

        public void ProduceFruits()
        {
            if (State == State.Adult) Fruit += 1 * ProductionRate;
        }

        private void CheckAlive()
        {
            if (LifePoints < 0 || Age >MaxAge) Die();
        }
        
        //TODO: Improve
        public void Spread()
        {
             var animalNearby = ForestLayer.AnimalEnvironment.Explore(Position, 5).ToList().Map(t => t.Position).First();
             if (animalNearby == null)
             {
                 return;
             }
             
             if (ForestLayer.TreeEnvironment.Entities.Any(t => t.Position.Equals(Position)))
             {
                 return;
             }

             var positionTree = ForestLayer.TreeEnvironment.Entities.Any(t => t.Position.Equals(animalNearby));
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
                     t.Position = animalNearby;

                 } ) ;
                 
                 if (tree != null)
                 {
                     ForestLayer.TreeEnvironment.Insert(tree.First());
                 }
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
/*
public void Init(ForestLayer layer)
{
    var random = new Random();
    ForestLayer = layer;
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

    //Spread();

    // Check on Life Point
    if (LifePoints == 0)
    {
        alive = false;
        Die();
    }

    // Check on Wood Value
    if (wood == 0)
    {
        ForestLayer.Environment.Remove(this);
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

    var nutrient = ForestLayer.TerrainLayer.GetSoilNutrients(Position);
    var water = ForestLayer.TerrainLayer.GetWaterLevel(Position);

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
    var animalNearby = ForestLayer.AnimalLayer.ExploreAnimals(Position, 5).Any();
    var distance = 10;
    if (animalNearby) distance += 5;
    
    switch (Specie)
    {
        case Specie.NutmegTree:
            ForestLayer.CreatSeeding(1, Position.CreatePosition(Position.X + distance, Position.Y + distance));
            break;
        case Specie.PalmTree:
            ForestLayer.CreatSeeding(2, Position.CreatePosition(Position.X + distance, Position.Y + distance));
            break;
        case Specie.BrazilNutTree:
            ForestLayer.CreatSeeding(3, Position.CreatePosition(Position.X + distance, Position.Y + distance));
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

public ForestLayer ForestLayer { get; private set; }
public Position Position { get; set; }

public int LifePoints { get; set; }

// identifies the agent
public Guid ID { get; set; }
}
}
*/