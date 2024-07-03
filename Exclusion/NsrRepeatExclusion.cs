using NsrModels;
using System.Linq;

namespace NsrTagPlanner.Exclusion
{
    /// <summary>
    /// 同名标签排斥规则
    /// </summary>
    /// <param name="Name">标签名</param>
    /// <param name="RepeatTime">重复次数</param>
    internal record NsrRepeatExclusion(string Name, int RepeatTime) : INsrExclusion
    {

        public string ExclusionMassage(NsrTag _) => RepeatTime == 0 ? $"标签[{Name}]不得重复出现!" : $"标签[{Name}]不得重复超过{RepeatTime}次!";

        public bool Match(NsrTags tags) => tags != null && tags.Count(t => t.Name == Name) > RepeatTime;
    }
}