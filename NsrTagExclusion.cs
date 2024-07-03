using NsrModels;
using System;
using System.Linq;

namespace NsrTagPlanner
{
    /// <summary>
    /// 标签指定互斥
    /// </summary>
    /// <param name="ExclusionNames">互斥的名字列表</param>
    /// <remarks>
    /// </remarks>
    internal record NsrTagExclusion(string[] ExclusionNames) : INsrTagExclusion
    {
        internal NsrTagExclusion(string TagNames) : this(TagNames.Split(",")) { }

        public string ExclusionMassage(NsrTag tag) => $"标签[{tag.Name}]和{string.Join(',', ExclusionNames)}冲突!";

        public bool Match(NsrTagList tags)=> tags != null && tags.Any(tag => ExclusionNames.Contains(tag.Name));

        public override string ToString() => string.Join(',', ExclusionNames);
    }
}
