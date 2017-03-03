using System;
using System.Windows;
using Nevron.Chart;
using Nevron.Chart.WinForm;
using Nevron.GraphicsCore;
using Nevron.Chart.Windows;
using System.Reflection;
using static System.Math;
using NCalc;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        static string func = "y=2*x+Sin(z)";
        static float width = 60;
        static float depth = 60;
        static float height = 30;
        static float startx = 0;
        static float startz = 0;

        public static NChartControl GenerateChart(string func)
        {
            NZoomTool zt = new NZoomTool();
                zt.BeginDragMouseCommand = new NMouseCommand(MouseAction.Wheel, (MouseButton)System.Windows.Forms.MouseButtons.Middle, 0);
                zt.ZoomStep = 16;

            NChartControl nChartControl1 = new NChartControl();
                nChartControl1.Controller.Tools.Add(new NSelectorTool());
                nChartControl1.Controller.Tools.Add(new NTrackballTool());
                nChartControl1.Controller.Tools.Add(zt);
                nChartControl1.Legends[0].Mode = LegendMode.Disabled;

            NTriangulatedSurfaceSeries triangulatedSurface = new NTriangulatedSurfaceSeries();
                triangulatedSurface.UsePreciseGeometry = true;
                triangulatedSurface.AutomaticPalette = true;
                triangulatedSurface.SyncPaletteWithAxisScale = false;
                triangulatedSurface.PaletteSteps = 6;
                triangulatedSurface.FrameMode = SurfaceFrameMode.Mesh;

            NCartesianChart chart = (NCartesianChart)nChartControl1.Charts[0];
                chart.Enable3D = true;
                chart.Width = width;
                chart.Depth = depth;
                chart.Height = height;
                chart.BoundsMode = BoundsMode.Fit;
                chart.Projection.Type = ProjectionType.Perspective;
                chart.Projection.Elevation = 30;
                chart.Projection.Rotation = -60;
                chart.LightModel.SetPredefinedLightModel(PredefinedLightModel.NorthernLights);
                chart.Series.Add(triangulatedSurface);

            string[] f2 = func.Split(Char.Parse("="));
            if(f2.Length == 2)
            {
                if (f2[0] == "y" || f2[0] == "f(x)") // f-cja w postaci y=costam
                {
                    float x, z;
                    try
                    {
                        for (x = startx; x < width; x++) //x
                        {
                            for (z = startz; z < depth; z++) //z
                            {
                                NCalc.Expression ex = new NCalc.Expression(f2[1]);
                                ex.Parameters["x"] = x;
                                ex.Parameters["z"] = z;

                                ex.EvaluateParameter += delegate (string name, ParameterArgs args)
                                {
                                    if (name == "x")
                                        args.Result = x;
                                    else if (name == "z")
                                        args.Result = z;
                                };
                                var y = ex.Evaluate();

                                triangulatedSurface.XValues.Add(x);
                                triangulatedSurface.ZValues.Add(z);
                                triangulatedSurface.Values.Add(y);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Sprawdź, czy poprawnie wpisałeś równanie!", "Nie pykło");
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("Niepoprawny zapis funkcji!", "Nie pykło");
                }
            }
            else
            {
                MessageBox.Show("Niepoprawny zapis funkcji!", "Nie pykło");
            }

            return nChartControl1;
        }

        public MainWindow()
        {
            InitializeComponent();

            titleLabel.Content = "Wykres funkcji y=" + func;

            winHost.Child = GenerateChart(func);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            func = textBox.Text.ToString();
            titleLabel.Content = "Wykres funkcji " + func;
            winHost.Child = GenerateChart(func);
        }

        private void tbWidth_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                float w = float.Parse(tbWidth.Text);
                if (w != 0)
                {
                    width = w;
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        private void tbDepth_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                float w = float.Parse(tbDepth.Text);
                if (w != 0)
                {
                    depth = w;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void tbStartx_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                float w = float.Parse(tbStartx.Text);
                if (w != 0)
                {
                    startx = w;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void tbStartz_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                float w = float.Parse(tbStartz.Text);
                if (w != 0)
                {
                    startz = w;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
