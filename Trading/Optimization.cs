using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Infrastructure.Framework.Threading;

namespace Trading
{
    public class Optimization
    {
        Simulation Sim;
        List<Company> List;
        string Dbfile;
        public Optimization(string dbFile)
        {
            Simulation sim = new Simulation();
            Console.WriteLine("Initializing...");
            sim.Dbfile = dbFile;
            sim.SetDefault();
            Dbfile = dbFile;
            List = sim.GetTradeList();
        }
        public void Optimize(bool parallel = false)
        {
            Console.WriteLine("Running optimization");
            // Rank StopGain StopLoss MinEpsGrowth

            var chromosome = new SimChromosome();
            var population = new Population(5, 15, chromosome);

            var fitness = new SimFitness(List, Dbfile);

            var selection = new EliteSelection();
            var crossover = new UniformCrossover(0.5f);
            var mutation = new UniformMutation();
            var termination = new FitnessStagnationTermination(100);

            var ga = new GeneticAlgorithm(
            population,
            fitness,
            selection,
            crossover,
            mutation);

            if (parallel)
            {
                var taskExecutor = new ParallelTaskExecutor();
                taskExecutor.MinThreads = 2;
                taskExecutor.MaxThreads = 4;
            }

            ga.Termination = termination;

            Console.WriteLine("Generation: (minRank, stopGain, stopLoss) = value");

            var latestFitness = 0.0;

            ga.GenerationRan += (sender, e) =>
            {
                var bestChromosome = ga.BestChromosome as SimChromosome;
                var bestFitness = bestChromosome.Fitness.Value;

                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness;
                    var Rank = (double)bestChromosome.GetGene(0).Value;
                    var StopGain = (double)bestChromosome.GetGene(1).Value;
                    var StopLoss = (double)bestChromosome.GetGene(2).Value;
                    var MinGrowth = (double)bestChromosome.GetGene(3).Value;
                    Console.WriteLine(
                        "Generation {0,2}: ({1},{2},{3}) = {4}",
                        ga.GenerationsNumber,
                        Rank,
                        StopGain,
                        StopLoss,
                        bestFitness
                    );
                }
            };

            ga.Start();

        }
    }

    public class SimFitness : IFitness
    {
        public List<Company> List;
        public string DbFile;
        public SimFitness(List<Company> list, string dbFile)
        {
            List = list;
            DbFile = dbFile;
        }
        public double Evaluate(IChromosome chromosome)
        {
            //var fc = c as FloatingPointChromosome;

            //var values = fc.ToFloatingPoints();
            var Rank = (double)chromosome.GetGene(0).Value;
            var StopGain = (double)chromosome.GetGene(1).Value;
            var StopLoss = (double)chromosome.GetGene(2).Value;
            var MinGrowth = (double)chromosome.GetGene(3).Value;
            Simulation sim = new Simulation();
            sim.SetDefault();
            sim.Dbfile = DbFile;
            sim.Chp.MinRank = (float)Rank;
            sim.Portfolio.StopGain = (float)StopGain;
            sim.Portfolio.StopLoss = (float)StopLoss;
            sim.MinEpsGrowth = (float)MinGrowth;

            sim.Run(List);
            float totValue = sim.Portfolio.GetTotalValue();
            Console.WriteLine("Starting simulation,   MinRank={0}   StopGain={1}   StopLoss={2}   MinEpsGrowth={3}   FinalValue={4}", Rank, StopGain, StopLoss, MinGrowth, totValue);
           
            return totValue;
        }
    }

    class SimChromosome : ChromosomeBase
    {
        public SimChromosome()
            : base(4)
        {
            ReplaceGene(0, GenerateGene(0));
            ReplaceGene(1, GenerateGene(1));
            ReplaceGene(2, GenerateGene(2));
            ReplaceGene(3, GenerateGene(3));
        }

        // These properties represents your phenotype.
        public double MinRank
        {
            get
            {
                return (double)GetGene(0).Value;
            }
        }

        public double StopGain
        {
            get
            {
                return (double)GetGene(1).Value;
            }
        }
        public double StopLoss
        {
            get
            {
                return (double)GetGene(2).Value;
            }
        }
        public double MinGrowth
        {
            get
            {
                return (double)GetGene(3).Value;
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            double value;


            if (geneIndex == 0)
            {
                value = RandomizationProvider.Current.GetDouble(2, 12);
            }
            else if (geneIndex == 1)
            {
                value = RandomizationProvider.Current.GetDouble(1.05, 1.5);
            }
            else if (geneIndex == 2)
            {
                value = RandomizationProvider.Current.GetDouble(0.5, 0.95);
            }
            else
            {
                value = RandomizationProvider.Current.GetDouble(0.05, 0.5);
            }

            return new Gene(value);
        }

        public override IChromosome CreateNew()
        {
            return new SimChromosome();
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as SimChromosome;

            return clone;
        }
    }
}
