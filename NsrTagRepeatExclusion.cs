using NsrModels;
using System.Linq;

namespace NsrTagPlanner
{
    /// <summary>
    /// 同名标签排斥规则
    /// </summary>
    /// <param name="Name">标签名</param>
    /// <param name="RepeatTime">重复次数</param>
    internal record NsrTagRepeatExclusion(string Name, int RepeatTime) : INsrTagExclusion
    {

        public string ExclusionMassage(NsrTag _) => RepeatTime == 0 ? $"标签[{Name}]不得重复出现!" : $"标签[{Name}]不得重复超过{RepeatTime}次!";

        public bool Match(NsrTagList tags) => tags != null && tags.Count(t => t.Name == Name) > RepeatTime;
    }
}