/*************************************************************************
 *     This file & class is part of the MIConvexHull Library Project. 
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     MIConvexHull is free software: you can redistribute it and/or modify
 *     it under the terms of the MIT License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     MIConvexHull is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     MIT License for more details.
 *  
 *     You should have received a copy of the MIT License
 *     along with MIConvexHull.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at https://designengrlab.github.io/MIConvexHull/
 *************************************************************************/

namespace magicsim
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using System.Windows.Shapes;
    using MIConvexHull;

    /// <summary>
    /// A vertex is a simple class that stores the postion of a point, node or vertex.
    /// </summary>
    class Cell : TriangulationCell<Vertex, Cell>
    {
        static Random rnd = new Random();

        public Brush Brush { get; private set; }

        public class FaceVisual
        {
            Cell f;

            public Model3D CreateModel(Point3DCollection points, Color color)
            {
                // center = Sum(p[i]) / 4
                var center = points.Aggregate(new Vector3D(), (a, c) => a + (Vector3D)c) / (double)points.Count;

                //var normals = new Vector3DCollection();
                var indices = new Int32Collection();
                MakeFace(0, 1, 2, center, indices, points);

                var geometry = new MeshGeometry3D { Positions = points, TriangleIndices = indices };
                var material = new MaterialGroup
                {
                    Children = new MaterialCollection
                {
                    new DiffuseMaterial(new SolidColorBrush(color) { Opacity = 1.00 })
                }
                };

                return new GeometryModel3D { Geometry = geometry, Material = material, BackMaterial = material };
            }

            /// <summary>
            /// Helper function to get the position of the i-th vertex.
            /// </summary>
            /// <param name="i"></param>
            /// <returns>Position of the i-th vertex</returns>
            Point GetPosition(int i)
            {
                return f.Vertices[i].ToPoint();
            }

            /// <summary>
            /// This function adds indices for a triangle representing the face.
            /// The order is in the CCW (counter clock wise) order so that the automatically calculated normals point in the right direction.
            /// </summary>
            /// <param name="i"></param>
            /// <param name="j"></param>
            /// <param name="k"></param>
            /// <param name="center"></param>
            /// <param name="indices"></param>
            void MakeFace(int i, int j, int k, Vector3D center, Int32Collection indices, Point3DCollection points)
            {
                var u = points[j] - points[i];
                var v = points[k] - points[j];

                // compute the normal and the plane corresponding to the side [i,j,k]
                var n = Vector3D.CrossProduct(u, v);
                var d = -Vector3D.DotProduct(n, center);

                // check if the normal faces towards the center
                var t = Vector3D.DotProduct(n, (Vector3D)points[i]) + d;
                if (t >= 0)
                {
                    // swapping indices j and k also changes the sign of the normal, because cross product is anti-commutative
                    indices.Add(k); indices.Add(j); indices.Add(i);
                }
                else
                {
                    // indices are in the correct order
                    indices.Add(i); indices.Add(j); indices.Add(k);
                }
            }

            public FaceVisual(Cell f)
            {
                this.f = f;

                var fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));
                f.Brush = fill;
            }
        }

        double Det(double[,] m)
        {
            return m[0, 0] * ((m[1, 1] * m[2, 2]) - (m[2, 1] * m[1, 2])) - m[0, 1] * (m[1, 0] * m[2, 2] - m[2, 0] * m[1, 2]) + m[0, 2] * (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]);
        }

        double LengthSquared(double[] v)
        {
            double norm = 0;
            for (int i = 0; i < v.Length; i++)
            {
                var t = v[i];
                norm += t * t;
            }
            return norm;
        }

        Point GetCircumcenter()
        {
            // From MathWorld: http://mathworld.wolfram.com/Circumcircle.html

            var points = Vertices;

            double[,] m = new double[3, 3];

            // x, y, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 0] = points[i].Position[0];
                m[i, 1] = points[i].Position[1];
                m[i, 2] = 1;
            }
            var a = Det(m);

            // size, y, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 0] = LengthSquared(points[i].Position);
            }
            var dx = -Det(m);

            // size, x, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 1] = points[i].Position[0];
            }
            var dy = Det(m);

            // size, x, y
            for (int i = 0; i < 3; i++)
            {
                m[i, 2] = points[i].Position[1];
            }
            var c = -Det(m);

            var s = -1.0 / (2.0 * a);
            var r = System.Math.Abs(s) * System.Math.Sqrt(dx * dx + dy * dy - 4 * a * c);
            return new Point(s * dx, s * dy);
        }

        Point GetCentroid()
        {
            return new Point(Vertices.Select(v => v.Position[0]).Average(), Vertices.Select(v => v.Position[1]).Average());
        }

        public FaceVisual Visual;
        Point? circumCenter;
        public Point Circumcenter
        {
            get
            {
                circumCenter = circumCenter ?? GetCircumcenter();
                return circumCenter.Value;
            }
        }

        Point? centroid;
        public Point Centroid
        {
            get
            {
                centroid = centroid ?? GetCentroid();
                return centroid.Value;
            }
        }

        public Cell()
        {
            Visual = new FaceVisual(this);
        }
    }
}
