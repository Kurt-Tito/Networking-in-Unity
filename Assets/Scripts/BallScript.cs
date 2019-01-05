using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    public static BallScript Instance { set; get; }

    public GameObject ball;

    public Vector3 direction = new Vector3(0.0f, 5.0f, 0.0f);
    public float force = 1.0f;
    public float gravity = -9.8f;

    private Client client;

    private bool p1 = false;
    private bool p2 = false;

	// Use this for initialization
	void Start () {
        Instance = this;
        client = FindObjectOfType<Client>();

        if (client.isHost)
            p1 = true;
        else
            p2 = true;
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKey(KeyCode.Space))
        {
            jump();
        }

        if (Input.touchCount > 0)
        {
            string msg = "CJUMP";
            client.Send(msg);

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                jump();
            }
        }
	}

    public void jump()
    {
        
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.AddForce(direction * force, ForceMode.Impulse);
            ball.transform.position += direction;
        }
    }
}
