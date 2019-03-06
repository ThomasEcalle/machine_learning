using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class dll_wrapper : MonoBehaviour
{

    [DllImport("lib")]
    public extern static int foo();

    [DllImport("lib")]
    public static extern IntPtr createModel(int dimensions);

    [DllImport("lib")]
    public static extern double inferenceRegressionClassif(IntPtr model, int modelSize, IntPtr inputs);

    [DllImport("lib")]
    public static extern double inferenceLinearClassif(IntPtr model, int modelSize, IntPtr inputs);

    [DllImport("lib")]
    public static extern void trainLinearClassif(IntPtr model,
      double modelSize,
         IntPtr inputs, int inputsSize, int inputElementSIze, IntPtr expectedOutputs, double learningRate, int nbEpochs);



    public Transform[] surface;
    public Transform[] exemples;

    private double[] formIntPtrToArray(int size, IntPtr pointer)
    {
        var managedArray = new double[size];

        Marshal.Copy(pointer, managedArray, 0, size);

        return managedArray;

    }

    private IntPtr formArrayToIntPtr(double[] array)
    {
        IntPtr newintPtr = Marshal.AllocHGlobal(array.Length);

        Marshal.Copy(newintPtr, array,  0, array.Length);

        return newintPtr;

    }




    // Start is called before the first frame update
    void Start()
    {
        // Pipeline Test
        Debug.Log(foo());

        int nb_of_sphere_of_surface = surface.Length;
        int nb_of_know_element = exemples.Length;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // Create_Model
        Debug.Log("CreateModel");
        //double[] model = formIntPtrToArray(3, createModel(2));
        var rawmodel = createModel(2);

        // Train_linear_classif
        double[] postionElementTrain = new double[2*nb_of_know_element];
        double[] expectedOutputTrain = new double[nb_of_know_element];

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

            Debug.Log(" x : " + postionElementTrain[index - 2] + ", z : " + postionElementTrain[index - 1]);

            float newPose = sphere.position.y * 3;
            sphere.position = new Vector3(sphere.position.x, newPose, sphere.position.z);

        }


        Debug.Log("Train");
        trainLinearClassif(rawmodel, 3, formArrayToIntPtr(postionElementTrain), postionElementTrain.Length, 2, formArrayToIntPtr(expectedOutputTrain), 0.01, 10000);


        //Inference
        Debug.Log("Inférence");
        foreach (var sphere in surface)
        {
            Debug.Log("[" + sphere.position.x + " , " + sphere.position.z + "]");
            double[] inputs = new double[2];
            inputs[0] = sphere.position.x;
            inputs[1] = sphere.position.z;

            double res = inferenceLinearClassif(rawmodel, inputs.Length, formArrayToIntPtr(inputs));
            Debug.Log("res -> " + res);
            sphere.position = new Vector3(sphere.position.x, (float)res, sphere.position.z);
        }










    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
