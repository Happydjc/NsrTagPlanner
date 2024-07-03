using NsrModels;
using System;
using System.Linq;

namespace NsrTagPlanner.Exclusion
{
    internal class NsrComponentExclusion : INsrExclusion
    {
        /// <summary>
        /// 标签限制（未定义）
        /// </summary>
        /// <param name="tag">标签</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ExclusionMassage(NsrTag tag) => throw new NotImplementedException();

        public bool Match(NsrTags tags) => tags != null && tags.ExcludeComponents.Except(tags.IncludeComponents).Any();
    }
}
