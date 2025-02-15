using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOFView_2 : MonoBehaviour
{
    Mesh mesh;
    [SerializeField] Transform player;
    Vector3 origin;
    [SerializeField] float fov = 90f, startingAngle = 0f, viewDistance = 5f;
    [SerializeField] int rayCount = 50;
    [SerializeField] LayerMask obstacleLayer; // Detect walls

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; // Ensure MeshFilter is attached

        if (player != null) origin = player.position;
        else origin = Vector3.zero;
    }

    private void LateUpdate() {
        if (player != null) origin = player.position; // Update position every frame

        Vector3[] vertices = new Vector3[rayCount + 2]; // Center + ray points
        int[] triangles = new int[rayCount * 3];

        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        vertices[0] = Vector3.zero; // Center of FOV in local space

        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 direction = player.TransformDirection(DirFromAngle(angle)); // Rotate based on player

            Vector3 vertex;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, viewDistance, obstacleLayer))
            {
                vertex = transform.InverseTransformPoint(hit.point); // Convert to local space
            }
            else
            {
                vertex = transform.InverseTransformPoint(origin + direction * viewDistance);
            }

            vertices[i + 1] = vertex;

            if (i > 0) {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = i;
                triangles[triangleIndex + 2] = i + 1;
                triangleIndex += 3;
            }

            angle -= angleIncrease;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Ensure the FOV object follows the player's rotation
        transform.position = origin;
        transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0); // Match player Y rotation
    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        float angleRad = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)); // 3D correction in XZ plane
    }
}
