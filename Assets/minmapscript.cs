using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minmapscript : MonoBehaviour
{      
    public Transform Player;

    void LateUpdate()
    {
        Vector3 newposition = Player.position;
        newposition.y = transform.position.y;
        transform.position = newposition;


        transform.rotation = Quaternion.Euler(90f , Player.eulerAngles.y , 0f);


    }
}
