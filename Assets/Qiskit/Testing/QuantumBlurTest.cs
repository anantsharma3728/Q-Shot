using IronPython.Hosting;
using Qiskit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using QuantumImage;
using UnityEngine.UI;
using Qiskit.Float;

public class QuantumBlurTest : MonoBehaviour {

    const string lib = @"/StreamingAssets/Lib/";
    const string pythonScripts = @"/StreamingAssets/PythonScripts/";
    const string blurHelper = @"quantumblurhelper.py";
    const string advancedQiskitTest = @"microqisktiTestAdvanced/";
    const string visualisation = "Visualisation";

    public Texture2D InputTexture;
    public Texture2D InputTexture2;

    public QuantumCircuit Circuit;
    public double[] Probabilities;
    public float[] ProbabilitiesFloat;

    public float Rotation = 0.01f;
    public bool UseLog = false;
    public bool DoFast = false;

    public bool Colored = false;

    public string FileName = "Test";

    public float Mixture = 0;

    public Texture2D OutputTexture;

    public int Length;

    public string[] Lines;

    public int[] IntLines;

    public bool Renormalize = false;

    public QuantumCircuitFloat FloatCircuit;

    QuantumImageCreator creator;

    public RawImage InputImage;
    public RawImage OutputImage;

    // Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;

        /*
        creator = new QuantumImageCreator();

        GC.Collect();
        Invoke("DoCreatorTest", 0.5f);
        
        */
        //Invoke("doFastTest", 1.25f);
        //Invoke("doSlowTest", 2.5f);

        //Invoke("DirectTest", 0.5f);

