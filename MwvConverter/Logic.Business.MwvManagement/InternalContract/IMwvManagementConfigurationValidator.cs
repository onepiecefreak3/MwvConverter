using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Core.Contract.Aspects;
using Logic.Business.MwvManagement.InternalContract.Exceptions;

namespace Logic.Business.MwvManagement.InternalContract
{
    [MapException(typeof(MwvManagementConfigurationValidatorException), "Error in config validator.")]
    public interface IMwvManagementConfigurationValidator
    {
        void Validate(MwvManagementConfiguration config);
    }
}
