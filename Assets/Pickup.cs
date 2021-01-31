using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject pickedUpItem;
    public Vector3 prevItemPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUpItem)
        {
            pickedUpItem.transform.position = this.transform.position;
            pickedUpItem.transform.rotation = this.transform.rotation;
        }
    }
}
