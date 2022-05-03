using System;
using System.IO;
using Mars.Components.Starter;
using Mars.Interfaces.Model;
using RunnerModel.Model;

namespace RunnerModel
{
    internal static class Program
    {
        private static void Main()
        {
            // the scenario consist of the model (represented by the model description)
            // an the simulation configuration (see config.json)
            
            // Create a new model description that holds all parts of the model
            var description = new ModelDescription();
            description.AddLayer<TreeLayer>();
            description.AddAgent<Tree, TreeLayer>();

            // scenario definition
            // use config.json that provides the specification of the scenario
            var file = File.ReadAllText("config.json");
            var config = SimulationConfig.Deserialize(file);

            // Create simulation task accordingly
            var starter = SimulationStarter.Start(description, config);
            // Run simulation
            var handle = starter.Run();
            // Feedback to user that simulation run was successful
            Console.WriteLine("Successfully executed iterations: " + handle.Iterations);
            starter.Dispose();
        }
    }
}
