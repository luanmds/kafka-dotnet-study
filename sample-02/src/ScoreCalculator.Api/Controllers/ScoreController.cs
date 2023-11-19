using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreCalculator.Domain.MessageBus;
using ScoreCalculator.Domain.Model.Commands;
using ScoreCalculator.Domain.Model.Entities;

namespace ScoreCalculator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly KafkaPublisherMessage _publisherMessage;

    public ScoreController(KafkaPublisherMessage publisherMessage)
    {
        _publisherMessage = publisherMessage;
    }

    [HttpPost]
    public async Task<ActionResult> StartCalculate()
    {
        var command = GetCommand(new CalculateProcess());

        await _publisherMessage.Publish(command, default);

        return Ok(command.CalculateProcess.Id);
    }

    [HttpPost("cancel")]
    public async Task<ActionResult> CancelCalculate(string processId)
    {
        var command = new CancelCalculateScore(processId, processId);

        await _publisherMessage.Publish(command, default);

        return Ok("Cancellation of the process has been initialize");
    }

    private StartCalculateProcess GetCommand(CalculateProcess data)
        => new(data, data.Id);

}
