﻿using NsrModels;
using System.Linq;

namespace NsrTagPlanner
{
    /// <summary>
    /// 标签前后缀限制
    /// </summary>
    /// <param name="StartWith">前缀限制</param>
    /// <param name="EndWith">后缀限制</param>
    internal record NsrTagDescMutualExclusion(string StartWith, string EndWith) : INsrTagExclusion
    {
        public string ExclusionMassage(NsrTag tag) => $"标签[{tag.Name}]不得在形如{this}中重复出现!";

        internal bool Match(string s) =>
            s is not null && s.StartsWith(StartWith) && s.EndsWith(EndWith) && s != $"{StartWith}一{EndWith}";

        public bool Match(NsrTagList tags) => tags != null && tags.Any(tag => Match(tag.Description));

        public override string ToString() => $"{StartWith}...{EndWith}";
    }
}
