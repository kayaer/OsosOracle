using FluentValidation;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using System;
using System.Text;

namespace OsosOracle.Framework.CrossCuttingConcern.Validation.FluentValidation
{
    public class ValidatorTool
    {
        public static void FluentValidate(IValidator validator, object entity)
        {
            var result = validator.Validate(entity);
            if (result.Errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                var ex = new Exception();



                //tüm validasyon hatalarını ekle 
                foreach (var error in result.Errors)
                {

                    sb.AppendLine(error.ErrorMessage);
                    ex.Data.Add(error.PropertyName, error.ErrorMessage);
                }

                if (sb.Length > 0)
                {
                    throw new ValidationCoreException(ex, sb.ToString());
                }
            }




        }
    }
}
