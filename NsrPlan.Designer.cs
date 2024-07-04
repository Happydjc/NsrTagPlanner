
namespace NsrTagPlanner
{
    partial class NsrPlan
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NsrPlan));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            value = new System.Windows.Forms.TextBox();
            colorLimit = new System.Windows.Forms.CheckedListBox();
            operationCommandPanel = new System.Windows.Forms.Panel();
            refreshUsageTime = new System.Windows.Forms.Button();
            operation = new System.Windows.Forms.Button();
            icons = new System.Windows.Forms.ImageList(components);
            selectionTablePanel = new System.Windows.Forms.TableLayoutPanel();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            tagList = new System.Windows.Forms.ListView();
            TagName = new System.Windows.Forms.ColumnHeader();
            TagDesc = new System.Windows.Forms.ColumnHeader();
            LockTime = new System.Windows.Forms.ColumnHeader();
            resultPanel = new System.Windows.Forms.RichTextBox();
            progress = new System.Windows.Forms.ProgressBar();
            tagMenu = new System.Windows.Forms.ContextMenuStrip(components);
            chooseAll = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            allowSensitive = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            operationCommandPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tagMenu.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(value);
            splitContainer1.Panel1.Controls.Add(colorLimit);
            splitContainer1.Panel1.Controls.Add(operationCommandPanel);
            splitContainer1.Panel1.Controls.Add(selectionTablePanel);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(944, 601);
            splitContainer1.SplitterDistance = 215;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 6;
            // 
            // value
            // 
            value.Dock = System.Windows.Forms.DockStyle.Fill;
            value.Location = new System.Drawing.Point(0, 206);
            value.Multiline = true;
            value.Name = "value";
            value.ReadOnly = true;
            value.Size = new System.Drawing.Size(215, 245);
            value.TabIndex = 7;
            // 
            // colorLimit
            // 
            colorLimit.Dock = System.Windows.Forms.DockStyle.Bottom;
            colorLimit.FormattingEnabled = true;
            colorLimit.Items.AddRange(new object[] { "蓝", "黄", "橙", "粉" });
            colorLimit.Location = new System.Drawing.Point(0, 451);
            colorLimit.Name = "colorLimit";
            colorLimit.Size = new System.Drawing.Size(215, 67);
            colorLimit.TabIndex = 8;
            // 
            // operationCommandPanel
            // 
            operationCommandPanel.Controls.Add(refreshUsageTime);
            operationCommandPanel.Controls.Add(operation);
            operationCommandPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            operationCommandPanel.Location = new System.Drawing.Point(0, 518);
            operationCommandPanel.Name = "operationCommandPanel";
            operationCommandPanel.Size = new System.Drawing.Size(215, 83);
            operationCommandPanel.TabIndex = 6;
            // 
            // refreshUsageTime
            // 
            refreshUsageTime.BackgroundImage = Properties.Resources.reload_all_tabs;
            refreshUsageTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            refreshUsageTime.Dock = System.Windows.Forms.DockStyle.Right;
            refreshUsageTime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            refreshUsageTime.Location = new System.Drawing.Point(131, 0);
            refreshUsageTime.Name = "refreshUsageTime";
            refreshUsageTime.Size = new System.Drawing.Size(84, 83);
            refreshUsageTime.TabIndex = 3;
            refreshUsageTime.Text = "更新使用次数";
            refreshUsageTime.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            refreshUsageTime.UseMnemonic = false;
            refreshUsageTime.UseVisualStyleBackColor = true;
            refreshUsageTime.Click += RefreshUsageTime_Click;
            // 
            // operation
            // 
            operation.Dock = System.Windows.Forms.DockStyle.Left;
            operation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            operation.ImageIndex = 2;
            operation.ImageList = icons;
            operation.Location = new System.Drawing.Point(0, 0);
            operation.Name = "operation";
            operation.Size = new System.Drawing.Size(84, 83);
            operation.TabIndex = 2;
            operation.Text = "求解";
            operation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            operation.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            operation.UseMnemonic = false;
            operation.UseVisualStyleBackColor = true;
            operation.Click += Operation_Click;
            // 
            // icons
            // 
            icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            icons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("icons.ImageStream");
            icons.TransparentColor = System.Drawing.Color.Transparent;
            icons.Images.SetKeyName(0, "lock_closed.png");
            icons.Images.SetKeyName(1, "lock_open.png");
            icons.Images.SetKeyName(2, "plan.png");
            icons.Images.SetKeyName(3, "find.png");
            icons.Images.SetKeyName(4, "delete.png");
            icons.Images.SetKeyName(5, "reload_all_tabs.png");
            // 
            // selectionTablePanel
            // 
            selectionTablePanel.AutoSize = true;
            selectionTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            selectionTablePanel.ColumnCount = 3;
            selectionTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            selectionTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            selectionTablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            selectionTablePanel.Location = new System.Drawing.Point(0, 0);
            selectionTablePanel.Name = "selectionTablePanel";
            selectionTablePanel.RowCount = 5;
            selectionTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            selectionTablePanel.Size = new System.Drawing.Size(215, 206);
            selectionTablePanel.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Cursor = System.Windows.Forms.Cursors.VSplit;
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tagList);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(resultPanel);
            splitContainer2.Panel2.Controls.Add(progress);
            splitContainer2.Size = new System.Drawing.Size(724, 601);
            splitContainer2.SplitterDistance = 532;
            splitContainer2.TabIndex = 3;
            // 
            // tagList
            // 
            tagList.CheckBoxes = true;
            tagList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { TagName, TagDesc, LockTime });
            tagList.Dock = System.Windows.Forms.DockStyle.Fill;
            tagList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            tagList.FullRowSelect = true;
            tagList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            tagList.Location = new System.Drawing.Point(0, 0);
            tagList.Name = "tagList";
            tagList.ShowItemToolTips = true;
            tagList.Size = new System.Drawing.Size(532, 601);
            tagList.TabIndex = 0;
            tagList.UseCompatibleStateImageBehavior = false;
            tagList.View = System.Windows.Forms.View.Details;
            tagList.KeyPress += TagList_KeyPress;
            // 
            // TagName
            // 
            TagName.Text = "名称";
            TagName.Width = 150;
            // 
            // TagDesc
            // 
            TagDesc.Text = "达成条件";
            TagDesc.Width = 300;
            // 
            // LockTime
            // 
            LockTime.Text = "解锁";
            LockTime.Width = 100;
            // 
            // resultPanel
            // 
            resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            resultPanel.Location = new System.Drawing.Point(0, 0);
            resultPanel.Name = "resultPanel";
            resultPanel.ReadOnly = true;
            resultPanel.Size = new System.Drawing.Size(188, 578);
            resultPanel.TabIndex = 3;
            resultPanel.Text = "";
            resultPanel.DoubleClick += ResultPanel_DoubleClick;
            // 
            // progress
            // 
            progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            progress.Location = new System.Drawing.Point(0, 578);
            progress.Name = "progress";
            progress.Size = new System.Drawing.Size(188, 23);
            progress.TabIndex = 4;
            // 
            // tagMenu
            // 
            tagMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { chooseAll, toolStripMenuItem1, allowSensitive, toolStripMenuItem2 });
            tagMenu.Name = "tagMenu";
            tagMenu.Size = new System.Drawing.Size(137, 60);
            // 
            // chooseAll
            // 
            chooseAll.Image = Properties.Resources.reload_all_tabs;
            chooseAll.Name = "chooseAll";
            chooseAll.Size = new System.Drawing.Size(136, 22);
            chooseAll.Text = "全选";
            chooseAll.Click += ChooseAll_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(133, 6);
            // 
            // allowSensitive
            // 
            allowSensitive.Checked = true;
            allowSensitive.CheckState = System.Windows.Forms.CheckState.Checked;
            allowSensitive.Name = "allowSensitive";
            allowSensitive.Size = new System.Drawing.Size(136, 22);
            allowSensitive.Text = "允许敏感词";
            allowSensitive.Click += AllowSensitive_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(133, 6);
            // 
            // PlannerWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(944, 601);
            Controls.Add(splitContainer1);
            Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Name = "PlannerWindow";
            ShowIcon = false;
            Text = "Form1";
            FormClosing += PlannerWindow_FormClosing;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            operationCommandPanel.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tagMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel selectionTablePanel;
        private System.Windows.Forms.ImageList icons;
        private System.Windows.Forms.Button operation;
        private System.Windows.Forms.Panel operationCommandPanel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox resultPanel;
        private System.Windows.Forms.ContextMenuStrip tagMenu;
        private System.Windows.Forms.ToolStripMenuItem chooseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem allowSensitive;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.TextBox value;
        private System.Windows.Forms.ListView tagList;
        private System.Windows.Forms.ColumnHeader TagName;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.ColumnHeader TagDesc;
        private System.Windows.Forms.CheckedListBox colorLimit;
        private System.Windows.Forms.ColumnHeader LockTime;
        private System.Windows.Forms.Button refreshUsageTime;
    }
}

