using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {    //a
    [Header("Set in Inspector")]

    public GameObject applePrefab;

    //Speed at which the AppleTree moves
    public float speed = 1f;

    //Distance where AppleTree turns around
    public float leftAndRightEdge = 10f;

    //Chance that the AppleTree will change directions
    public float chanceToChangeDirections = 0.02f;

    //Rate at which Apples will be instantiated
    public float secondsBetweenAppleDrops = 1f;

	// Use this for initialization
	void Start () {
        //Dropping apples every second
        Invoke("DropApple", 2f);
	}
	
    void DropApple()
    {
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;
        Invoke("DropApple", secondsBetweenAppleDrops);
    }


	// Update is called once per frame
	void Update () {
        //Basic movement
        Vector3 pos = transform.position;   //b
        pos.x += speed * Time.deltaTime;    //c
        transform.position = pos;           //d


        //Changing direction		
        if (pos.x < -leftAndRightEdge) {
            speed = Mathf.Abs(speed);
        } else if (pos.x > leftAndRightEdge) {
            speed = -Mathf.Abs(speed);
        } 
	}

    void FixedUpdate()
    {
        //Changing Direction Randomly is now time-based because of FiedUpdate()
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1;    //Change direction
        }
    }
}
