using System.Collections.Generic;
using Paramdigma.Core.HalfEdgeMesh;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Grasshopper
{
    public static class RhinoConvert
    {
        public static RhinoMeshResult ToRhinoMesh(Mesh mesh, out Rhino.Geometry.Mesh rhinoMesh)
        {
            //CHECKS FOR NGON MESHES!
            if (mesh.IsNgonMesh())
            {
                rhinoMesh = null;
                return RhinoMeshResult.Invalid;
            }

            rhinoMesh = new Rhino.Geometry.Mesh();

            foreach (MeshVertex vertex in mesh.Vertices)
            {
                rhinoMesh.Vertices.Add(vertex.X, vertex.Y, vertex.Z);
            }

            foreach (MeshFace face in mesh.Faces)
            {
                List<MeshVertex> faceVertices = face.AdjacentVertices();
                if (faceVertices.Count == 3) rhinoMesh.Faces.AddFace(new Rhino.Geometry.MeshFace(faceVertices[0].Index, faceVertices[1].Index, faceVertices[2].Index));
                if (faceVertices.Count == 4) rhinoMesh.Faces.AddFace(new Rhino.Geometry.MeshFace(faceVertices[0].Index, faceVertices[1].Index, faceVertices[2].Index, faceVertices[3].Index));
            }

            return RhinoMeshResult.OK;

        }

        public static RhinoMeshResult FromRhinoMesh(Rhino.Geometry.Mesh rhinoMesh, out Mesh mesh)
        {
            List<Point3d> vertices = new List<Point3d>();
            List<List<int>> faceVertexIndexes = new List<List<int>>();

            foreach (Rhino.Geometry.Point3d vertex in rhinoMesh.Vertices)
            {
                vertices.Add(new Point3d(vertex.X, vertex.Y, vertex.Z));
            }
            foreach (Rhino.Geometry.MeshNgon face in rhinoMesh.GetNgonAndFacesEnumerable())
            {

                List<int> list = new List<int>();

                foreach (int i in face.BoundaryVertexIndexList()) list.Add(i);

                faceVertexIndexes.Add(list);

            }
            mesh = new Mesh(vertices, faceVertexIndexes);
            return RhinoMeshResult.OK;
        }

        public enum RhinoMeshResult
        {
            OK,
            Empty,
            Invalid
        }
    }

}
