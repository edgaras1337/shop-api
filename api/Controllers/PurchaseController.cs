using api.CustomExceptions;
using api.Dtos.PurchaseControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/purchase")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<CreatePurchaseResponse>> CreatePurchase(CreatePurchaseRequest request)
        {
            try
            {
                var res = await _purchaseService.CreatePurchaseAsync(request);

                return Ok(res);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<ActionResult<List<GetCurrentUserPurchaseHistory>>> GetCurrentUserPurchaseHistory()
        {
            try
            {
                var res = await _purchaseService.GetCurrentUserPurchaseHistoryAsync();

                return Ok(res);
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{userId}/history")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<GetCurrentUserPurchaseHistory>>> GetUserPurchaseHistory(int userId)
        {
            try
            {
                var res = await _purchaseService.GetUserPurchaseHistoryAsync(userId);

                return Ok(res);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("all/history")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<GetAllPurchaseHistoryResponse>>> GetAllPurchaseHistory()
        {
            try
            {
                var res = await _purchaseService.GetAllPurchaseHistoryAsync();

                return Ok(res);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
