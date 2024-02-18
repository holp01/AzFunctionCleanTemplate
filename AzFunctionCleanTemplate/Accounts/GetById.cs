using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AzFunctionCleanTemplate.Application.Accounts;
using AzFunctionCleanTemplate.Application.Accounts.Dtos;
using AzFunctionCleanTemplate.Application.Interfaces;
using AzFunctionCleanTemplate.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AzFunctionCleanTemplate.Function.Accounts
{
    public class GetById
    {
        private readonly ILogger<GetById> _logger;
        private readonly IAccountAppService _accountAppService;

        public GetById(ILogger<GetById> log,
            IAccountAppService accountAppService)
        {
            _logger = log;
            _accountAppService = accountAppService;
        }

        [FunctionName("GetById")]
        [OpenApiOperation(operationId: "GetById", tags: new[] { "id" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(AccountDto), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Account/GetById")] HttpRequest req,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string idString = req.Query["id"];

            if (!Guid.TryParse(idString, out Guid id))
            {
                // Return a bad request response if the GUID is not valid
                return new BadRequestObjectResult("Invalid GUID format.");
            }

            try
            {
                var account = await _accountAppService.GetAccountById(id, cancellationToken);
                if (account == null)
                {
                    return new NotFoundObjectResult($"Account with ID {id} not found.");
                }

                return new OkObjectResult(account);
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation scenario
                _logger.LogWarning("Request was cancelled.");
                return new StatusCodeResult(408); //Timeout Error
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return new StatusCodeResult(500); //Internal Server Error
            }
        }
    }
}

