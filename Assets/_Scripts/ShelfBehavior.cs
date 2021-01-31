using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class ShelfBehavior : MonoBehaviour
{
    public GameObject[] boxPositions;
    public GameObject boxInstancer;
    public Material boxBaseMaterial;
    public GameObject cameraEmpty;
    public GameObject selectorObject;
    
    private Camera mainCamera;
    private GameObject player;

    private List<List<GameObject>> boxMap = new List<List<GameObject>>();
    private int cursorX = 1;
    private int cursorY = 1;
    private GameObject selectedBox;
    private float selectorSlerpCount = 0.0f;
    private GameObject selectorInstance;
    private Renderer selectorInstanceRenderer;

    private Color[] _colors = {
        Color.blue,
        Color.red,
        Color.green,
        Color.magenta,
        Color.yellow,
        Color.gray,
        Color.cyan,
    };
    private Random _r = new Random();

    private bool isCameraOnShelf = false;
    private bool colliding = false;

    private Vector3 prevPos;
    private Vector3 prevRot;

    private Vector3 desiredPos;
    private Vector3 desiredRot;

    private float cameraSlerpCount = 0.0f;

    private PlayerController pc;
    private Pickup pickupScript;
    
    // Start is called before the first frame update
    void Start()
    {
        // Find object instances in scene
        mainCamera = Camera.main;
        player = GameObject.Find("Player");

        Transform mainCameraTransform = mainCamera.transform;
        desiredPos = mainCameraTransform.position;
        desiredRot = mainCameraTransform.rotation.eulerAngles;

        //pc = player.GetComponent<PlayerController>();
//        pickupScript = player.GetComponentInChildren<Pickup>();
        
        // Set boxes into nested list so we can access them in the controls later
        var boxList = new List<GameObject>();
        for (int i = 0; i < boxPositions.Length; i++)
        {
            var boxPos = boxPositions[i];

            // Instantiate boxes in shelf positions
            GameObject boxInstance;
            boxInstance = Instantiate(boxInstancer, boxPos.transform.position, boxPos.transform.rotation);
            boxInstance.transform.SetParent(this.transform);
            Renderer boxRenderer;
            boxRenderer = boxInstance.GetComponent<Renderer>();
            Material boxMaterial = Instantiate(boxBaseMaterial);
            boxMaterial.SetColor("_Color", _colors[Random.Range(0, _colors.Length - 1)]);
            //boxRenderer.material = boxMaterial;
            
            // Add box instances to nested array for cursor
            boxList.Add(boxInstance);
            if ((i + 1) % 3 == 0)
            {
                boxMap.Add(boxList);
                boxList = new List<GameObject>();
            }
        }

        selectedBox = boxMap[cursorY][cursorX];
        // Instantiate selector
        selectorInstance = Instantiate(selectorObject, selectedBox.transform.position, Quaternion.identity);
        selectorInstance.transform.SetParent(this.transform);
        selectorInstanceRenderer = selectorInstance.GetComponent<Renderer>();
        selectorInstanceRenderer.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Transform mainCameraTransform = mainCamera.transform
        
        // Save camera position before movement
        //prevPos = mainCameraTransform.position;
        //prevRot = mainCameraTransform.rotation.eulerAngles;
        
        colliding = true;

    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Collider");
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                Transform cameraEmptyTransform = cameraEmpty.transform;
            
                if (!isCameraOnShelf)
                {
                    // Set desired camera position to camera empty
                    desiredPos = cameraEmptyTransform.position;
                    desiredRot = cameraEmptyTransform.rotation.eulerAngles;
                
                    pc.enabled = false;
                    isCameraOnShelf = true;
                } else
                {
                    // Set desired camera position to previous camera position
                    desiredPos = prevPos;
                    desiredRot = prevRot;
                
                    pc.enabled = true;
                    isCameraOnShelf = false;
                }

                cameraSlerpCount = 0.0f;
            }

            if (colliding && isCameraOnShelf)
            {
                selectorInstanceRenderer.enabled = true;
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (cursorY > 0)
                    {
                        cursorY -= 1;
                        selectorSlerpCount = 0.0f;
                    }
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (cursorY < boxMap.Count-1)
                    {
                        cursorY += 1;
                        selectorSlerpCount = 0.0f;
                    }
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (cursorX > 0)
                    {
                        cursorX -= 1;
                        selectorSlerpCount = 0.0f;
                    }
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (cursorX < boxMap.Count-1)
                    {
                        cursorX += 1;
                        selectorSlerpCount = 0.0f;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (boxMap[cursorY][cursorX])
                    {
                        if (pickupScript.pickedUpItem)
                        {
                            pickupScript.pickedUpItem.transform.position = pickupScript.prevItemPos;
                            pickupScript.pickedUpItem = null;
                        }
                        pickupScript.prevItemPos = boxMap[cursorY][cursorX].transform.position;
                        pickupScript.pickedUpItem = boxMap[cursorY][cursorX];
                    }
                }
            }
            else
            {
//                selectorInstanceRenderer.enabled = false;
            }
            
            selectedBox = boxMap[cursorY][cursorX];
        }
    }
    
    void LateUpdate()
    {           
        //Transform mainCameraTransform = mainCamera.transform;

        if (colliding)
        {
            if (isCameraOnShelf)
            {
               // mainCameraTransform.position = Vector3.Lerp(mainCameraTransform.position, desiredPos, cameraSlerpCount);
            }
            
            selectorInstance.transform.position = Vector3.Lerp(selectorInstance.transform.position, selectedBox.transform.position,
                selectorSlerpCount);
            //mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, Quaternion.Euler(desiredRot), cameraSlerpCount);
        }

        selectorSlerpCount = selectorSlerpCount + Time.deltaTime;
        cameraSlerpCount = cameraSlerpCount + Time.deltaTime;
    }
}
