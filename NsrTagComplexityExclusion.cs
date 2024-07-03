using NsrModels;
using System;

namespace NsrTagPlanner
{

    /// <summary>
    /// 标签复杂度限制
    /// </summary>
    internal class NsrTagComplexityExclusion : INsrTagExclusion
    {
        public string ExclusionMassage(NsrTag tag) => throw new NotImplementedException();

        public bool Match(NsrTagList tags) => tags != null && tags.Complexity > tags.ComplexityCap;
    }
}

