using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Hook : MonoBehaviour
{
    [SerializeField] private DistanceJoint2D _joint2D;
    [SerializeField] private float rotoffset;
    private Rigidbody2D _rb;
    private Transform target;

    [Space] [Header("Particles")] [SerializeField]
    private ParticleSystem groundparticles;

    [SerializeField] private ParticleSystem rocksParticles;
    [SerializeField] private ParticleSystem personparticles;
    [SerializeField] private ParticleSystem defaultParticle;

    public Rigidbody2D Rb
    {
        get { return _rb; }

        set { _rb = value; }
    }

    public DistanceJoint2D DistanceJoint
    {
        get { return _joint2D; }

        set { _joint2D = value; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _joint2D.enabled = false;
    }

    private void Start()
    {
        //     RotationChangeWhileFlying();
    }

    private void FixedUpdate()
    {
    }

    /*private void RotationChangeWhileFlying()
    {
        if (_rb.bodyType == RigidbodyType2D.Static) return;
        if (Camera.main != null)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var currentDir = mousePos - transform.position;
            currentDir.Normalize();
            float rotZ = Mathf.Atan2(currentDir.y, currentDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ + rotoffset);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;

        if (other.GetComponent<Rigidbody2D>() == null) return;

        if (GetComponent<Hookline>().StartPos != null)
            Invoke("ActivateJoint", 2f);
        Particles();
    }

    public void Particles(BlockType type)
    {
        switch (type)
        {
            case BlockType.Ground:
                groundparticles.Play();
                break;
            case BlockType.Person:
                personparticles.Play();
                break;
            case BlockType.Rock:
                rocksParticles.Play();
                break;
        }
    }

    public void Particles()
    {
        defaultParticle.Emit(1);
    }

    public void ActivateJoint()
    {
        var startPos = GetComponent<Hookline>().StartPos;
        _joint2D.connectedBody = startPos.GetComponent<Rigidbody2D>();
        _joint2D.distance = 0f;
        _joint2D.enabled = true;
    }
}