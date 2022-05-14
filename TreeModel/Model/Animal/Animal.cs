using System;
using Mars.Interfaces.Environments;

namespace TreeModel.Model.Animal;

public class Animal : IAnimal<AnimalLayer>
{
    public void Init(AnimalLayer layer)
    {
        AnimalLayer = layer;
    }

    public void Tick()
    {
        //TODO
    }

    public Guid ID { get; set; }
    public void Move()
    {
        //TODO
    }

    public void Consume()
    {
        //TODO
    }

    public void Poop()
    {
        //TODO
    }

    public void Die()
    {
        //TODO
    }

    public Position Position { get; set; }
    public AnimalLayer AnimalLayer { get; set; }
}