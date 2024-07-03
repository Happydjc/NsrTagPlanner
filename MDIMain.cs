using NsrModels;
using System;
using System.Windows.Forms;

namespace NsrTagPlanner
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
            PlannerWindow newWindow = new($"规划{++childFormNumber}", nsrData) { MdiParent = this };
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
            foreach (var childForm in MdiChildren)
                childForm.Close();
        }

        private void UpdateDataToolStripMenuItem_Click(object sender, EventArgs e) => UpdateTags();

        private void UpdateTags()
        {
            nsrData = Program.NsrDataAdapter.GetData();
            toolStripStatusLabel.Text
                = nsrData?.NsrTags == null ? "" : $" {nsrData.NsrTags.Count}条完整Tag数据已读取."
                + nsrData?.NsrComponents == null ? "" : $"{nsrData.NsrComponents.Count}条完整部件数据已读取.";
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
    }
}
