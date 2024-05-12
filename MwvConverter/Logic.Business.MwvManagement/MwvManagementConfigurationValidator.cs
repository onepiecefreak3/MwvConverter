using Logic.Business.MwvManagement.InternalContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Business.MwvManagement
{
    internal class MwvManagementConfigurationValidator : IMwvManagementConfigurationValidator
    {
        public void Validate(MwvManagementConfiguration config)
        {
            if (config.ShowHelp)
                return;

            ValidateOperation(config);
            ValidateFilePath(config);
        }

        private void ValidateOperation(MwvManagementConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.Operation))
                throw new InvalidOperationException("No operation mode was given. Specify an operation mode by using the -o argument.");

            if (config.Operation != "d" && config.Operation != "e")
                throw new InvalidOperationException($"The operation mode '{config.Operation}' is not valid. Use -h to see a list of valid operation modes.");
        }

        private void ValidateFilePath(MwvManagementConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.FilePath))
                throw new InvalidOperationException("No file to process was specified. Specify a file by using the -f argument.");

            if (!File.Exists(config.FilePath) && !Directory.Exists(config.FilePath))
                throw new InvalidOperationException($"File or directory '{Path.GetFullPath(config.FilePath)}' was not found.");
        }
    }
}
