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

        if (Mathf.Abs(transform.rotation.x) > 0.1f || Mathf.Abs(transform.rotation.z) > 0.1f)
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, transform.rotation.y, 0));

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        var hSpin = Input.GetAxis("HorizontalRotate") * Time.deltaTime * StrafeSpeed;
        var vSpin = Input.GetAxis("VerticalRotate") * Time.deltaTime * StrafeSpeed;

        transform.Rotate(0, hSpin, 0);
        BulletSpawn.transform.Rotate(vSpin, 0, 0);
        transform.Translate(0, x, z);

        if (cooloff >= FireRate && Input.GetAxis("Fire1") > 0.1f)
            CmdFire();
        else
            cooloff += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;

        Camera.main.transform.SetPositionAndRotation(transform.position + CameraOffset, BulletSpawn.rotation);
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
