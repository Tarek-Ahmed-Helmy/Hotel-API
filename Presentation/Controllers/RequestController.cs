using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Utilities;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestController : ControllerBase
{
    private readonly ILogger<BasicDataController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RequestController(ILogger<BasicDataController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("AddRequest")]
    public async Task<IActionResult> AddRequest(string lang, [FromBody] CreateRequestHeaderDto request)
    {
        if (request == null)
        {
            return BadRequest("Invalid request data.");
        }
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        var openingStatus = await _unitOfWork.Status.FindAsync(s => s.NameEn == "Opening");
        var newRequest = new RequestHeader
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Code = Helpers.GenerateRequestCode(),
            HotelId = request.HotelId,
            RoomId = request.RoomId,
            Note = request.Note,
            StatusId = openingStatus.Id
        };

        await _unitOfWork.RequestHeader.AddAsync(newRequest);
        await _unitOfWork.SaveChangesAsync();

        if (request.RequestDetails != null && request.RequestDetails.Any())
        {
            foreach (var detail in request.RequestDetails)
            {
                var newDetail = new RequestDetails
                {
                    RequestHeaderId = newRequest.Id,
                    StatusId = openingStatus.Id,
                    ServiceId = detail.ServiceId,
                    Note = detail.Note
                };
                await _unitOfWork.RequestDetails.AddAsync(newDetail);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        return Ok(lang == "EN" ? "Request Created Successfully" : "تم إنشاء الطلب بنجاح");
    }

    [HttpGet("GetUserRequestDetails{id}")]
    public async Task<IActionResult> GetUserRequestDetails(string lang, int id)
    {
        var request = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id, ["Hotel", "Room", "Status", "RequestDetails.Service", "RequestDetails.Status"]);
        if (request == null)
        {
            return NotFound();
        }
        var requestDetails = new UserRequestHeaderDto
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Reply = request.Reply,
            HotelName = request.Hotel.Name,
            RoomNumber = request.Room.Number,
            StatusName = lang == "EN" ? request.Status.NameEn : request.Status.NameAr,
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                StatusName = lang == "EN" ? rd.Status.NameEn : rd.Status.NameAr,
                ServiceName = lang == "EN" ? rd.Service.NameEn : rd.Service.NameAr,
            }).ToList()
        };
        return Ok(requestDetails);
    }

    [HttpPut("AddReviewToRequest{id}")]
    public async Task<IActionResult> AddReviewToRequest(string lang, int id, [FromBody] string review)
    {
        if (review == null)
        {
            return BadRequest("Invalid request data.");
        }
        var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id);
        if (existingRequest == null)
        {
            return NotFound();
        }
        existingRequest.Review = review;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(lang == "EN" ? "Review Added Successfully" : "تم إضافة المراجعة بنجاح");
    }

    [HttpGet("GetAllRequests")]
    public async Task<IActionResult> GetAllRequests(string lang)
    {
        var requests = (await _unitOfWork.RequestHeader.GetAllAsync()).OrderBy(r => r.UpdatedAt).ThenBy(r => r.CreatedAt);
        if (requests == null || !requests.Any())
        {
            return NotFound();
        }
        var requestDtos = requests.Select(r => new RequestHeaderDto
        {
            Id = r.Id,
            Code = r.Code,
            CustName = r.CustName,
            CustEmail = r.CustEmail,
            CustPhone = r.CustPhone,
            StatusName = lang == "EN" ? r.Status.NameEn : r.Status.NameAr
        }).ToList();
        return Ok(requestDtos);
    }

    [HttpGet("GetRequestDetails{id}")]
    public async Task<IActionResult> GetRequestDetails(string lang, int id)
    {
        var request = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id, ["Hotel", "Room", "Status", "RequestDetails.Service", "RequestDetails.Status"]);
        if (request == null)
        {
            return NotFound();
        }
        var requestDetails = new RequestHeaderDto
        {
            Id = request.Id,
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Code = request.Code,
            Note = request.Note,
            AttachmentPath = request.AttachmentPath,
            Reply = request.Reply,
            Review = request.Review,
            HotelName = request.Hotel.Name,
            RoomNumber = request.Room.Number,
            StatusName = lang == "EN" ? request.Status.NameEn : request.Status.NameAr,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                StatusName = lang == "EN" ? rd.Status.NameEn : rd.Status.NameAr,
                ServiceName = lang == "EN" ? rd.Service.NameEn : rd.Service.NameAr
            }).ToList()
        };
        return Ok(requestDetails);
    }
}
