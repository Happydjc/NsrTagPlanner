using ExcelDataReader;
using Newtonsoft.Json;
using NsrModels;
using NsrTagPlanner.Exclusion;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace NsrTagPlanner
{
    public record NsrDataAdapter(string DataPath):INsrDataAdapter<NsrData>
    {
        static readonly JsonSerializerSettings serializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
        public NsrData GetData()
        {
            FileInfo file = new(Path.Combine(new FileInfo(DataPath).DirectoryName, $"{nameof(NsrTag)}List.json"));
            string excelPath = Properties.Resources.ExcelPath;
            if (!IsExcelFileChanged(file, excelPath))
            {
                using StreamReader reader = new(file.FullName);
                return JsonConvert.DeserializeObject<NsrData>(reader.ReadToEnd(),serializerSettings);
            }
            else
            {               
                NsrData nsrData = ReadExcel(excelPath);
                using StreamWriter writer = new(file.FullName);
                writer.Write(JsonConvert.SerializeObject(nsrData,serializerSettings));
                return nsrData;
            }
        }

        public void SevePlan(PlannerWindow plannerWindow)
        {
            FileInfo file = GetPlanFile(plannerWindow.Name);
            plannerWindow.RefreshPlan();
            using StreamWriter writer = new(file.FullName);
            writer.Write(JsonConvert.SerializeObject(plannerWindow.PlanSettings, serializerSettings));
        }

        public void LoadPlan(PlannerWindow plannerWindow)
        {
            FileInfo file = GetPlanFile(plannerWindow.Name);
            if (file.Exists)
            {
                using StreamReader reader = new(file.FullName);
                List<NsrTagSetting> nsrPlanTagSettings = JsonConvert.DeserializeObject<List<NsrTagSetting>>(reader.ReadToEnd(), serializerSettings);
                plannerWindow.SetPlanSettings(nsrPlanTagSettings);
            }
        }
        FileInfo GetPlanFile(string formName) => new(Path.Combine(new FileInfo(DataPath).DirectoryName, $"{formName}_{nameof(NsrTagSetting)}List.json"));

        public static string ShowNsrTagOperateList(NsrTagChoices tags)
        {
            StringBuilder sb = new();
            tags.ForEach(tag => sb.Append($"{tag.Name}({tag.SubStribe})+"));
            if (tags.Count > 0) sb.Remove(sb.Length - 1, 1);
            if (tags.Count > 1) sb.Append($"={tags.BaseSubstribe}");
            if (tags.Multiple > 1)
            {
                sb.AppendLine();
                sb.Append(tags.BaseSubstribe);
                foreach (var item in tags.TagGroupCountDict)
                    sb.Append(NsrTagChoices.GetCountItemText(item));
                sb.Append($"={tags.Substribe}");
            }
            return sb.ToString();
        }

        static bool IsExcelFileChanged(FileInfo file, string excelPath) => !file.Exists || file.Length == 0 || (new FileInfo(excelPath).LastWriteTime > file.LastWriteTime);

        static NsrData ReadExcel(string excelPath)
        {
            //using FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using FileStream stream = new(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            DataSet data = ExcelReaderFactory.CreateReader(stream).AsDataSet();
            List<NsrTag> tags = ReadTags(data.Tables["标签"]);
            List<NsrComponent> components = new();
            components.AddRange(ReadStructureComponents(data.Tables["结构组件"]));
            components.AddRange(ReadPropulsionComponents(data.Tables["推进组件"]));
            components.AddRange(ReadActiveComponents(data.Tables["能动组件"]));
            return new NsrData(tags, components);

        }

        static List<NsrStructureComponent> ReadStructureComponents(DataTable dataTable)
        {
            List<NsrStructureComponent> components = new();
            foreach (DataRow row in dataTable.Rows)
                if (int.TryParse(row.ItemArray[2].ToString(), out _))
                {
                    _ = int.TryParse(row.ItemArray[2].ToString(), out int complexity);
                    _ = int.TryParse(row.ItemArray[3].ToString(), out int weight);
                    _ = int.TryParse(row.ItemArray[4].ToString(), out int dragPerTen);
                    _ = int.TryParse(row.ItemArray[5].ToString(), out int solidness);
                    NsrStructureComponent component = new()
                    {
                        ComponentType = row.ItemArray[0].ToString(),
                        Name = row.ItemArray[1].ToString(),
                        Complexity = complexity,
                        Weight = weight,
                        DragPerTen = dragPerTen,
                        Solidness = solidness,
                    };
                    for (int i = 6; i <= 9; i++)
                        if (!string.IsNullOrEmpty(row.ItemArray[i].ToString()))
                            component.Tags.Add(row.ItemArray[i].ToString());
                    components.Add(component);
                }
            return components;
        }

        static List<NsrPropulsionComponent> ReadPropulsionComponents(DataTable dataTable)
        {
            List<NsrPropulsionComponent> components = new();
            foreach (DataRow row in dataTable.Rows)
                if (int.TryParse(row.ItemArray[2].ToString(), out _))
                {
                    _ = int.TryParse(row.ItemArray[2].ToString(), out int complexity);
                    _ = int.TryParse(row.ItemArray[3].ToString(), out int weight);
                    _ = int.TryParse(row.ItemArray[4].ToString(), out int dragPerTen);
                    _ = int.TryParse(row.ItemArray[5].ToString(), out int strength);
                    _ = int.TryParse(row.ItemArray[6].ToString(), out int fuel);
                    _ = int.TryParse(row.ItemArray[7].ToString(), out int fuelSpeed);
                    _ = int.TryParse(row.ItemArray[8].ToString(), out int efficiency);
                    _ = int.TryParse(row.ItemArray[9].ToString(), out int workingSeconds);

                    NsrPropulsionComponent component = new()
                    {
                        ComponentType = row.ItemArray[0].ToString(),
                        Name = row.ItemArray[1].ToString(),
                        Complexity = complexity,
                        Weight = weight,
                        DragPerTen = dragPerTen,
                        Strength = strength,
                        Fuel = fuel,
                        FuelSpeed = fuelSpeed,
                        Efficiency = efficiency,
                        WorkingSeconds = workingSeconds,
                    };

                    if (!string.IsNullOrEmpty(row.ItemArray[10].ToString()))
                        component.Tags.Add(row.ItemArray[10].ToString());
                    components.Add(component);
                }
            return components;
        }

        static List<NsrActiveComponent> ReadActiveComponents(DataTable dataTable)
        {
            List<NsrActiveComponent> components = new();
            foreach (DataRow row in dataTable.Rows)
                if (int.TryParse(row.ItemArray[2].ToString(), out _))
                {
                    _ = int.TryParse(row.ItemArray[2].ToString(), out int complexity);
                    _ = int.TryParse(row.ItemArray[3].ToString(), out int weight);
                    _ = int.TryParse(row.ItemArray[4].ToString(), out int dragPerTen);
                    NsrActiveComponent component = new()
                    {
                        ComponentType = row.ItemArray[0].ToString(),
                        Name = row.ItemArray[1].ToString(),
                        Complexity = complexity,
                        Weight = weight,
                        DragPerTen = dragPerTen,
                    };
                    for (int i = 5; i <= 9; i++)
                        if (!string.IsNullOrEmpty(row.ItemArray[i].ToString()))
                            component.Tags.Add(row.ItemArray[i].ToString());
                    components.Add(component);
                }
            return components;
        }

        static List<NsrTag> ReadTags(DataTable dataTable)
        {
            List<NsrTag> tags = new();
            foreach (DataRow row in dataTable.Rows)
                if (int.TryParse(row.ItemArray[9].ToString(), out _))
                {
                    _ = int.TryParse(row.ItemArray[1].ToString(), out int rarityId);
                    _ = bool.TryParse(row.ItemArray[3].ToString(), out bool isSensitive);
                    _ = int.TryParse(row.ItemArray[8].ToString(), out int repeatTime);
                    NsrTag tag = new()
                    {
                        Name = row.ItemArray[0].ToString(),
                        Rarity = (NsrTagRarity)rarityId,
                        SubStribe = (NsrTagRarity)rarityId switch
                        {
                            NsrTagRarity.普通 => 5,
                            NsrTagRarity.罕见 => 15,
                            NsrTagRarity.稀有 => 45,
                            NsrTagRarity.史诗 => 135,
                            NsrTagRarity.疯传 => 405,
                            _ => 0,
                        },
                        Description = row.ItemArray[7].ToString(),
                        RepeatTime = repeatTime,
                        IsSensitive = isSensitive,
                    };
                    tag.Exclusions.Add(new NsrRepeatExclusion(tag.Name, tag.RepeatTime));
                    tag.Exclusions.AddRange(descExclusions.Where(ex => ex.Match(row.ItemArray[7].ToString())));
                    string exclusionNames = row.ItemArray[24].ToString();
                    if (!string.IsNullOrEmpty(exclusionNames))
                        tag.Exclusions.Add(new NsrTagExclusion(exclusionNames));
                    const int offset = 10;
                    for (int i = offset; i <= offset+12; i++)
                        if (int.TryParse(row.ItemArray[i].ToString(), out int value) && value == 2)
                            tag.Groups.Add((NsrTagGroup)(i - offset));
                    tags.Add(tag);
                }
            return tags;
        }

        static readonly List<NsrDescMutualExclusion> descExclusions = new()
        {
            new("完成“", "”"),
            new("完成来自", "的一项挑战"),
            new("仅使用", "结构套组"),
            new("最高高度为", "米"),
            new("达到", "的距离"),
            new("时速", "km"),
            new("到达", "的高度"),
            new("拥有一个", "对称的火箭"),
            new("拥有一个宽度", "的火箭"),
            new("使用", "个结构部件"),
            new("使用", "一个头锥、一个主体、一个尾部"),
            new("使用", "个结构套组"),
            new("使用", "一整套结构套组"),
            new("使用", "个泰迪熊"),
            new("使用", "个大象"),
            new("使用", "个足球"),
            new("使用", "个不同的垃圾部件"),
            new("使用", "个尾部件"),
            new("使用", "个助推器"),
            new("使用", "个引擎"),
            new("使用", "个燃料缸"),
            new("使用", "个燃料缸"),
            new("使用", "台泵"),
            new("使用", "个躯干部件"),
            new("使用", "个腿部件"),
            new("使用", "个肢体"),
            new("使用", "个球"),
            new("在一个", "个圆形部件"),
            new("翻", "个跟头"),
            new("在一个", "个圆形部件"),
            new("在没有推进器或爆炸物的情况下达到", "米的距离"),
        };
    }
}
