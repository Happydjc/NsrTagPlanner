namespace NsrTagPlanner
{
    internal record NsrPlanListSetting(string TagName)
    {
        internal bool IsLocked { get; set; }
    }
}
