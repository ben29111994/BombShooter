using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBombSite : MonoBehaviour
{
    public List<GameObject> bombList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MovingBomb")
        {
            bombList.Add(other.gameObject);
            if (bombList.Count >= 2)
            {
                other.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MovingBomb")
        {
            bombList.Remove(other.gameObject);
        }
    }
}
