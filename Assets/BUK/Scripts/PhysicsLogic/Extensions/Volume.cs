using System;
using UnityEngine;

namespace Buk.PhysicsLogic.Extensions
{
    public static class MeshExtensions {
      private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 scale)
      {
          float v321 = p3.x * scale.x * p2.y * scale.y * p1.z * scale.z;
          float v231 = p2.x * scale.x * p3.y * scale.y * p1.z * scale.z;
          float v312 = p3.x * scale.x * p1.y * scale.y * p2.z * scale.z;
          float v132 = p1.x * scale.x * p3.y * scale.y * p2.z * scale.z;
          float v213 = p2.x * scale.x * p1.y * scale.y * p3.z * scale.z;
          float v123 = p1.x * scale.x * p2.y * scale.y * p3.z * scale.z;
          return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
      }

      public static float Volume(this Mesh mesh, Vector3 scale)
      {
          float volume = 0;
          Vector3[] vertices = mesh.vertices;
          int[] triangles = mesh.triangles;
          for (int i = 0; i < mesh.triangles.Length; i += 3)
          {
              Vector3 p1 = vertices[triangles[i + 0]];
              Vector3 p2 = vertices[triangles[i + 1]];
              Vector3 p3 = vertices[triangles[i + 2]];
              volume += SignedVolumeOfTriangle(p1, p2, p3, scale);
          }
          return Mathf.Abs(volume);
      }
    }
}
