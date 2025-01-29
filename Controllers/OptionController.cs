using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bakery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionController : Controller
    {
        private readonly IMediator _mediator;
        public OptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("api/configure/edit/{country}/{language}/{segment}/{customerset}/{ordercode}")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditSelection.Command editSelection)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _mediator.Send(editSelection);
            return new JsonResult("");
        }
    }

    public class EditSelection
    {
        public class Command : IRequest<CommandResult>
        {
            public string Language { get; set; }
            public string Country { get; set; }
            public string CustomerSet { get; set; }
            public string Segment { get; set; }
            public string OrderCode { get; set; }
            [FromBody]
            public SelectionDetail Detail { get; set; }
            public class SelectionDetail
            {

            }
        }
        public class CommandResult
        {

        }
        public class Handler : IRequestHandler<Command, CommandResult>
        {
            public Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
