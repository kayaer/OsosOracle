using FluentValidation;
using OsosOracle.Framework.CrossCuttingConcern.Validation.FluentValidation;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OsosOracle.Framework.Aspects.ValidationAspects
{
    [Serializable]

    public sealed class FluentValidationAspect : OnMethodBoundaryAspect
    {
        private readonly Type _validatorType;

        public FluentValidationAspect(Type validatorType)
        {
            _validatorType = validatorType;
            base.AspectPriority = 1;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType, Type.EmptyTypes);

            Type entityType;
            if (_validatorType.BaseType != null)
                entityType = _validatorType.BaseType.GetGenericArguments()[0];
            else
            {
                entityType = _validatorType.GetGenericArguments()[0];
            }

            var entities = args.Arguments.Where(t => t.GetType() == entityType);

            foreach (var entity in entities)
            {
                ValidatorTool.FluentValidate(validator, entity);
            }
        }
    }
}
