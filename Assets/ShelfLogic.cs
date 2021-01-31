using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfLogic : MonoBehaviour
{
    private Transform shelvesContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        shelvesContainer = transform.Find("Shelves");
        int specialIdx = Random.Range(0, shelvesContainer.childCount);
        for (int i = 0; i < shelvesContainer.childCount; i++)
        {
            Debug.Log($"{i}, {shelvesContainer.childCount}");
            Transform shelf = shelvesContainer.GetChild(i);
            ShelfBehaviorNew sb = shelf.GetComponent<ShelfBehaviorNew>();
            sb.PopulateBoxes(i == specialIdx);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
