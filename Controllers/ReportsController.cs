using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace DemoUploadReport.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{

    [HttpGet("file")]
    public IActionResult GerarRelatorio()
    {
        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Nome Aba");

        worksheet.Cell(1,1).Value = "Coluna 1";
        worksheet.Cell(1, 2).Value = "Coluna 2";
    
        using MemoryStream memory = new();
        workbook.SaveAs(memory);
        memory.Position = 0;
        
        return File(memory.ToArray(), 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"Teste-{DateTime.Now.Ticks}.xlsx");
    }
    
    [HttpGet("blob")]
    public async Task<IActionResult> UploadRelatorio()
    {
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=demouploadfan;AccountKey=Pk/WvelxIcmvwcJ9UfiiXWkDBuvUOwEdsYao2OHEqzOUp68IJSLRW5HvEaj6OAD19PUPivHjkQuV+AStWGZhDQ==;EndpointSuffix=core.windows.net";
        var blobContainerName = "relatorios";

        var container = new BlobContainerClient(connectionString, blobContainerName);
        var client = container.GetBlockBlobClient($"relatorio-{DateTime.Now.Ticks}.xlsx");

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Nome Aba");

            worksheet.Cell(1, 1).Value = "Coluna 1";
            worksheet.Cell(1, 2).Value = "Coluna 2";

            using MemoryStream memory = new();
            workbook.SaveAs(memory);
            memory.Position = 0;

            await client.UploadAsync(memory);
        }

        return Ok();
    }
}