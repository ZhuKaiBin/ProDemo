using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Behaviours
{

    public class PerformanceBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest:notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            IUser user,
            IIdentityService identityService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _user = user;
            _identityService = identityService;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 5000)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _user.Id ?? string.Empty;
                var userName = string.Empty;

                
                if (!string.IsNullOrEmpty(userId))
                {
                    userName = await _identityService.GetUserNameAsync(userId);
                }

                _logger.LogWarning("CleanArchitectureSolutionTemplate Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);

            }

            return response;

        }


    }
}
