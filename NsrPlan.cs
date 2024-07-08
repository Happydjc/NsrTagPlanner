using Nsr;
using Nsr.Exclusion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nsr.Planner
{
    public partial class NsrTagStatus : Form, INsrOperateUI
    {
        #region INsrOperationUI接口
        public bool Cancel { get; private set; }
        public NsrTags AllTags { get; private set; }
        public void Clear()
        {
            Text = FormName;
            resultPanel.ResetText();
        }

        string label;

        public void WriteLine(object o = null)
        {
            label = $"{o}";
            Text = $"{FormName} - {o}";
            resultPanel.AppendText($"{o}\r\n");
        }

        public void Init(int max) => (progress.Value, progress.Maximum) = (0, max);

        public int Progress
        {
            get => progress.Value;
            set => (Text, progress.Value) = ($"{FormName} -{label} ({100 * value / progress.Maximum}%)", value);
        }

        public void DoEvents() => Application.DoEvents();

        public List<NsrTagSetting> TagSettings { get; private set; } = new();

        /// <summary>
        /// 重置所有标签
        /// </summary>
        /// <param name="tags"></param>
        public void RefreshTags(NsrTags tags)
        {
            RefreshPlan();

            for (int i = 0; i < 5; i++)
                FillSelection(selections[i], tags.ToArray());

            foreach (var tag in AllTags.Concat(tags).Distinct())
            {
                if (AllTags.Contains(tag) && !tags.Contains(tag))
                {
                    //should remove
                    tagList.Items.RemoveByKey(tag.Name);
                }
                if (!AllTags.Contains(tag) && tags.Contains(tag))
                {
                    ListViewItem tagItem = CreateItem(tag);
                    //should add
                    tagList.Items.Add(tagItem);
                }
            }

            AllTags=tags;
            SetPlanSettings();
            foreach (var menuItem in modelMenuItems)
                tagMenu.Items.Remove(menuItem);
            modelMenuItems.Clear();
            foreach (var tag in AllTags)
                FillMenuByTag(tag);
        }

        public void SetPlanSettings(List<NsrTagSetting> nsrPlanTagSettings)
        {
            TagSettings = nsrPlanTagSettings;
            SetPlanSettings();
        }
        void SetPlanSettings()
        {
            foreach (ListViewItem tagItem in tagList.Items)
            {
                if (TagSettings!=null && TagSettings.Any(s => s.TagName == tagItem.Name))
                {
                    NsrTagSetting tagSetting = TagSettings.Find(s => s.TagName == tagItem.Name);
                    tagItem.Checked = tagSetting.Checked;
                    if (tagSetting.UsingCycle >= 0) UpdateTagUsageTime(tagItem, tagSetting.UsingCycle);
                }
            }
        }

        public void RefreshPlan()
        {
            TagSettings.Clear();
            foreach (ListViewItem tagItem in tagList.Items)
            {
                int cycleTime = -1;
                if (tagItem.SubItems[2].Tag != null)
                    _ = int.TryParse(tagItem.SubItems[2].Tag.ToString(), out cycleTime);
                TagSettings.Add(new(tagItem.Text, tagItem.Checked, cycleTime));
            }
        }
        #endregion

        #region 初始化
        public string FormName { get; init; }
        private readonly Button[] lockCommands = new Button[5];
        private readonly ComboBox[] selections = new ComboBox[5];
        private readonly Button[] deleteCommands = new Button[5];
        private readonly ToolTip[] SelectionTips = new ToolTip[5];
        private readonly List<ToolStripMenuItem> modelMenuItems = new();
        public List<NsrComponent> AllComponents { get; private init; }

        internal NsrTagStatus(string name, NsrData nsrData)
        {
            (FormName, AllTags, AllComponents) = (name, nsrData.NsrTags, nsrData.NsrComponents);
            InitializeComponent();
            colorLimit.SetSelected(0, true);
            InitByTags();
            Clear();
        }

        private void InitByTags()
        {
            selectionTablePanel.Controls.Clear();
            for (int i = 0; i < 5; i++)
            {
                lockCommands[i] = CreateButton("Lock", i, 0, false, 1, Lock_Click);
                selections[i] = CreateComboBox("Selection", i, 1, lockCommands[i]);
                deleteCommands[i] = CreateButton("Delete", i, 2, i, 4, Delete_Click);
            }

            tagList.Items.Clear();
            foreach (var menuItem in modelMenuItems)
                tagMenu.Items.Remove(menuItem);
            modelMenuItems.Clear();
            foreach (NsrTag tag in AllTags)
            {
                tagList.Items.Add(CreateItem(tag));
                FillMenuByTag(tag);
            }
            tagList.ContextMenuStrip = tagMenu;

        }

        static ListViewItem CreateItem(NsrTag tag)
        {
            ListViewItem item = new()
            {
                Name = tag.Name,
                Text = tag.Text,
                Checked = true,
                Tag = tag,
                ForeColor = tag.IsSensitive ? Color.White : tag.Rarity.RarityColor(),
                BackColor = !tag.IsSensitive ? Color.White : tag.Rarity.RarityColor(),
            };
            item.SubItems.Add(tag.Description);
            item.SubItems.Add("");
            return item;
        }
        private void FillMenuByTag(NsrTag tag)
        {
            if (tag.Exclusions.Any(e => e is NsrDescMutualExclusion) && !modelMenuItems.Any(i => tag.Exclusions.Contains((NsrDescMutualExclusion)i.Tag)))
            {
                NsrDescMutualExclusion dm = (NsrDescMutualExclusion)tag.Exclusions.First(e => e is NsrDescMutualExclusion);
                ToolStripMenuItem menuItem = new(dm.ToString(), null, ModelToolStripMenuItem_Click)
                {
                    Checked = true,
                    Tag = dm
                };
                tagMenu.Items.Add(menuItem);
                modelMenuItems.Add(menuItem);
            }
        }

        private ComboBox CreateComboBox(string name, int index, int columnId, object tag)
        {
            ComboBox selection = new()
            {
                AutoCompleteMode = AutoCompleteMode.Suggest,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Location = new Point(2, 6),
                Margin = new Padding(2, 6, 2, 6),
                FormattingEnabled = true,
                DisplayMember = "Name",
                ValueMember = "Name",
            };
            FillSelection(selection, AllTags.ToArray());
            selection.MouseEnter += Selection_TooltipRefreshed;
            selection.SelectedIndexChanged += Selection_Changed;
            selection.SelectedIndexChanged += Selection_TooltipRefreshed;
            selection.TextChanged += Selection_Changed;
            selection.TextChanged += Selection_TooltipRefreshed;
            AddToTable(name, selection, columnId, index, tag);
            SelectionTips[index] = new()
            {
                InitialDelay = 200,
                AutoPopDelay = 10000,
                ReshowDelay = 200,
                ShowAlways = true,
                IsBalloon = true
            };
            return selection;
        }

        private static void FillSelection(ComboBox selection, NsrTag[] tags)
        {
            selection.Items.Clear();
            selection.Items.AddRange(tags);
        }

        private void AddToTable(string name, Control control, int column, int row, object tag)
        {
            control.Name = $"{name}{row}";
            control.TabIndex = row;
            control.Dock = DockStyle.Fill;
            control.Tag = tag;

            selectionTablePanel.Controls.Add(control, column, row);
        }

        private Button CreateButton(string name, int index, int columnId, object tag, int iconIndex, EventHandler funcClick)
        {
            Button button = new()
            {
                AutoSize = true,
                Location = new Point(2, 2),
                Margin = new Padding(2),
                Size = new Size(36, 36),
                UseMnemonic = true,
            };
            PaintIcon(button, iconIndex);
            button.Click += funcClick;
            AddToTable(name, button, columnId, index, tag);
            return button;
        }

        #endregion

        #region 事件处理
        private void ModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;
            menu.Checked = !menu.Checked;
            for (int i = 0; i < tagList.Items.Count; i++)
            {
                if (tagList.Items[i].Tag is NsrTag tag)
                    if (tag.Exclusions.Contains((NsrDescMutualExclusion)menu.Tag))
                        tagList.Items[i].Checked = menu.Checked && (!tag.IsSensitive || allowSensitive.Checked);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Button deleteButton = (Button)sender;
            UpdateTagUsageTime(deleteButton, NsrTag.MinReuseRounds - 1);
        }

        private void RefreshUsageTime_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in tagList.Items)
            {
                if (item.SubItems[2].Tag is int time)
                    UpdateTagUsageTime(item, ++time);
            }
            foreach (var button in deleteCommands)
                UpdateTagUsageTime(button, 1);
        }

        static int keypressTime = 0;
        private void TagList_KeyPress(object sender, KeyPressEventArgs e)
        {
            keypressTime++;
            if (keypressTime > 1)
            {
                keypressTime = 0;
                return;
            }
            int inc = e.KeyChar switch
            {
                '+' => 1,
                '-' => -1,
                _ => 0
            };

            if (inc != 0 && sender is ListView lv)
            {
                if (lv.SelectedItems.Count == 1)
                {
                    var item = lv.SelectedItems[0];
                    int time = 0;
                    if (item.SubItems[2].Tag is int @int)
                    {
                        time = @int;
                    }
                    time += inc;
                    UpdateTagUsageTime(item, time);
                }
            }
        }

        private void UpdateTagUsageTime(Button deleteButton, int time)
        {
            int commandId = (int)deleteButton.Tag;
            string tagName = selections[commandId].Text;
            selections[commandId].Text = string.Empty;
            if (AllTags.Any(tag => tag.Name == tagName))
                UpdateTagUsageTime(tagList.Items[tagName], time);
        }

        private static void UpdateTagUsageTime(ListViewItem item, int time)
        {
            time %= NsrTag.MinReuseRounds;
            if (time < 0) time += NsrTag.MinReuseRounds;
            if (time == 0)
            {
                item.SubItems[2].Text = null;
                item.SubItems[2].Tag = null;
                item.Checked = true;
            }
            else
            {
                item.SubItems[2].Text = $"{time}次前用过";
                item.SubItems[2].Tag = time;
                item.Checked = false;
            }
        }

        private void Lock_Click(object sender, EventArgs e)
        {
            Button lockButton = (Button)sender;
            bool locked = !(bool)lockButton.Tag;
            lockButton.Tag = locked;
            nint index = int.Parse(lockButton.Name[4..]);
            selections[index].Enabled = !locked;
            PaintIcon(lockButton, locked ? 0 : 1);
        }

        private void Selection_Changed(object sender, EventArgs e)
        {
            List<NsrTag> tags = new();
            foreach (ComboBox selection in selections)
                if (AllTags.Any(tag => tag.Name == selection.Text))
                    tags.Add(AllTags.First(tag => tag.Name == selection.Text));
            value.Text = NsrDataAdapter.ShowNsrTagOperateList(new NsrSelectedTags(tags));
        }

        private void Selection_TooltipRefreshed(object sender, EventArgs e)
        {
            if (sender is ComboBox combo)
            {
                if (selections.Contains(combo))
                {
                    int index = Array.IndexOf(selections, combo);
                    SelectionTips[index].RemoveAll();
                    SelectionTips[index].SetToolTip(combo, AllTags.FirstOrDefault(t => t.Name == combo.Text)?.Description);
                }

            }
        }
        private void PaintIcon(Button button, int iconId) => button.Image = icons.Images[iconId];

        private void Operation_Click(object sender, EventArgs e) => Calc(((Button)sender).Text, Operation);

        private void Operation(NsrSelectedTags selectedTags, IList<NsrTag> tags)
        {
            NsrSelectedTags result = NsrSelectedTags.Empty;
            for (int i = 2; i >= 0; i--)
            {
                result = new NsrOperateAlgorithm(tags, this, (int)Math.Pow(10, i)).Operate(selectedTags);
                if (result.Count > 0) break;
            }
            if (result.Count == 0)
                throw new ApplicationException("现有标签不足以完成优化!");
            int j = 0;
            for (nint i = 0; i < 5; i++)
                if (!(bool)lockCommands[i].Tag)
                    selections[i].Text = result[j++].Name;
        }

        private void ChooseAll_Click(object sender, EventArgs e)
        {
            allowSensitive.Checked = true;
            foreach (var item in modelMenuItems)
                item.Checked = true;
            for (int i = 0; i < tagList.Items.Count; i++)
                tagList.Items[i].Checked = true;
        }

        private void AllowSensitive_Click(object sender, EventArgs e)
        {
            allowSensitive.Checked = !allowSensitive.Checked;
            for (int i = 0; i < tagList.Items.Count; i++)
            {
                ListViewItem item = tagList.Items[i];
                if (item.Tag is NsrTag tag)
                {
                    if (tag.Exclusions.Any(ex => ex is NsrDescMutualExclusion))
                    {
                        foreach (var menu in modelMenuItems)
                            if (tag.Exclusions.Contains((NsrDescMutualExclusion)menu.Tag))
                                item.Checked = menu.Checked && (!tag.IsSensitive || allowSensitive.Checked);
                    }
                    else
                        item.Checked = !tag.IsSensitive || allowSensitive.Checked;
                }
            }
        }
        private void ResultPanel_DoubleClick(object sender, EventArgs e) => Cancel = true;

        #endregion

        bool Validate(NsrTags tags)
        {
            NsrTags temp = new();
            foreach (NsrTag tag in tags)
            {
                NsrValidateException validateMessage = temp.Validate(tag);
                if (!validateMessage.IsValid)
                {
                    validateMessage.Messages.ForEach(message => { WriteLine(message); });
                    return false;
                }
                else temp.Add(tag);
            }
            return true;
        }

        private void Calc(string name, Action<NsrSelectedTags, IList<NsrTag>> action)
        {
            NsrSelectedTags conditions = new(selections
                .Where(selection => (bool)((Button)selection.Tag).Tag)
                .Select(selection => string.IsNullOrEmpty(selection.Text) ? new NsrTag() : AllTags.First(tag => tag.Name == selection.Text)));
            List<NsrTag> allTags = new();
            foreach (ListViewItem item in tagList.CheckedItems)
                if (item.Tag is NsrTag tag)
                {
                    //颜色管理
                    if (tag.Name.EndsWith("色") || tag.Name.EndsWith("色"))
                    {
                        bool colorLimited = true;
                        foreach (var color in colorLimit.CheckedItems)
                            if (tag.Name.StartsWith(color.ToString()))
                            {
                                colorLimited = true;
                                break;
                            }
                        if (colorLimited) continue;
                    }
                    allTags.Add(tag);
                }
            Clear();
            LockControls(true);
            try
            {
                if (Validate(conditions))
                {
                    WriteLine($"空位{5 - conditions.Count},可用标签{allTags.Count}枚.");
                    if (conditions.Count == 5) throw new ApplicationException("所有数据已锁定,无从搜索.");
                    WriteLine($"{name}开始");
                    DateTime startTime = DateTime.Now;
                    Progress = 0;
                    Cancel = false;
                    action(conditions, allTags);
                    WriteLine($"规划完成，用时{DateTime.Now - startTime}s.");
                }
            }
            catch (ApplicationException ex)
            {
                WriteLine($"{ex.Message}");
            }
            catch (NsrOperateException ex)
            {
                WriteLine($"{ex.Sender.GetType().Name}:{ex.Message}");
            }
            //catch (Exception ex)
            //{
            //    WriteLine(ex);
            //}
            finally
            {
                LockControls(false);
            }
        }

        private void LockControls(bool isLock)
        {
            operationCommandPanel.Enabled = !isLock;
            selectionTablePanel.Enabled = !isLock;
            tagList.CheckBoxes = !isLock;
            if (!isLock)
                Text = FormName;
        }

        private void PlannerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.NsrDataAdapter.SevePlan(this);
        }
    }
}
