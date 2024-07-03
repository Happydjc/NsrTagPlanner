using System.Collections.Generic;
using NsrModels;

namespace NsrTagPlanner
{
    public record NsrData(List<NsrTag> NsrTags, List<NsrComponent> NsrComponents);
}
