using Misa;
using System.Reflection;
namespace Near
{
    public class WorkflowAssignerData
    {
        public required string NamespaceToLoad { get; set; }

        public required Assembly AssemblyToLoad;

        private DateTime workflowDate;
        public DateTime WorkflowDate
        {
            get
            {
                if (workflowDate == DateTime.MinValue)
                    return DateTime.Now;

                return workflowDate;
            }
            set { workflowDate = value; }
        }

    }

    public static class WorkflowAssigner
    {
        public static Type? GetWorkflowObjectByDate(WorkflowAssignerData data)
        {
            var test = data.AssemblyToLoad.GetTypes()
                .Where(p => string.Equals(p.Namespace, data.NamespaceToLoad, StringComparison.Ordinal));

            return test
                .Select(t => new
                {
                    Type = t,
                    Attribut = t.GetCustomAttribute<WorkflowValidityAttribute>()
                })
                .Where(t => t.Attribut != null && DateTime.Parse(
                    t.Attribut.ValidFrom) <= data.WorkflowDate &&
                    DateTime.Parse(t.Attribut.ValidFrom).Year == data.WorkflowDate.Year)
                .Select(t => new
                {
                    t.Type,
                    WorkflowDate = DateTime.Parse(t.Attribut.ValidFrom),
                })
                .OrderBy(t => Math.Abs((data.WorkflowDate - t.WorkflowDate).Ticks))
                .Select(t => t.Type)
                .FirstOrDefault();
        }
    }
}
