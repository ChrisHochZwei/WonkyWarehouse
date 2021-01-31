using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfBehaviorNew : MonoBehaviour
{
    public Transform[] boxTransforms;
    public GameObject crateProtoDefault;
    public GameObject crateProtoSpecial;

    public void PopulateBoxes(bool includesSpecial = false)
    {
        Transform boxPositions = transform.Find("Positions");
        int specialIdx = 0;
        if (includesSpecial)
        {
            specialIdx = Random.Range(0, boxPositions.childCount);
        }
        
        for (int i = 0; i < boxPositions.childCount; i++)
        {
            GameObject boxProto;
            if (includesSpecial && i == specialIdx)
            {
                boxProto = crateProtoSpecial;
            }
            else
            {
                boxProto = crateProtoDefault;
            }
            Transform boxEmptyTransform = boxPositions.GetChild(i).transform;
            GameObject boxInstance = Instantiate(boxProto, boxEmptyTransform.position, boxEmptyTransform.rotation);
            boxInstance.transform.SetParent(this.transform);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
