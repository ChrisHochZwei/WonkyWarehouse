using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject player;
    private Rigidbody rb;
    private Vector3 _startPosition;


    // Start is called before the first frame update
    void Start()
    {
        _startPosition = new Vector3(0, 0, 0);
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = _startPosition;
            rb.velocity = new Vector3(0,0,0);
        }
    }
}
