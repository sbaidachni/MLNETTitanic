using Microsoft.ML.Models;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML;
using System;

namespace MLNetTitanic
{
    class Program
    {
        static void Main(string[] args)
        {
            string trainSetPath = "train_data.csv";
            string testSetPath = "test_data.csv";

            var pipeline = new LearningPipeline();

            pipeline.Add(new TextLoader<Passenger>(trainSetPath, useHeader: true, separator: ","));

            pipeline.Add(new ColumnDropper() {Column=new string[] {"Cabin","Ticket"} });

            pipeline.Add(new MissingValueSubstitutor(new string[] { "Age" })
                { ReplacementKind=NAReplaceTransformReplacementKind.Mean });

            pipeline.Add(new CategoricalOneHotVectorizer("Sex", "Embarked"));

            pipeline.Add(new ColumnConcatenator(
                "Features", "Age","Pclass", "SibSp","Parch","Sex","Embarked"));

            pipeline.Add(new FastTreeBinaryClassifier());

            var model=pipeline.Train<Passenger, PredictedData>();

            var testLoader = new TextLoader<Passenger>(testSetPath, useHeader: true, separator: ",");

            var evaluator = new BinaryClassificationEvaluator();

            var metrics=evaluator.Evaluate(model, testLoader);

            Console.WriteLine($"Accuracy: {metrics.Accuracy} F1 Score: {metrics.F1Score}");

            Console.WriteLine($"True Positive: {metrics.ConfusionMatrix[0, 0]} False Positive: {metrics.ConfusionMatrix[0, 1]}");
            Console.WriteLine($"False Negative: {metrics.ConfusionMatrix[1, 0]} True Negative: {metrics.ConfusionMatrix[1, 1]}");
        }
    }
}
