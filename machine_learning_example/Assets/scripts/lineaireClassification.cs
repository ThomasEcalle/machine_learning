using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class lineaireClassification : MonoBehaviour
{

    [DllImport("lib")]
    public extern static int foo();

    [DllImport("lib")]
    public static extern IntPtr createModel(int dimensions);

    [DllImport("lib")]
    public static extern double regressionLineaire(IntPtr model, int modelSize, double[] inputs);

    [DllImport("lib")]
    public static extern double inferenceLinearClassif(IntPtr model, int modelSize, double[] inputs);

    [DllImport("lib")]
    public static extern void trainLinearClassif(IntPtr model,
         double[] inputs, int inputsSize, int inputElementSIze, double[] expectedOutputs, double learningRate, int nbEpochs);

    [DllImport("lib")]
    public static extern void trainRegression(IntPtr model, double[] inputs, int inputsSize, int inputElementSize, double[] expectedOutputs,
                     int expectedOutputsSize,
                     int nbEpochs);


    private IntPtr rawModel;
    public Transform[] surface;
    public Transform[] exemples;

    private IntPtr formArrayToIntPtr(double[] array)
    {
        IntPtr newintPtr = Marshal.AllocHGlobal(array.Length);

        Marshal.Copy(newintPtr, array,  0, array.Length);

        return newintPtr;

    }

    private double[] fromPointerToArray(IntPtr pointer, int size)
    {
        var managedArray = new double[size];

        Marshal.Copy(pointer, managedArray, 0, size);

        return managedArray;

    }


    // Start is called before the first frame update
    void Start()
    {
        // Pipeline Test
        Debug.Log("Version foo : " + foo());

        int nb_of_sphere_of_surface = surface.Length;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Create_Model
        Debug.Log("CreateModel");rawModel = createModel(2);

        //Debug.Log("Model (1) = " + String.Join(",", fromPointerToArray(rawModel, 2)));

        // Train_linear_classif
        double[] postionElementTrain = new double[2 * exemples.Length];
        double[] expectedOutputTrain = new double[exemples.Length];

        int index = 0;
        int indexExpec = 0;

        foreach (var sphere in exemples)
        {
            Debug.Log("[" + sphere.position.x + " , " + sphere.position.z + "] -> " + sphere.position.y);

            postionElementTrain[index] = sphere.position.x;
            index += 1;
            postionElementTrain[index] = sphere.position.z;
            index += 1;
            expectedOutputTrain[indexExpec] = sphere.position.y;
            indexExpec += 1;

            //Debug.Log(" x : " + postionElementTrain[index - 2] + ", z : " + postionElementTrain[index - 1]);

            //float newPose = sphere.position.y * 3;
            //sphere.position = new Vector3(sphere.position.x, newPose, sphere.position.z);

        }


        Debug.Log("Elements to train = " + String.Join(",", postionElementTrain));
        Debug.Log("postionElementTrain.Length = " + postionElementTrain.Length);
        Debug.Log("expectedOutputTrain = " + String.Join(",", expectedOutputTrain));

        Debug.Log("Model (2) = " + String.Join(",", fromPointerToArray(rawModel, 2)));

        Debug.Log("Train");
        trainLinearClassif(rawModel, postionElementTrain, postionElementTrain.Length, 2, expectedOutputTrain, 0.01, 10000);

        Debug.Log("Model (3) = " + String.Join(",", fromPointerToArray(rawModel, 2)));
        //Inference
        Debug.Log("Inférence");
        foreach (var sphere in surface)
        {
            Debug.Log("[" + sphere.position.x + " , " + sphere.position.z + "]");
            double[] inputs = new double[2];
            inputs[0] = sphere.position.x;
            inputs[1] = sphere.position.z;

            double res = inferenceLinearClassif(rawModel, inputs.Length, inputs);
            Debug.Log("res -> " + res);
            sphere.position = new Vector3(sphere.position.x, (float)res, sphere.position.z);
        }

    }

    // Update is called once per frame
    void Update()
    {
    }
}
