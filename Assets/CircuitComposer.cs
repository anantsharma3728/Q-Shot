using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CircuitComposer : MonoBehaviour
{
     public GameObject ComposerRoot;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {    // Lock cursor when clicking outside of menu
       
        if (!ComposerRoot.activeSelf && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape) | Input.GetKeyDown(KeyCode.RightShift))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(Input.GetKeyDown(KeyCode.RightShift))
        {
             ComposerRoot.SetActive(true);
        }

    }
}
