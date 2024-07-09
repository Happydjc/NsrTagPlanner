using ExcelDataReader;
using Newtonsoft.Json;
using Nsr.Component;
using Nsr.Exclusion;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Nsr.Planner
{
    /// <summary>
    /// NSR数据适配器
    /// </summary>
    /// <param name="JsonPath">Json数据目录</param>
    /// <remarks>
    /// 将NSR数据的存储从Excel文件迁移到JSON文件，以便更方便地在程序中进行处理和管理。
    /// </remarks>
    internal record NsrDataAdapter(string JsonPath, string ExcelPath)
    {
        /// <summary>
        /// 获取NSR数据
        /// </summary>
        /// <returns>
        /// NSR数据
        /// </returns>
        /// <remarks>
        /// 检查一个JSON文件是否存在且未被修改（通过IsExcelFileChanged方法），
        ///     如果文件未被修改，直接从JSON文件中反序列化数据并返回。
        ///     如果文件被修改或不存在，从Excel文件中读取数据（通过ReadExcel方法），然后将这些数据序列化到JSON文件中，并返回这些数据。
        /// </remarks>
        internal NsrData GetData()
        {
            FileInfo file = new(Path.Combine(new FileInfo(JsonPath).DirectoryName, $"{nameof(NsrTag)}List.json"));
            if (!IsExcelFileChanged(file))
            {
                using StreamReader reader = new(file.FullName);
                return JsonConvert.DeserializeObject<NsrData>(reader.ReadToEnd(), serializerSettings);
            }
            else
            {
                NsrData nsrData = ReadExcel();
                using StreamWriter writer = new(file.FullName);
                writer.Write(JsonConvert.SerializeObject(nsrData, serializerSettings));
                return nsrData;
            }
        }

        /// <summary>
        /// 保存NSR标签状态
        /// </summary>
        /// <param name="tagStatus">NSR标签状态</param>
        internal void SevePlan(NsrPlanProto tagStatus)
        {
            FileInfo file = GetPlanFile(tagStatus.Name);
            tagStatus.RefreshPlan();
            using StreamWriter writer = new(file.FullName);
            writer.Write(JsonConvert.SerializeObject(tagStatus.TagSettings, serializerSettings));
        }

        /// <summary>
        /// 读取标签状态
        /// </summary>
        /// <param name="tagStatus">NSR标签状态</param>
        internal void LoadPlan(NsrPlanProto tagStatus)
        {
            FileInfo file = GetPlanFile(tagStatus.Name);
            if (file.Exists)
            {
                using StreamReader reader = new(file.FullName);
                List<NsrTagSetting> nsrPlanSettings = JsonConvert.DeserializeObject<List<NsrTagSetting>>(reader.ReadToEnd(), serializerSettings);
                nsrPlanSettings ??= new();
                tagStatus.SetPlanSettings(nsrPlanSettings);
            }
        }


        FileInfo GetPlanFile(string formName) => new(Path.Combine(new FileInfo(JsonPath).DirectoryName, $"{formName}_{nameof(NsrTagSetting)}List.json"));

        static readonly JsonSerializerSettings serializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        bool IsExcelFileChanged(FileInfo file) => !file.Exists || file.Length == 0 || (new FileInfo(ExcelPath).LastWriteTime > file.LastWriteTime);

        NsrData ReadExcel()
        {
            using FileStream stream = new(ExcelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            DataSet data = ExcelReaderFactory.CreateReader(stream).AsDataSet();
            NsrTags tags = ReadTags(data.Tables["标签"]);
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

        static NsrTags ReadTags(DataTable dataTable)
        {
            NsrTags tags = new();

            foreach (DataRow row in dataTable.Rows)
                if (int.TryParse(row.ItemArray[10].ToString(), out _))
                {
                    _ = int.TryParse(row.ItemArray[1].ToString(), out int rarityId);
                    _ = bool.TryParse(row.ItemArray[3].ToString(), out bool isSensitive);
                    _ = int.TryParse(row.ItemArray[9].ToString(), out int repeatTime);
                    NsrTagRarity rarity = (NsrTagRarity)rarityId;
                    NsrTag tag = new()
                    {
                        Name = row.ItemArray[0].ToString(),
                        Rarity = rarity,
                        SubStribe = rarity.SubStribe(),
                        Description = row.ItemArray[8].ToString(),
                        RepeatTime = repeatTime,
                        IsSensitive = isSensitive,
                    };
                    tag.Exclusions.Add(new NsrRepeatExclusion(tag.Name, tag.RepeatTime));
                    tag.Exclusions.AddRange(descExclusions.Where(ex => ex.Match(tag.Description)));
                    string exclusionNames = row.ItemArray[25].ToString();
                    if (!string.IsNullOrEmpty(exclusionNames))
                        tag.Exclusions.Add(new NsrTagExclusion(exclusionNames));
                    const int offset = 11;
                    for (int i = offset; i <= offset + 12; i++)
                        if (int.TryParse(row.ItemArray[i].ToString(), out int value) && value == 2)
                            tag.Groups.Add((NsrTagCategory)(i - offset));
                    tags.Add(tag);
                }
            FillTagExclusions(tags);
            return tags;
        }

        static void FillTagExclusions(NsrTags tags)
        {
            foreach (NsrTag tag in tags)
            {
                foreach (string tagName in tag.TagExclusion)
                {
                    NsrTag nsrTag = tags.First(firstTag => firstTag.Name == tagName);
                    if (!nsrTag.TagExclusion.Contains(tag.Name))
                        nsrTag.TagExclusion.Add(tag.Name);
                }
            }
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
