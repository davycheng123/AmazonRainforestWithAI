using System;
using Mars.Interfaces.Environments;
using System.Collections.Generic;
using TreeModel.Model.Environment;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<ForestLayer>
{
    private List<Position> _adultTree;
    public ForestLayer ForestLayer { get; set; }
    public TerrainLayer TerrainLayer { get; set; }
    public void Init(ForestLayer layer)
    {
        ForestLayer = layer;
        Alive = true;
    }

    public void Tick()
    {
        if(!Alive) return;
        Move();
        if(Energy < 50) Consume();
        Poop();
        Age++;
        if (Energy < 1) LifePoints--;
        Energy -= 1 * ConsumptionRate;
        if (Age > MaxAge || LifePoints < 1) Die();
        /*
        if (!Alive) return;

        // Die when too Old or out of Lifepoint
        if (LifePoints < 1 || Age > (365 * 20))
        {
            Alive = false;
            Die();
            return;
        }
        
        // Find Tree in The Radios
        _adultTree = AnimalLayer.ForestLayer.ExploreTrees(Position, 10)
            .FindAll(t => AnimalLayer.ForestLayer.GetState(t) == State.Adult);
        
        // Spawning Child when enough Energy and Old enough
        
        if (Energy > 140 && Age > 15 * 365)
        {
            var child = AnimalLayer.CreateAnimal(2, Position);
            Energy -= 20;
            LifePoints -= 20;
        }
        
        
        Move();
        
        // Eat if Energy too low
        if (Energy < 80)
        {
            Consume();
        }

        // Energy too low affect life point
        if (Energy <= 0)
        {
            Energy = 0;
            LifePoints -= 5;
        }

        Age++;
        */
    }
/*
    public void Move()
    {
        //if (_adultTree.Count > 0 && Energy < 30)
        if (_adultTree.Count > 0)    
        {
            AnimalLayer.Environment.MoveTo(this, _adultTree.First(), Movement);
        }
        else
        {
            var rnd = new Random();
            var x = AnimalLayer.Environment.DimensionX;
            x = rnd.Next(x);
            var y = AnimalLayer.Environment.DimensionY;
            y = rnd.Next(y);
            AnimalLayer.Environment.MoveTo(this, new Position(x, y), Movement);
        }

        // Lower the Energy when Moving
        Energy--;
    }

    public void Consume()
    {
        // Ask if the tree enough Fruits
        var fruitLeft = AnimalLayer.ForestLayer.FruitLeft(Position);

        if (fruitLeft > 0)
        {
            // Needed Fruit for full health
            var fruitNeed = (100 - Energy) / 20;
        
            // Gather Fruit from a tree, lower the Fruits count
            Energy += (AnimalLayer.ForestLayer.GatherFruit(Position, fruitNeed)) * 20 + 10;
            LifePoints += 10;
                
            cnt ++;
            // POop Spread Tree
            _seed = AnimalLayer.ForestLayer.GetSpecie(Position);
            Poop(_seed);
            
        }
        
    }
    private Specie _seed;
    private int cnt=0;
    
    public void Poop(Specie seed)
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 10);
        var rnd = new Random();
        if (rnd.Next(100*PoopRate) % 69 == 0)
        {
            var x = RandomHelper.Random.Next(AnimalLayer.ForestLayer.Width);
            var y = RandomHelper.Random.Next(AnimalLayer.ForestLayer.Height);
            
            switch (seed)
            { 
                case Specie.NutmegTree:
                    //Console.Write("**Seed are 1**");
                    AnimalLayer.ForestLayer.CreatSeeding(1, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;
                case Specie.PalmTree:
                    //Console.Write("**Seed are 2**");
                    AnimalLayer.ForestLayer.CreatSeeding(2, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;
                case Specie.BrazilNutTree:
                    //Console.Write("**Seed are 3**");
                    AnimalLayer.ForestLayer.CreatSeeding(3, Position.CreatePosition(Position.X + x, Position.Y + y));
                    break;

            }
            
        }

        _seed = Specie.NotATree;


    }

    public void Die()
    {
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position, 500);
        AnimalLayer.RemoveAnimal(this);
        AnimalLayer.TerrainLayer.AddSoilNutrients(Position,100);

    }

    public AnimalLayer AnimalLayer { get; set; }
*/
    public bool Alive { get; set; }

    public Position Position { get; set; }
    
    //100% LifePoints & Energy 
    public int LifePoints = 100;
    public double Energy = 100;

    // name = type, example: name = Monkey
    public string Name { get; set; }
    public int Age { get; set; }
    public int MaxAge { get; set; }
    public int MatureAge { get; set; }
    
    // rates
    public double MovementSpeed { get; set; }
    public double ConsumptionRate { get; set;}
    
    // herbivore = eats plants/fruits
    public bool Herbivore { get; set; }
    
    //carnivore = eats meat/other animals
    public bool Carnivore { get; set; }
    

    // identifies the agent
    public Guid ID { get; set; }
    public void Move()
    {
    }

    public void Consume()
    {
    }

    public void Poop()
    {
    }

    public void Die()
    {
    }
}