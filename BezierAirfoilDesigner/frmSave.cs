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

        private string AppendPointsToFileContent(string fileContents, List<PointD> points, bool top)
        {
            int start = top ? points.Count - 1 : 1;
            int end = top ? -1 : points.Count;
            int step = top ? -1 : 1;

            for (int i = start; i != end; i += step)
            {
                fileContents += ($"{points[i].X:F16} {points[i].Y:F16}" + System.Environment.NewLine);
            }

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

            string airfoilName = PrepareAirfoilName(loadedAirfoilName);
            string fileContents = "Bezier " + airfoilName + System.Environment.NewLine;

            fileContents = AppendPointsToFileContent(fileContents, pointsTop, true);
            fileContents = AppendPointsToFileContent(fileContents, pointsBottom, false);

            fileContents = fileContents.Replace(',', '.');

            string path = ShowSaveDialog("Bezier " + airfoilName, "dat files (*.dat)|*.dat|All files (*.*)|*.*");
            if (path != null)
            {
                SaveTextToFile(fileContents, path);
            }
        }

        private void btnSaveBezDat_Click(object sender, EventArgs e)
        {
            if (!ParseAndApplyChord(txtChord.Text, controlPointsTop) || !ParseAndApplyChord(txtChord.Text, controlPointsBottom))
            {
                return;
            }

            string airfoilName = PrepareAirfoilName(loadedAirfoilName);
            string fileContents = airfoilName + System.Environment.NewLine;

            fileContents = AppendPointsToFileContent(fileContents, controlPointsTop, true);
            fileContents = AppendPointsToFileContent(fileContents, controlPointsBottom, false);

            fileContents = fileContents.Replace(',', '.');

            string path = ShowSaveDialog(airfoilName, "Bezier dat files (*.bez.dat)|*.bez.dat|All files (*.*)|*.*");
            if (path != null)
            {
                SaveTextToFile(fileContents, path);
            }
        }

        private void btnSaveBez_Click(object sender, EventArgs e)
        {
            if (!ParseAndApplyChord(txtChord.Text, controlPointsTop) || !ParseAndApplyChord(txtChord.Text, controlPointsBottom))
            {
                return;
            }

            string airfoilName = PrepareAirfoilName(loadedAirfoilName);
            string fileContents = airfoilName + System.Environment.NewLine;

            fileContents += "Top Start" + System.Environment.NewLine;
            fileContents = AppendPointsToFileContent(fileContents, controlPointsTop, true);
            fileContents += "Top End" + System.Environment.NewLine;

            fileContents += "Bottom Start" + System.Environment.NewLine;
            fileContents = AppendPointsToFileContent(fileContents, controlPointsBottom, false);
            fileContents += "Bottom End" + System.Environment.NewLine;

            fileContents = fileContents.Replace(',', '.');

            string path = ShowSaveDialog(airfoilName, "Bezier files (*.bez)|*.bez|All files (*.*)|*.*");
            if (path != null)
            {
                SaveTextToFile(fileContents, path);
            }
        }

    }
}
