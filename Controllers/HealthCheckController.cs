using Microsoft.AspNetCore.Mvc;

namespace DemoUploadReport.Api.Controllers;

[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{

    [HttpGet("ping")]
    public string Ping() => "Pong";
}