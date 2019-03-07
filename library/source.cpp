//
// Created by Thomas Ecalle on 04/03/2019.
//

#include <cstdlib>
#include <cstring>
#include <cstdio>
#include <iostream>

void printArray(double *array, int size)
{
    for (int i = 0; i < size; ++i)
    {
        std::cout << "index : " << i << " -> " << array[i] << std::endl;
    }
}

//  Activation function
int sign(double number)
{
    return number < 0 ? -1 : 1;
}

// Generate a random double number
double randomDouble(float min, float max)
{
    double scale = rand() / (double) RAND_MAX; /* [0, 1.0] */
    return min + scale * (max - min);      /* [min, max] */
}

extern "C"
{

// Fake function to test the pipeline
int foo()
{
    return 5;
}

//  création du modele = besoin de créer un tableau de double représentant les poids
//  function createModel(nombre d'inputs pris par le model) : tableau représentant les poids
//// Attention, création d'un tableau du nombre d'entrée +1
//// Il est préférable d'initialiser le tableau avec des valeurs aléatoires
//// On ne stock pas le tableau en C, on revoi juste le pointeur
double *createModel(int dimensions)
{
    auto *weights = new double[dimensions + 1];

    // Weights initialization
    for (int i = 0; i <= dimensions; i++)
    {
        weights[i] = randomDouble(-1, 1);
    }


    return weights;
}

// Free model space
void linear_remove_model(double *model)
{
    free(model);
}


// function inferenceRegressionClassif(leMOdel: TableauDeDoubleRepresentantLesPoids, double *tableauInputs, )
//// total = model[0];
//// pour x de 1 <= model.taille
////// total += model[i] * inputs[i-1];
//// return total;
double inferenceRegressionClassif(const double *model, int nbInputs, const double *inputs)
{
    double total = model[0];
    for (auto i = 1; i <= nbInputs; i++)
    {
        total += inputs[i - 1] * model[i];
    }

    return total;
}

//// On considère que le biais est toujours la case 0

// Ici, différent selon regression/linéaire/...
// function inferenceLinearClassif(leMOdel: TableauDeDoubleRepresentantLesPoids, double *tableauInputs, )
//// total = model[0];
//// pour x de 1 <= model.taille
////// table += model[i] * inputs[i-1];
//// return Sign(total);
double inferenceLinearClassif(const double *model, int nbInputs, const double *inputs)
{
    return sign(inferenceRegressionClassif(model, nbInputs, inputs));
}

// Les inputs sont des tableaux de tableau car un tzableau de toutes les lignes de la BDD
// int nbIterations = nbEpochs
// lezrning rate = 0.01 par exemple
// nbIterations = 10.0000 par exemple
// function trainLinearClassif(double* model,
//                              double ** inputs,
//                              double * expectedOutputs,
//                              double learningRate,
//                              int nbIterations)
//// Pour i de 0 à nbEpochs
////// Pour chaque index, input dans inputs
/////// gxk = inferenceLinearClassif(model, input);
/////// yK = expectedOutputs[input];
///////// Pour w = 0; w <= nbINputs; w++
/////////// model[w] += learningRate * (yK - gxK) * (si w == 0 ? 1 : input[w -1])
void trainLinearClassif(double *model,
                        double *inputs,
                        const int inputsSize,
                        const int inputElementSize,
                        double *expectedOutputs,
                        double learningRate,
                        int nbEpochs)
{
    for (int i = 0; i < nbEpochs; ++i)
    {
        int expectedOutputsIndex = 0;
        for (int inputsIndex = 0; inputsIndex < inputsSize; inputsIndex += inputElementSize)
        {
            auto *input = new double[inputElementSize];
            for (int temp = 0; temp < inputElementSize; temp++)
            {
                input[temp] = inputs[inputsIndex + temp];
            }
            std::cout << "The " << inputElementSize << " inputs are" << std::endl;
            printArray(input, inputElementSize);

            auto gxk = inferenceLinearClassif(model, inputElementSize, input);
            auto yK = expectedOutputs[expectedOutputsIndex];

            std::cout << "Expected output is  " << yK << std::endl;

            expectedOutputsIndex++;

            for (int w = 0; w <= inputElementSize; w++)
            {
                model[w] += learningRate * (yK - gxk) * (w == 0 ? 1 : inputs[w - 1]);
            }

        }
    }
}

}

int main()
{
    auto *model = createModel(2);

    // Data to trained
    int inputSize = 6;
    auto *dataToTrain = new double[inputSize]{3, 2, 2, 3, 8, 8};

    // Expected results
    int expectedResultsSize = inputSize / 2;
    auto *expectedResults = new double[expectedResultsSize]{1, 1, -1};

    trainLinearClassif(model, dataToTrain, inputSize, 2, expectedResults, 0.01, 10000);


    // New Inputs to test
    int nbPositive = 0;
    int nbNegative = 0;
    for (int i = 0; i < 100; ++i)
    {
        double fakeX = randomDouble(1, 10);
        double fakeY = randomDouble(1, 10);
        double result = inferenceLinearClassif(model, 2, new double[2]{fakeX, fakeY});
        result == -1 ? nbNegative++ : nbPositive++;
    }

    std::cout << "Nb negative = " << nbNegative << std::endl;
    std::cout << "Nb positive = " << nbPositive << std::endl;



    //printArray(model, 3);
    //printArray(dataToTrain, 6);
}