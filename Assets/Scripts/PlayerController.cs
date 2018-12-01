﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject[] SpawnPoints;
    public float Speed = 3.0f;
    public float StrafeSpeed = 150.0f;

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isLocalPlayer)
            return;
        
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * StrafeSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}
