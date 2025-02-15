using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] GameObject mask;

    void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Player"){
            mask.SetActive(true);
            this.gameObject.SetActive(false);
        //}
    }
}
