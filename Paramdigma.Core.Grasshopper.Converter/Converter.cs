using System;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Collections;
using Paramdigma.Core.HalfEdgeMesh;
using Paramdigma.Core.Geometry;
using Rhino.Geometry.Collections;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Grasshopper.Converter
{
    public static class Converter
    {
        public static RG.Interval ToRhino(this Collections.Interval interval) =>
            new RG.Interval(interval.Start, interval.End);

        public static Collections.Interval ToRhino(this RG.Interval interval) =>
            new Collections.Interval(interval.Min, interval.Max);

        public static RG.Point3d ToRhino(this Point3d pt) => new RG.Point3d(pt.X, pt.Y, pt.Z);

        public static Point3d ToCore(this RG.Point3d pt) => new Point3d(pt.X, pt.Y, pt.Z);

        public static RG.Point4d ToRhino(this Point4d pt)
        {
            return new RG.Point4d(pt.X, pt.Y, pt.Z, pt.Weight);
        }

        public static Point4d ToCore(this RG.Point4d pt)
        {
            return new Point4d(pt.X, pt.Y, pt.Z, pt.W);
        }

        public static RG.Vector3d ToRhino(this Vector3d pt) => new RG.Vector3d(pt.X, pt.Y, pt.Z);

        public static Vector3d ToCore(this RG.Vector3d pt) => new Vector3d(pt.X, pt.Y, pt.Z);

        public static RG.Mesh ToRhino(this Mesh mesh)
        {
            var result = new RG.Mesh();

            foreach (var vertex in mesh.Vertices)
                result.Vertices.Add(vertex.X, vertex.Y, vertex.Z);

            foreach (var faceVertices in mesh.Faces.Select(face => face.AdjacentVertices()))
                switch (faceVertices.Count)
                {
                    case 3:
                        result.Faces.AddFace(new RG.MeshFace(
                            faceVertices[0].Index,
                            faceVertices[1].Index,
                            faceVertices[2].Index));
                        break;
                    case 4:
                        result.Faces.AddFace(new RG.MeshFace(
                            faceVertices[0].Index,
                            faceVertices[1].Index,
                            faceVertices[2].Index,
                            faceVertices[3].Index));
                        break;
                }

            return result;
        }

        public static Mesh ToCore(this RG.Mesh mesh)
        {
            var vertices = new List<Point3d>();
            var faceVertexIndexes = new List<List<int>>();

            foreach (Rhino.Geometry.Point3d vertex in mesh.Vertices)
            {
                vertices.Add(new Point3d(vertex.X, vertex.Y, vertex.Z));
            }

            foreach (var face in mesh.GetNgonAndFacesEnumerable())
            {
                var list = new List<int>();

                foreach (int i in face.BoundaryVertexIndexList()) list.Add(i);

                faceVertexIndexes.Add(list);
            }

            return new Mesh(vertices, faceVertexIndexes);
        }

        public static RG.NurbsSurface ToRhino(this NurbsSurface surface)
        {
            var result = RG.NurbsSurface.Create(3, false, surface.DegreeU + 1, surface.DegreeV + 1,
                surface.ControlPoints.M, surface.ControlPoints.N);

            for (var i = 0; i < surface.ControlPoints.M; i++)
            for (var j = 0; j < surface.ControlPoints.N; j++)
                result.Points.SetPoint(i, j, surface.ControlPoints[i, j].ToRhino());

            var knotsU = surface.KnotsU.GetRange(1, surface.KnotsU.Count - 2);
            for (var i = 0; i < knotsU.Count; i++)
                result.KnotsU[i] = knotsU[i];

            var knotsV = surface.KnotsV.GetRange(1, surface.KnotsV.Count - 2);
            for (var i = 0; i < knotsU.Count; i++)
                result.KnotsV[i] = knotsV[i];

            return result;
        }

        public static NurbsSurface ToCore(this RG.NurbsSurface surface)
        {
            var controlPoints = new Matrix<Point4d>(surface.Points.CountU, surface.Points.CountV);
            for (int i = 0; i < surface.Points.CountU; i++)
            {
                for (int j = 0; j < surface.Points.CountV; j++)
                {
                    surface.Points.GetPoint(i, j, out RG.Point4d pt);
                    controlPoints[i, j] = pt.ToCore();
                }
            }

            return new NurbsSurface(controlPoints, surface.OrderU - 1, surface.OrderV - 1);
        }
    }
}