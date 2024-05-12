using CrossCutting.Core.Contract.Configuration.DataClasses;

namespace Logic.Business.MwvManagement
{
    public class MwvManagementConfiguration
    {
        [ConfigMap("CommandLine", new[] { "h", "help" })]
        public virtual bool ShowHelp { get; set; }

        [ConfigMap("CommandLine", new[] { "o", "operation" })]
        public virtual string Operation { get; set; }

        [ConfigMap("CommandLine", new[] { "f", "file" })]
        public virtual string FilePath { get; set; }
    }
}