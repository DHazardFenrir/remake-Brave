using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private Transform body = default;

    [SerializeField] private IKFootSolver otherFoot = default;

    [SerializeField] private float speed = 1;

    [SerializeField] private float stepDistance = 4;

    [SerializeField] private float stepLength = 4;

    [SerializeField] private float stepHight = 1;

    [SerializeField] private Vector3 offset = default;

    private float footSpacing;

    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;

    private float lerp;

    // Start is called before the first frame update
    void Start()
    {
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z
                    ? 1
                    : -1;
                newPosition = info.point + (body.forward * stepLength * direction) + offset;
                newNormal = info.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(newPosition, 00.05f);
    }

    bool IsMoving()
    {
        return lerp < 1;
    }
}
