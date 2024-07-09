using Nsr;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Nsr.Planner
{
    public partial class MDIMain : Form
    {
        private int childFormNumber = 0;
        private NsrData nsrData;


        public MDIMain()
        {
            InitializeComponent();
            UpdateTags();
            ShowNewForm();
        }

        private void ShowNewForm(object sender, EventArgs e) => ShowNewForm();

        private void ShowNewForm()
        {
            NsrPlanProto newWindow = new($"规划{++childFormNumber}", nsrData) { MdiParent = this };
            Program.NsrDataAdapter.LoadPlan(newWindow);
            newWindow.Show();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e) => toolStrip.Visible = toolBarToolStripMenuItem.Checked;

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e) => statusStrip.Visible = statusBarToolStripMenuItem.Checked;

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileVertical);

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.ArrangeIcons);

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllChirdren();
        }

        private void CloseAllChirdren()
        {
            foreach (var childForm in MdiChildren)
                childForm.Close();
        }

        private void UpdateDataToolStripMenuItem_Click(object sender, EventArgs e) => UpdateTags();

        private void UpdateTags()
        {
            nsrData = Program.NsrDataAdapter.GetData();
            toolStripStatusLabel.Text = nsrData?.NsrTags == null ? "" : $" {nsrData.NsrTags.Count} {Properties.Resources.NsrTag}. ";

            toolStripStatusLabel.Text += nsrData?.NsrComponents == null ? "" : $"{nsrData.NsrComponents.Count} {Properties.Resources.NsrComponent}. ";

            foreach (var childForm in MdiChildren)
            {
                if (childForm is INsrOperateUI uI)
                    uI.RefreshTags(nsrData.NsrTags);
            }
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 数据重载RToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MDIMain_FormClosing(object sender, FormClosingEventArgs e)
        { 
            CloseAllChirdren();
            Application.Exit();
        }
    }
}
