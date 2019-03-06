//
// Created by Thomas Ecalle on 04/03/2019.
//

#include <cstdlib>
#include <cstring>
#include <cstdio>

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
    return 42;
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
    for (int i = 0; i < dimensions; i++)
    {
        double random = randomDouble(-1, 1);
        weights[i] = random;
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
double inferenceRegressionClassif(const double *model, int modelSize, const double *inputs)
{
    double total = model[0];
    for (auto i = 1; i <= modelSize; i++)
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
double inferenceLinearClassif(const double *model, int modelSize, const double *inputs)
{
    return sign(inferenceRegressionClassif(model, modelSize, inputs));
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
void trainLinearClassif(double *model, const double modelSize, double *inputs, const int inputsSize,
                        const int inputElementSIze,
                        double *expectedOutputs,
                        double learningRate,
                        int nbEpochs)
{
    for (int i = 0; i < nbEpochs; ++i)
    {
        for (int inputsIndex = 0; inputsIndex < inputsSize; inputsIndex += inputElementSIze)
        {
            double* input = inputs + inputElementSIze * sizeof(double);

            auto gxk = inferenceLinearClassif(model, modelSize, input);
            auto yK = expectedOutputs[inputsIndex / inputElementSIze];

            for (int w = 0; w <= inputsSize; w++)
            {
                model[w] += learningRate * (yK - gxk) * (w == 0 ? 1 : inputs[w - 1]);
            }

        }
    }
}
//int linear_firt_regression(double *model, double, double *inputs, int inputsSize, int inputSize, double *outputs, int outputsSize);
//int linear_fit_classification_rosenblatt(double *modem, double *inputs, int inputsSize, double *outputs, int outputsSize);
//double linear_classify(double *model, double *input, int inputSize);
//double linear_regression(double *model, double *input, int inputSize);
}