using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Behaviours
{

    //这几个的Behaviour的行为都是一样的，都是在这个MediR中做管道处理,所以入参和出参都是一致的

    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                                .Where(r => r.Errors.Any())
                                .SelectMany(r => r.Errors)
                                .ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }

            //如果验证通过，则继续执行下一个管道处理器
            return await next();

        }

    }
}
