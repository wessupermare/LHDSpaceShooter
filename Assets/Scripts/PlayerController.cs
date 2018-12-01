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

    public float Speed = 5.0f;
    public float StrafeSpeed = 150.0f;
    public float JumpSpeed = 10.0f;
    public float FireRate = 0.5f;
    public int MuzzleVelocity = 10;

    private Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
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

        if (transform.position.y < -5f)
            GetComponent<Health>().TakeDamage(100);

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

        var hSpin = Input.GetAxisRaw("HorizontalRotate") * Time.deltaTime * StrafeSpeed;
        var vSpin = Input.GetAxisRaw("VerticalRotate") * Time.deltaTime * StrafeSpeed;
        
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + hSpin, 0);
        BulletSpawn.transform.Rotate(vSpin, 0, 0);
        transform.Translate(x, 0, z);

        if (canJump && Input.GetAxisRaw("Jump") >= 0.2f)
        {
            canJump = false;
            rb.velocity = new Vector3(rb.velocity.x, JumpSpeed, rb.velocity.z);
        }

        if (cooloff >= FireRate && (Input.GetAxis("Shoot") + Input.GetAxis("Fire1") > 0.1f))
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

    private bool canJump = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
            canJump = true;
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
