// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SurfacePlotVisual3D.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Gets or sets the points defining the surface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using MIConvexHull;
using System.Linq;
using System.Drawing.Drawing2D;

namespace magicsim
{
    /// <summary>
    /// As noted in the copyright, this file is largely copied from samples taken from the Helix Toolkit website. Adjustments were made to fit our domain but it was largely used as-was.
    /// Helix Toolkit samples were published under MIT License, as is this tool.
    /// </summary>
    public class SurfacePlotVisual3D : ModelVisual3D
    {
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(Point3D[,]), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty ColorValuesProperty =
            DependencyProperty.Register("ColorValues", typeof(double[,]), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty SurfaceBrushProperty =
            DependencyProperty.Register("SurfaceBrush", typeof(Brush), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty XAxisNameProperty =
            DependencyProperty.Register("XAxisName", typeof(string), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty YAxisNameProperty =
            DependencyProperty.Register("YAxisName", typeof(string), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));

        public static readonly DependencyProperty ZAxisNameProperty =
            DependencyProperty.Register("ZAxisName", typeof(string), typeof(SurfacePlotVisual3D),
                                        new UIPropertyMetadata(null, ModelChanged));


        public SurfacePlotVisual3D()
        {
            IntervalX = 0.1;
            IntervalY = 0.1;
            IntervalZ = 1000;
            FontSize = 0.06;
            LineThickness = 0.01;
        }

        public string XAxisName
        {
            get { return (string)GetValue(XAxisNameProperty); }
            set
            {
                if ((string)GetValue(XAxisNameProperty) != value)
                {
                    SetValue(XAxisNameProperty, value);
                }
            }
        }
        public string YAxisName
        {
            get { return (string)GetValue(YAxisNameProperty); }
            set
            {
                if ((string)GetValue(YAxisNameProperty) != value)
                {
                    SetValue(YAxisNameProperty, value);
                }
            }
        }
        public string ZAxisName
        {
            get { return (string)GetValue(ZAxisNameProperty); }
            set
            {
                if ((string)GetValue(ZAxisNameProperty) != value)
                {
                    SetValue(ZAxisNameProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the points defining the surface.
        /// </summary>
        public Point3D[,] Points
        {
            get { return (Point3D[,])GetValue(PointsProperty); }
            set
            {
                if (GetValue(PointsProperty) != value)
                {
                    SetValue(PointsProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color values corresponding to the Points array.
        /// The color values are used as Texture coordinates for the surface.
        /// Remember to set the SurfaceBrush, e.g. by using the BrushHelper.CreateGradientBrush method.
        /// If this property is not set, the z-value of the Points will be used as color value.
        /// </summary>
        public double[,] ColorValues
        {
            get { return (double[,])GetValue(ColorValuesProperty); }
            set {
                if (GetValue(ColorValuesProperty) != value)
                {
                    SetValue(ColorValuesProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the brush used for the surface.
        /// </summary>
        public Brush SurfaceBrush
        {
            get { return (Brush)GetValue(SurfaceBrushProperty); }
            set {
                if (GetValue(SurfaceBrushProperty) != value)
                {
                    SetValue(SurfaceBrushProperty, value);
                }
            }
        }


        // todo: make Dependency properties
        public double IntervalX { get; set; }
        public double IntervalY { get; set; }
        public double IntervalZ { get; set; }
        public double FontSize { get; set; }
        public double LineThickness { get; set; }

        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((double[,])d.GetValue(ColorValuesProperty) != null && (Brush)d.GetValue(SurfaceBrushProperty) != null
                && (Point3D[,])d.GetValue(PointsProperty) != null && (string)d.GetValue(XAxisNameProperty) != null
                 && (string)d.GetValue(YAxisNameProperty) != null && (string)d.GetValue(ZAxisNameProperty) != null)
            {
                ((SurfacePlotVisual3D)d).UpdateModel();
            }
        }

        private void UpdateModel()
        {
            this.Content = CreateModel();
        }

        private Model3D CreateModel()
        {

            var viewport = this.GetViewport3D();
            var plotModel = new Model3DGroup();
            var Children = plotModel.Children;
            plotModel.Children = null;
            int rows = Points.GetUpperBound(0) + 1;
            int columns = Points.GetUpperBound(1) + 1;
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            double minColorValue = double.MaxValue;
            double maxColorValue = double.MinValue;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    double x = Points[i, j].X;
                    double y = Points[i, j].Y;
                    double z = Points[i, j].Z;
                    if(x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }
                    maxX = Math.Max(maxX, x);
                    maxY = Math.Max(maxY, y);
                    maxZ = Math.Max(maxZ, z);
                    minX = Math.Min(minX, x);
                    minY = Math.Min(minY, y);
                    minZ = Math.Min(minZ, z);
                    if (ColorValues != null)
                    {
                        maxColorValue = Math.Max(maxColorValue, ColorValues[i, j]);
                        minColorValue = Math.Min(minColorValue, ColorValues[i, j]);
                    }
                }
            }

            IntervalX = (maxX - minX) / 6.0;
            IntervalY = (maxY - minY) / 6.0;
            IntervalZ = (maxZ - minZ) / 6.0;
            FontSize = 0.01;
            LineThickness = 0.005;

            // make color value 0 at texture coordinate 0.5
            if (Math.Abs(minColorValue) < Math.Abs(maxColorValue))
            {
                minColorValue = -maxColorValue;
            }
            else
            {
                maxColorValue = -minColorValue;
            }

            // set the texture coordinates by z-value or ColorValue
            var texcoords = new Point[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    double u = (Points[i, j].Z - minZ) / (maxZ - minZ);
                    if (ColorValues != null)
                        u = (ColorValues[i, j] - minColorValue) / (maxColorValue - minColorValue);
                    texcoords[i, j] = new Point(u, u);
                }
            }

            if (rows == 1 || columns == 1)
            {
                var surfaceMeshBuilder = new MeshBuilder();
                var pointList = new List<Point3D>();
                var texturePoints = new List<double>();
                var diameters = new List<double>();
                for(int i = 0; i < rows; i++)
                {
                    for(int j = 0; j < columns; j++)
                    {
                        pointList.Add(Points[i, j]);
                        texturePoints.Add(texcoords[i, j].X);
                        diameters.Add(LineThickness*2.0);
                    }
                }
                surfaceMeshBuilder.AddTube(pointList, texturePoints.ToArray(), diameters.ToArray(), 9, false, true, true);
                var mesh = surfaceMeshBuilder.ToMesh();

                var surfaceModel = new GeometryModel3D(mesh,
                                                       MaterialHelper.CreateMaterial(SurfaceBrush, null, null, 1, 0));
                surfaceModel.BackMaterial = surfaceModel.Material;
                Children.Add(surfaceModel);
            }
            else
            {
                var vertexZMapping = new Dictionary<Vertex, double>();
                var vertices = new List<Vertex>();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (Points[i, j].X == 0.0 && Points[i, j].Y == 0.0 && Points[i, j].Z == 0.0)
                        {
                            continue;
                        }
                        var vertex = new Vertex(Points[i, j].X, Points[i, j].Y);
                        vertices.Add(vertex);
                        vertexZMapping[vertex] = Points[i, j].Z;
                    }
                }

                var mesh = DelaunayTriangulation<Vertex, Cell>.Create(vertices, 1e-10);

                foreach (var cell in mesh.Cells)
                {
                    // 3D-ify it.
                    var min = cell.Vertices.Min(vertex => vertexZMapping[vertex]);
                    var max = cell.Vertices.Max(vertex => vertexZMapping[vertex]);
                    var minPercentile = (min - minZ) / (maxZ - minZ);
                    var maxPercentile = (max - minZ) / (maxZ - minZ);

                    // R = Distance from end.
                    // G = Distance from start.
                    // B = Distance from middle
                    var minR = Math.Round(255.0 * (1 - minPercentile));
                    var minG = Math.Round(255.0 * (minPercentile));
                    var minB = Math.Round(255.0 * (Math.Abs(0.5 - minPercentile) / 0.5));

                    var maxR = Math.Round(255.0 * (1 - maxPercentile));
                    var maxG = Math.Round(255.0 * (maxPercentile));
                    var maxB = Math.Round(255.0 * (Math.Abs(0.5 - maxPercentile) / 0.5));


                    var minColor = Color.FromArgb((byte)255, (byte)minR, (byte)minG, (byte)minB);
                    var maxColor = Color.FromArgb((byte)255, (byte)maxR, (byte)maxG, (byte)maxB);
                    Children.Add(cell.Visual.CreateModel(new Point3DCollection(cell.Vertices.Select(vertex =>
                    {
                        var vertexPoint = vertex.ToPoint();
                        return new Point3D(vertexPoint.X, vertexPoint.Y, vertexZMapping[vertex]);
                    })), minColor, maxColor));
                }
            }


            var axesMeshBuilder = new MeshBuilder();
            for (double x = minX; x <= maxX; x += IntervalX)
            {
                double j = (x - minX) / (maxX - minX) * (columns - 1);
                var path = new List<Point3D> { new Point3D(x, minY, minZ) };
                path.Add(new Point3D(x, maxY, minZ));

                axesMeshBuilder.AddTube(path, LineThickness, 9, false, true, true);
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(x.ToString("F4"), Brushes.Black, true, FontSize,
                                                                           new Point3D(x, minY - FontSize * 6, minZ),
                                                                           new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
                label.Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90.0), new Point3D(label.Bounds.SizeX/2 + label.Bounds.Location.X, label.Bounds.SizeY / 2 + label.Bounds.Location.Y, 0.0));
                Children.Add(label);
            }

            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(XAxisName, Brushes.Black, true, FontSize,
                                                                           new Point3D((minX + maxX) * 0.5,
                                                                                       minY - FontSize * 9, minZ),
                                                                           new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
                Children.Add(label);
            }

            for (double y = minY; y <= maxY; y += IntervalY)
            {
                double i = (y - minY) / (maxY - minY) * (rows - 1);
                var path = new List<Point3D> { new Point3D(minX, y, minZ) };
                path.Add(new Point3D(maxX, y, minZ));

                axesMeshBuilder.AddTube(path, LineThickness, 9, false, true, true);
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(y.ToString("F4"), Brushes.Black, true, FontSize,
                                                                           new Point3D(minX - FontSize * 6, y, minZ),
                                                                           new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
                Children.Add(label);
            }
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(YAxisName, Brushes.Black, true, FontSize,
                                                                           new Point3D(minX - FontSize * 10,
                                                                                       (minY + maxY) * 0.5, minZ),
                                                                           new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0));
                Children.Add(label);
            }
            //double z0 = (int)(minZ / IntervalZ) * IntervalZ;
            for (double z = minZ; z <= maxZ + double.Epsilon; z += IntervalZ)
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(z.ToString("F4"), Brushes.Black, true, FontSize,
                                                                           new Point3D(minX - FontSize * 3, maxY, z),
                                                                           new Vector3D(1, 0, 0), new Vector3D(0, 0, 1));
                Children.Add(label);
            }
            {
                GeometryModel3D label = TextCreator.CreateTextLabelModel3D(ZAxisName, Brushes.Black, true, FontSize,
                                                                           new Point3D(minX - FontSize * 10, maxY,
                                                                                       (minZ + maxZ) * 0.5),
                                                                           new Vector3D(0, 0, 1), new Vector3D(1, 0, 0));
                Children.Add(label);
            }


            var bb = new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, 0 * (maxZ - minZ));
            axesMeshBuilder.AddBoundingBox(bb, LineThickness);

            var axesModel = new GeometryModel3D(axesMeshBuilder.ToMesh(), Materials.Black);

            Children.Add(axesModel);

            plotModel.Children = Children;
            return plotModel;
        }
    }
}