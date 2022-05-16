using System;
using System.Linq.Expressions;
using Mars.Interfaces.Environments;
using TreeModel.Model.Shared;

namespace TreeModel.Model.Tree
{
    public class Tree : ITree<TreeLayer>
    {

        public void Init(TreeLayer layer)
        {
            TreeLayer = layer;
            //Console.WriteLine(Position);
        }


        public void Tick()
        {
            Grow();
            
            // Check on Life Point
            if (LifePoints == 0)
            {
                this.alive = false;
                Die();
            }
            
            // Check on Wood VAlue
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
            // TODO: Come up with a formular that calculate how all the element affect the Tree
            var rate = this.growthRate;
            // Example
            double rate_effect = rate - this.resilience;


            // Growing Age
            this.age += rate;
            // check of it enough age to change State 
            // TODO: Declare on which age it will change state
            if (age > 100 && age < 1000)
            {
                this.state = State.Juvenile;
                // TODO: Add an amount of wood
                this.wood += 100;
            }

            if (age > 1000)
            {
                this.state = State.Adult;
                // TODO: Add an amount of wood
                this.wood += 1000;
            }

            // Only Adult can produce Fruit
            if (this.state == State.Adult)
            {
                ProduceFruits(rate_effect);

            }
        }

        public void ProduceFruits(double rate)
        {
            this.fruit += (int) rate;
        }

        public void Spread()
        {
            switch(this.Specie)
            {
                case Specie.NutmegTree:
                    TreeLayer.CreateTree(1, Position.CreatePosition(Position.X + 10, Position.Y + 10)); 
                    break;
                    
                case Specie.PalmTree:
                    TreeLayer.CreateTree(2, Position.CreatePosition(Position.X + 10, Position.Y + 10)); 
                    break;
                case Specie.BrazilNutTree:
                    TreeLayer.CreateTree(3, Position.CreatePosition(Position.X + 10, Position.Y + 10)); 
                    break;
            }
            

        }

        public void Die()
        {
            this.state = State.DeadWood;
            this.growthRate = 0;
            // TODO: We can say that the fruits decay over time
            this.fruit -= 100;
            this.resilience = 0;
        }

        public int resilience { get; set; }
        
        public int growthRate { get; set; }
        
        public int fruit { get; set; }
        
        public bool alive { get; set; }

        public int age { get; set; }

        public int wood { get; set; }
        
        public Specie Specie { get; set; }
        
        public State state { get; set; }

        public TreeLayer TreeLayer { get; private set; }
        public Position Position { get; set; }
        
        public int LifePoints { get; set; }
            
        // identifies the agent
        public Guid ID { get; set; }
    }
}
