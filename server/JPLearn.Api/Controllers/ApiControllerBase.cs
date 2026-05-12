using JPLearn.Core.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace JPLearn.Api.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid CurrentUserId => HttpContext.RequestServices
        .GetRequiredService<ICurrentUserContext>()
        .UserId;
}