        DirectTest();


    }

    public void ApplyPartialQ(QuantumCircuit circuit, float rotation) {
        for (int i = 0; i < circuit.NumberOfQubits; i++) {
            circuit.RX(i, rotation * Mathf.PI);
        }
    }

    public void ApplyPartialQ(QuantumCircuitFloat circuit, float rotation) {
        for (int i = 0; i < circuit.NumberOfQubits; i++) {
            circuit.RX(i, rotation * Mathf.PI);
        }
    }

    public void DirectTest() {


        if (Colored) {

            //Debug.Log("Colored");

            //QuantumCircuit red, green, blue;

            QuantumCircuitFloat red, green, blue;


            QuantumImageCreator.GetCircuitDirectPerChannel(InputTexture, out red, out green, out blue);

            ApplyPartialQ(red, Rotation);
            ApplyPartialQ(green, Rotation);
            ApplyPartialQ(blue, Rotation);

            //FloatCircuit = red;

            //Circuit = red;

            /*


            QuantumCircuit red = QuantumImageCreator.GetCircuitDirect(InputTexture, ColorChannel.R);
            if (Rotation > 0)
            {
                ApplyPartialQ(red, Rotation);
            }


            QuantumCircuit green = QuantumImageCreator.GetCircuitDirect(InputTexture, ColorChannel.G);
            if (Rotation > 0)
            {
                ApplyPartialQ(green, Rotation);
            }


            QuantumCircuit blue = QuantumImageCreator.GetCircuitDirect(InputTexture, ColorChannel.B);
            if (Rotation > 0)
            {
                ApplyPartialQ(blue, Rotation);
            }
            */
            /*
            double sum = 0;
            double max = 0;

            for (int i = 0; i < Probabilities.Length; i++) {
                double tmp = Probabilities[i] - ProbabilitiesFloat[i];
                tmp = Math.Abs(tmp);
                sum += tmp;
                if (tmp > max) {
                    max = tmp;
                }
            }

            Debug.Log(sum + " " + max);
            */

            //MicroQiskitSimulator simulator = new MicroQiskitSimulator();

            //Probabilities = simulator.GetProbabilities(red);


            OutputTexture = QuantumImageCreator.GetColoreTextureDirectFast(red, green, blue, InputTexture.width, InputTexture.height, Renormalize);
            OutputTexture.filterMode = FilterMode.Bilinear;
            OutputTexture.name = "OutputTexture";


        } else {
            Circuit = QuantumImageCreator.GetCircuitDirect(InputTexture);

            ApplyPartialQ(Circuit, Rotation);

            OutputTexture = QuantumImageCreator.GetGreyTextureDirect(Circuit, InputTexture.width, InputTexture.height, Renormalize);
        }


        //if (OutputImage != null) {
        //    OutputImage.texture = OutputTexture;
        //}

        //if (InputImage != null) {
        //    InputImage.texture = InputTexture;
        //}

        /*
        
        //Init python
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of python scripts.py
        searchPaths.Add(Application.dataPath + pythonScripts + advancedQiskitTest);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + lib);
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.dataPath + pythonScripts + advancedQiskitTest + blurHelper);


        dynamic circuit = py.CircuitFromHeight(QuantumImageHelper.GetGreyHeighArray(InputTexture), InputTexture.width, InputTexture.height);

        int numberofQubits = circuit.num_qubits;

        string heightDimensions = circuit.name;

        Debug.Log("The name is:" + circuit.name + " the number of qubits are: " + numberofQubits);



        QuantumCircuit = QuantumImageHelper.ParseCircuit(circuit.data, numberofQubits);

        MicroQiskitSimulator simulator = new MicroQiskitSimulator();

        Probabilities = simulator.GetProbabilities(QuantumCircuit);

        double[] doubleArray = new double[0];
        string[] stringArray = new string[0];


        QuantumImageHelper.GetProbabilityArrays(Probabilities, numberofQubits, ref doubleArray, ref stringArray);

        IronPython.Runtime.PythonDictionary dictionary = py.HeightFromProbabilities(stringArray, doubleArray, doubleArray.Length, heightDimensions);

        OutputTexture = QuantumImageHelper.CalculateGreyTexture(dictionary, heightDimensions);
        */

    }

    public void DoIndirectTest() {
        //Init python
        var engine = Python.CreateEngine();

        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of python scripts.py
        searchPaths.Add(Application.dataPath + pythonScripts + advancedQiskitTest);
        //Path to the Python standard library
        searchPaths.Add(Application.dataPath + lib);
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.dataPath + pythonScripts + advancedQiskitTest + blurHelper);
        dynamic quantumHelper = py.QuantumBlurHelper("Helper");

        double[,] imageData = QuantumImageHelper.GetGreyHeighArray(InputTexture);

        quantumHelper.SetHeights(imageData, InputTexture.width, InputTexture.height, UseLog);

        quantumHelper.ApplyPartialX(Rotation);

        dynamic circuit = quantumHelper.GetCircuit();


        int numberofQubits = circuit.num_qubits;

        string heightDimensions = circuit.name;

        Debug.Log("The name is:" + circuit.name + " the number of qubits are: " + numberofQubits);



        Circuit = QuantumImageHelper.ParseCircuit(circuit.data, numberofQubits);

        MicroQiskitSimulator simulator = new MicroQiskitSimulator();

        Probabilities = simulator.GetProbabilities(Circuit);

        double[] doubleArray = new double[0];
        string[] stringArray = new string[0];


        QuantumImageHelper.GetProbabilityArrays(Probabilities, numberofQubits, ref doubleArray, ref stringArray);

        IronPython.Runtime.PythonDictionary dictionary = py.HeightFromProbabilities(stringArray, doubleArray, doubleArray.Length, heightDimensions, UseLog);

        OutputTexture = QuantumImageHelper.CalculateGreyTexture(dictionary, heightDimensions);


    }

    public void DoCreatorTest() {
        if (creator == null) {
            creator = new QuantumImageCreator();

        }
        if (Colored) {

            GC.Collect();
            /*
            long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            OutputTexture = creator.CreateBlurTextureColor(InputTexture, Rotation, UseLog, false);
            long finish = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            Debug.Log("It took " +  (finish - start) + " Miliseconds for slow");

            start = finish;

            OutputTexture = creator.CreateBlurTextureColor(InputTexture, Rotation, UseLog, true);

            finish = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            Debug.Log("It took " + (finish - start) + " Miliseconds for fast");

            doSlowTest();

            doFastTest();

            doSlowTest();

            doFastTest();

            doSlowTest();

            doFastTest();

            */

            doFastTest();


        } else {
            OutputTexture = creator.CreateBlurTextureGrey(InputTexture, Rotation, UseLog);
        }
    }


    void doSlowTest() {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        OutputTexture = creator.CreateBlurTextureColor(InputTexture, Rotation, UseLog, false);
        long finish = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        Debug.Log("It took " + (finish - start) + " Miliseconds for slow");

    }

    void doFastTest() {
        long start = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        OutputTexture = creator.CreateBlurTextureColor(InputTexture, Rotation, UseLog, true);

        long finish = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        Debug.Log("It took " + (finish - start) + " Miliseconds for fast");

    }


    public void SwapTest() {

        Debug.Log("Starting test");
        //if (creator == null)
        //{
        creator = new QuantumImageCreator();
        //}

        OutputTexture = creator.TeleportTexturesGreyPartByPart(InputTexture, InputTexture2, Mixture);


    }


    public void SafeFile() {
        string path = Path.Combine(Application.dataPath, visualisation, FileName + ".png");
        File.WriteAllBytes(path, OutputTexture.EncodeToPNG());
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }


    public void LineTest() {
        IntLines = QuantumImageHelper.MakeLinesInt(Length);
        Lines = QuantumImageHelper.MakeLines(Length);
    }
}
