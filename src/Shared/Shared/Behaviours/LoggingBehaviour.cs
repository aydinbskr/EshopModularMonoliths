﻿using MediatR;
using Microsoft.Extensions.Logging;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Behaviours
{
	public class LoggingBehaviour<TRequest, TResponse>
		(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
		: IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull, IRequest<TResponse>
		where TResponse: notnull
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{

			logger.LogInformation("[START] Handle request={Request} - Response ={Response} - RequestData ={ RequestData}",
				typeof(TRequest).Name, typeof(TResponse).Name, request);


			var timer = new Stopwatch(); 
			timer.Start();

			var response = await next();

			timer.Stop();
			var timeTaken = timer.Elapsed;
			if (timeTaken.Seconds > 3) // if the request is greater than 3 seconds, then lo
				logger.LogWarning("[PERFORMANCE] The request {Request} took {Time Taken} seconds",
					typeof(TRequest).Name, timeTaken.Seconds);

			logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
			return response;
		}
	}
}
