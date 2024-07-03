using NsrModels;
using System;

namespace NsrTagPlanner.Exclusion
{

    /// <summary>
    /// 标签复杂度限制
    /// </summary>
    internal class NsrComplexityExclusion : INsrExclusion
    {
        public string ExclusionMassage(NsrTag tag) => throw new NotImplementedException();

        public bool Match(NsrTags tags) => tags != null && tags.Complexity > tags.ComplexityCap;
    }
}

