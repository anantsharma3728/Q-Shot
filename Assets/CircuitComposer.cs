using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircuitComposer : MonoBehaviour
{
    public static bool isCircuitON= false;

    [SerializeField] GameObject circuit;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
              if (isCircuitON)
              {
                  CircuitOff();
              }
              else
              {
                  CircuitOn();
              }
        }
    }
}
void CircuitOn()
{

}
void CircuitOff()
{
    circuit.SetActive(true);
    
}