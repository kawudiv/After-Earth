using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI.Components
{
    public class EnemySensor : MonoBehaviour
    {
        [Header("Senser Componenets")]
        [SerializeField, Range(1, 50)]
        private float distance = 10;

        [SerializeField, Range(1, 180)]
        private float angle = 30;

        [SerializeField, Range(1, 5)]
        private float height = 1.0f;

        [SerializeField, Range(10, 100)]
        private int segments = 10;

        [SerializeField, ColorUsage(true, true)]
        private Color meshColor = new Color(0f / 255f, 27f / 255f, 255f / 255f, 100f / 255f); // RGBA (0, 27, 255, 53)

        [Header("Scan Settings")]
        [SerializeField, Range(1, 60)]
        private int scanFrequency = 30;

        [SerializeField, Range(0.1f, 5.0f)]
        private float scanInterval = 1.0f;

        [SerializeField]
        public LayerMask layers = ~0; // Default to "Everything"

        [SerializeField]
        public LayerMask occlusionLayers = 1; // Default to "Default"

        [SerializeField]
        public List<GameObject> Objects = new List<GameObject>();

        [HideInInspector]
        private float scanTimer;

        [HideInInspector]
        private int count;

        [HideInInspector]
        private Collider[] colliders = new Collider[50];

        [HideInInspector]
        private Mesh mesh;

        void Start()
        {
            scanInterval = 1.0f / scanFrequency;
        }

        void Update()
        {
            scanTimer -= Time.deltaTime;
            if (scanTimer < 0)
            {
                scanTimer += scanInterval;
                Scan();
            }
        }

        private void Scan()
        {
            count = Physics.OverlapSphereNonAlloc(
                transform.position,
                distance,
                colliders,
                layers,
                QueryTriggerInteraction.Collide
            );
            Objects.Clear();
            for (int i = 0; i < count; ++i)
            {
                GameObject obj = colliders[i].gameObject;
                if (IsInSight(obj))
                {
                    Objects.Add(obj);
                }
            }
            if (count > 0)
            {
                Debug.Log($"[EnemySensor] Detected {count} objects within range.", this);
            }
        }

        public bool IsInSight(GameObject obj)
        {
            Vector3 origin = transform.position;
            Vector3 dest = obj.transform.position;
            Vector3 direction = dest - origin;
            if (direction.y < 0 || direction.y > height)
            {
                return false;
            }

            direction.y = 0;
            float deltaAngle = Vector3.Angle(direction, transform.forward);
            if (deltaAngle > angle)
            {
                return false;
            }

            origin.y += height / 2;
            dest.y = origin.y;
            if (Physics.Linecast(origin, dest, occlusionLayers))
            {
                return false;
            }

            return true;
        }

        Mesh CreateWedgeMesh()
        {
            Mesh mesh = new Mesh();

            int numTriangles = (segments * 4) + 2 + 2;
            int numVertices = numTriangles * 3;

            Vector3[] vertices = new Vector3[numVertices];
            int[] triangles = new int[numVertices];

            Vector3 bottomCenter = Vector3.zero;
            Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
            Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

            Vector3 topCenter = bottomCenter + Vector3.up * height;
            Vector3 topRight = bottomRight + Vector3.up * height;
            Vector3 topLeft = bottomLeft + Vector3.up * height;

            int vert = 0;

            // Left Side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            // Right Side
            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            float currentAngle = -angle;
            float deltaAngle = (angle * 2) / segments;

            for (int i = 0; i < segments; ++i)
            {
                bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
                bottomRight =
                    Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

                topRight = bottomRight + Vector3.up * height;
                topLeft = bottomLeft + Vector3.up * height;

                //Far Side
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;
                vertices[vert++] = topRight;

                vertices[vert++] = topRight;
                vertices[vert++] = topLeft;
                vertices[vert++] = bottomLeft;

                // Top
                vertices[vert++] = topCenter;
                vertices[vert++] = topLeft;
                vertices[vert++] = topRight;

                // Bottom
                vertices[vert++] = bottomCenter;
                vertices[vert++] = bottomLeft;
                vertices[vert++] = bottomRight;

                currentAngle += deltaAngle;
            }

            for (int i = 0; i < numVertices; ++i)
            {
                triangles[i] = i;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        private void OnValidate()
        {
            mesh = CreateWedgeMesh();
        }

        private void OnDrawGizmos()
        {
            if (mesh)
            {
                Gizmos.color = meshColor;
                Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
            }

            // Change wire sphere color based on detection
            //Gizmos.color = (count > 0) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, distance);

            // // Draw spheres for detected colliders
            // Gizmos.color = Color.green;
            for (int i = 0; i < count; ++i)
            {
                Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
            }

            // Draw spheres for objects in 'Objects' list
            Gizmos.color = Color.red;
            foreach (var obj in Objects)
            {
                Gizmos.DrawSphere(obj.transform.position, 0.2f);
            }
        }
    }
}
