using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using netDxf;
using netDxf.Entities;
using static netDxf.Entities.HatchBoundaryPath;

namespace BezierAirfoilDesigner
{
    public partial class frmSave : Form
    {
        public List<PointD> controlPointsTop { get; set; }
        public List<PointD> controlPointsBottom { get; set; }

        public List<PointD> pointsTop { get; set; }
        public List<PointD> pointsBottom { get; set; }

        public string loadedAirfoilName { get; set; }


        public frmSave()
        {
            InitializeComponent();
        }

        private void frmSave_Load(object sender, EventArgs e)
        {
            cmbCoordinateStyle.Items.Add("x,y");
            cmbCoordinateStyle.Items.Add("0,y,z");
            cmbCoordinateStyle.Items.Add("x,0,z");
            cmbCoordinateStyle.Items.Add("x,y,0");
            cmbCoordinateStyle.SelectedIndex = 0;
        }

        public static void SaveTextToFile(string text, string path)
        {
            try
            {
                File.WriteAllText(path, text);
                MessageBox.Show("File Saved Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private bool ParseAndApplyChord(string chordText, List<PointD> points)
        {
            // Try to parse with invariant culture
            if (!double.TryParse(chordText.Replace(",", "."), CultureInfo.InvariantCulture, out double chord))
            {
                MessageBox.Show("Invalid Chord");
                return false;
            }

            // Multiply all points by chord
            for (int i = 0; i < points.Count; i++)
            {
                points[i].X *= chord;
                points[i].Y *= chord;
            }

            return true;
        }

        private string ShowSaveDialog(string fileName, string filter)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = filter;
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            sfd.FileName = fileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }

            return null;
        }

        // This method appends points to file content string. It can append points in regular or reverse order.
        private string AppendPointsToFileContent(string fileContents, List<PointD> points, bool reverseOrder)
        {
            // Determine the start, end, and step for the loop depending on whether we need to append points in reverse order
            int start = reverseOrder ? points.Count - 1 : 0;
            int end = reverseOrder ? -1 : points.Count;
            int step = reverseOrder ? -1 : 1;

            // Loop through the points
            for (int i = start; i != end; i += step)
            {
                // Format X and Y coordinates. /*If a coordinate is not negative, add a space in front of it.*/
                string xCoord = points[i].X >= 0 ? $"{points[i].X:F16}" : $"{points[i].X:F16}";
                string yCoord = points[i].Y >= 0 ? $"{points[i].Y:F16}" : $"{points[i].Y:F16}";

                // Check the coordinate style and append the point to fileContents accordingly
                switch (cmbCoordinateStyle.Text)
                {
                    case "x,y":
                        // Append point in "x,y" style
                        fileContents += $"{xCoord} {yCoord}" + System.Environment.NewLine;
                        break;
                    case "0,y,z":
                        // Append point in "0,y,z" style
                        fileContents += $"{0} {xCoord} {yCoord}" + System.Environment.NewLine;
                        break;
                    case "x,0,z":
                        // Append point in "x,0,z" style
                        fileContents += $"{xCoord} {0} {yCoord}" + System.Environment.NewLine;
                        break;
                    case "x,y,0":
                        // Append point in "x,y,0" style
                        fileContents += $"{xCoord} {yCoord} {0}" + System.Environment.NewLine;
                        break;
                    default:
                        // Append point in "x,y" style in case nothing is entered
                        fileContents += $"{xCoord} {yCoord}" + System.Environment.NewLine;
                        break;
                }

            }

            // Return the modified fileContents
            return fileContents;
        }

        private string PrepareAirfoilName(string airfoilName)
        {
            // Remove "Bezier " prefix from the loaded airfoil name if it exists
            return airfoilName.StartsWith("Bezier ") ? airfoilName.Substring(7) : airfoilName;
        }

        private void btnSaveDat_Click(object sender, EventArgs e)
        {
            if (!ParseAndApplyChord(txtChord.Text, pointsTop) || !ParseAndApplyChord(txtChord.Text, pointsBottom))
            {
                return;
            }

            string path = ShowSaveDialog("Bezier " + PrepareAirfoilName(loadedAirfoilName), "dat files (*.dat)|*.dat|All files (*.*)|*.*");
            if (path != null)
            {
                string airfoilName = Path.GetFileNameWithoutExtension(path);
                string fileContents = airfoilName + System.Environment.NewLine;

                fileContents = AppendPointsToFileContent(fileContents, pointsTop, true);
                fileContents = AppendPointsToFileContent(fileContents, pointsBottom.Skip(1).ToList(), false);

                fileContents = fileContents.Replace(',', '.');

                SaveTextToFile(fileContents, path);
            }
        }

        private void btnSaveBezDat_Click(object sender, EventArgs e)
        {
            if (!ParseAndApplyChord(txtChord.Text, controlPointsTop) || !ParseAndApplyChord(txtChord.Text, controlPointsBottom))
            {
                return;
            }

            string path = ShowSaveDialog(PrepareAirfoilName(loadedAirfoilName), "Bezier dat files (*.bez.dat)|*.bez.dat|All files (*.*)|*.*");
            if (path != null)
            {
                string airfoilName = Path.GetFileNameWithoutExtension(path);
                string fileContents = airfoilName + System.Environment.NewLine;

                fileContents = AppendPointsToFileContent(fileContents, controlPointsTop, true);
                fileContents = AppendPointsToFileContent(fileContents, controlPointsBottom.Skip(1).ToList(), false);

                fileContents = fileContents.Replace(',', '.');

                SaveTextToFile(fileContents, path);
            }
        }

        private void btnSaveBez_Click(object sender, EventArgs e)
        {
            if (!ParseAndApplyChord(txtChord.Text, controlPointsTop) || !ParseAndApplyChord(txtChord.Text, controlPointsBottom))
            {
                return;
            }

            string path = ShowSaveDialog(PrepareAirfoilName(loadedAirfoilName), "Bezier files (*.bez)|*.bez|All files (*.*)|*.*");
            if (path != null)
            {
                string airfoilName = Path.GetFileNameWithoutExtension(path);
                string fileContents = airfoilName + System.Environment.NewLine;

                fileContents += "Top Start" + System.Environment.NewLine;
                fileContents = AppendPointsToFileContent(fileContents, controlPointsTop, false);
                fileContents += "Top End" + System.Environment.NewLine;

                fileContents += "Bottom Start" + System.Environment.NewLine;
                fileContents = AppendPointsToFileContent(fileContents, controlPointsBottom, false);
                fileContents += "Bottom End" + System.Environment.NewLine;

                fileContents = fileContents.Replace(',', '.');

                SaveTextToFile(fileContents, path);
            }
        }

        private void btnSaveDXF_Click(object sender, EventArgs e)
        {
            //check if number of control points is greater than 11
            if (controlPointsTop.Count > 11 || controlPointsBottom.Count > 11)
            {
                MessageBox.Show("DXF export is limited to degree 10.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Try to parse with invariant culture
            if (!double.TryParse(txtChord.Text.Replace(",", "."), CultureInfo.InvariantCulture, out double chord))
            {
                MessageBox.Show("Invalid Chord");
                return;
            }

            // Create a new DXF document
            DxfDocument dxf = new DxfDocument();

            // Define control points for the spline
            List<Vector3> controlPointsTopVector = new();

            // Add top control points to the list
            foreach (PointD point in controlPointsTop)
            {
                switch (cmbCoordinateStyle.Text)
                {
                    case "x,y":
                        // show error message and return if "x,y" is selected
                        MessageBox.Show("Please select a different coordinate style for DXF export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case "0,y,z":
                        // Append point in "0,y,z" style
                        controlPointsTopVector.Add(new Vector3(0, point.Y * chord, point.X * chord));
                        break;
                    case "x,0,z":
                        // Append point in "x,0,z" style
                        controlPointsTopVector.Add(new Vector3(point.X * chord, 0, point.Y * chord));
                        break;
                    case "x,y,0":
                        // Append point in "x,y,0" style
                        controlPointsTopVector.Add(new Vector3(point.X * chord, point.Y * chord, 0));
                        break;
                    default:
                        // show error message and return if nothing or something else is selected
                        MessageBox.Show("Please select a different coordinate style for DXF export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }

            // Define control points for the spline
            List<Vector3> controlPointsBottomVector = new();

            // Add top control points to the list
            foreach (PointD point in controlPointsBottom)
            {
                switch (cmbCoordinateStyle.Text)
                {
                    case "x,y":
                        // show error message and return if "x,y" is selected
                        MessageBox.Show("Please select a different coordinate style for DXF export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case "0,y,z":
                        // Append point in "0,y,z" style
                        controlPointsBottomVector.Add(new Vector3(0, point.Y * chord, point.X * chord));
                        break;
                    case "x,0,z":
                        // Append point in "x,0,z" style
                        controlPointsBottomVector.Add(new Vector3(point.X * chord, 0, point.Y * chord));
                        break;
                    case "x,y,0":
                        // Append point in "x,y,0" style
                        controlPointsBottomVector.Add(new Vector3(point.X * chord, point.Y * chord, 0));
                        break;
                    default:
                        // show error message and return if nothing or something else is selected
                        MessageBox.Show("Please select a different coordinate style for DXF export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
            }

            // Create a spline with the control points
            netDxf.Entities.Spline splineTop = new netDxf.Entities.Spline(controlPointsTopVector, null, degree: short.Parse((controlPointsTop.Count - 1).ToString()));
            netDxf.Entities.Spline splineBottom = new netDxf.Entities.Spline(controlPointsBottomVector, null, degree: short.Parse((controlPointsBottom.Count - 1).ToString()));

            // Add the spline to the DXF document
            dxf.Entities.Add(splineTop);
            dxf.Entities.Add(splineBottom);

            // Save the DXF document to a file
            //dxf.Save(loadedAirfoilName + ".dxf", );

            // Create a SaveFileDialog to get the save location from the user
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = PrepareAirfoilName(loadedAirfoilName);
            saveFileDialog.Filter = "DXF Files (*.dxf)|*.dxf";
            saveFileDialog.Title = "Save as DXF";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                dxf.Save(saveFileDialog.FileName);
                MessageBox.Show("File Saved Successfully!");
            }
        }
    }
}
