using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject[] SpawnPoints;
    public GameObject Bullet;
    public Transform BulletSpawn;
    public Vector3 CameraOffset;

    public float Speed = 3.0f;
    public float StrafeSpeed = 150.0f;
    public float FireRate = 0.5f;
    public int MuzzleVelocity = 10;

	void Start ()
    {
		
	}

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    float cooloff = 0f;
    // Update is called once per frame
    void Update ()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * StrafeSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (cooloff >= FireRate && Input.GetAxis("Fire1") > 0.1f)
            CmdFire();
        else
            cooloff += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;

        Camera.main.transform.SetPositionAndRotation(transform.position + CameraOffset, transform.rotation);
    }

    [Command]
    void CmdFire()
    {
        cooloff = 0f;
        GameObject b = Instantiate(Bullet, BulletSpawn.position, BulletSpawn.rotation);
        b.GetComponent<Rigidbody>().velocity = b.transform.forward * MuzzleVelocity;
        NetworkServer.Spawn(b);

        Destroy(b, 2f);
    }
}
