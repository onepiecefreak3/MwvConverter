using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Business.MwvManagement.InternalContract.Exceptions
{
    [Serializable]
    public class MwvManagementConfigurationValidatorException:Exception
    {
        public MwvManagementConfigurationValidatorException()
        {
        }

        public MwvManagementConfigurationValidatorException(string message) : base(message)
        {
        }

        public MwvManagementConfigurationValidatorException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MwvManagementConfigurationValidatorException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
