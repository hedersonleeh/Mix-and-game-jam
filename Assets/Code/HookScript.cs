using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HookScript : MonoBehaviour
{
    [SerializeField] private Hook hookPrefab;
    [Space] [SerializeField] private PlayerMovement controller;
    [Space] [SerializeField] private float launchForce;
    [SerializeField] private Transform initPos;
    [SerializeField] private LayerMask whatIsHookable;
    [SerializeField] private Vector2 target;
    [SerializeField] private Vector2 offSet;
    private Hookline hookLine;
    private bool grappled;
    private Hook hookRef;
    [SerializeField] private float grappleRefreshTime;

    void Start()
    {
    }

    private void Update()
    {
        GetLaunchDir();
    }

    private void DisconnectCable()
    {
        if (hookRef == null) return;
        Destroy(hookRef.gameObject);
        StartCoroutine((GrappleCooldown(grappleRefreshTime)));
    }

    IEnumerator GrappleCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        grappled = false;
    }
    private void FixedUpdate()
    {
        if (!controller.Grap) return;
        if (grappled) return;
         ShootHook();
    }

    private void LateUpdate()
    {
        if (controller.Grap && grappled)  
            DisconnectCable();
    }

    private void ShootHook()
    {
        if (GetTarget() == null) return;
        var hook = Instantiate(hookPrefab, initPos.position, Quaternion.identity);
        hook.GetComponent<Hookline>().StartPos = transform;
        Grapple(hook);
        grappled = true;
        hookRef = hook;

    }
    private Rigidbody2D GetTarget()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        var direction = mousePos - transform.position;
        var hit = Physics2D.Raycast(transform.position, direction, 10f, whatIsHookable);

        if (hit.rigidbody != null)
            return hit.rigidbody;
        return null;
    }
    private Vector2 GetLaunchDir()
    {
        if (Camera.main == null) return default;
        
        var direction = MousePos() - transform.position;
        return direction;
    }

    private static Vector3 MousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }

    private void Grapple(Hook hook)
    {
        var rotZ = Mathf.Atan2(-GetLaunchDir().y, GetLaunchDir().x) * Mathf.Rad2Deg;
        hook.transform.rotation = Quaternion.Euler(0, 0, -(rotZ + 90f));
        var bounds = GetTarget().GetComponent<Collider2D>().bounds;
        var vector = new Vector2(bounds.min.x, bounds.center.y);
        hook.Rb.MovePosition(MousePos());
    }

    private void ConnectCable(Rigidbody2D target)
    {
    }
}