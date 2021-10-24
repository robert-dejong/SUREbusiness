using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace SUREbusiness.FleetManagement.BLL.Models
{
    public class BaseResult<T>
    {
        public T Result { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool HasErrors => Errors != null && Errors.Any();

        public BaseResult(T result)
        {
            Result = result;
        }

        public BaseResult(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors.Select(error => error.ErrorMessage);
        }

        public BaseResult(string error)
        {
            Errors = new List<string>() { error };
        }
    }
}
