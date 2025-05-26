using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalar2Sln_Application.Common.Behaviours
{
    //IRequestPreProcessor:用于在 请求被处理前做一些前置操作，就像是请求（Request）即将进入处理器（Handler）之前，你插入了一个“预处理钩子”。
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger;
        private readonly IUser _user;
        private readonly IIdentityService _identityService;


        public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
        {
            _logger = logger;
            _user = user;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            //id不是空的时候，去查找Name
            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("CleanArchitectureSolutionTemplate Request: {Name} {@UserId} {@UserName} {@Request}",
           requestName, userId, userName, request);
        }
    }
}
