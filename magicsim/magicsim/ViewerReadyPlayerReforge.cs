using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace magicsim
{
    public class ViewerReadyPlayerReforge
    {
        public string PlayerName { get; set; }
        public GearResults PlayerGear { get; set; }
        public Point3D[,] PlayerMesh { get; set; }
        public Brush SurfaceBrush { get; set; }
        public double[,] MeshColors { get; set; }
        public Model3DGroup Lights { get; set; }

        public ViewerReadyPlayerReforge(string name, GearResults gear, Point3D[,] mesh, double[,] meshTex, Brush brush, Model3DGroup lights)
        {
            this.PlayerName = name;
            this.PlayerGear = gear;
            this.PlayerMesh = mesh;
            this.SurfaceBrush = brush;
            this.MeshColors = meshTex;
            this.Lights = lights;
        }
    }
}