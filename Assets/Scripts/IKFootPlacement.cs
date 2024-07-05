using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{
    public Transform leftFrontFootTarget;
    public Transform leftFrontFootHint;
    public Transform rightFrontFootTarget;
    public Transform rightFrontFootHint;
    public Transform leftBackFootTarget;
    public Transform leftBackFootHint;
    public Transform rightBackFootTarget;
    public Transform rightBackFootHint;
    public LayerMask terrainLayer;
    private float raycastDistance = 1.0f;

    void Update()
    {
        AdjustFootTarget(leftFrontFootTarget, leftFrontFootHint);
        AdjustFootTarget(rightFrontFootTarget, rightFrontFootHint);
        AdjustFootTarget(leftBackFootTarget, leftBackFootHint);
        AdjustFootTarget(rightBackFootTarget, rightBackFootHint);
    }

    void AdjustFootTarget(Transform footTarget, Transform footHint)
    {
        RaycastHit hit;
        Vector3 footPosition = footTarget.position;

        if (Physics.Raycast(footPosition + Vector3.up, Vector3.down, out hit, raycastDistance, terrainLayer))
        {
            footTarget.position = hit.point;
            footHint.position = hit.point + Vector3.up * 0.1f; // Ajusta la posición del Hint
        }
    }
}
