using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;

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
            Code = Guid.NewGuid().ToString(),
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

    //[HttpPut("UpdateRequest")]
    //public async Task<IActionResult> AddReviewRequest(string lang, int id, [FromBody] CreateRequestHeaderDto request)
    //{
    //    if (request == null)
    //    {
    //        return BadRequest("Invalid request data.");
    //    }
    //    var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id);
    //    if (existingRequest == null)
    //    {
    //        return NotFound();
    //    }
    //    existingRequest.CustName = request.CustName;
    //    existingRequest.CustEmail = request.CustEmail;
    //    existingRequest.CustPhone = request.CustPhone;
    //    existingRequest.HotelId = request.HotelId;
    //    existingRequest.RoomId = request.RoomId;
    //    existingRequest.Note = request.Note;
    //    _unitOfWork.RequestHeader.Update(existingRequest);
    //    await _unitOfWork.SaveChangesAsync();
    //    if (request.RequestDetails != null && request.RequestDetails.Any())
    //    {
    //        foreach (var detail in request.RequestDetails)
    //        {
    //            var newDetail = new RequestDetails
    //            {
    //                RequestHeaderId = existingRequest.Id,
    //                ServiceId = detail.ServiceId,
    //                Note = detail.Note
    //            };
    //            await _unitOfWork.RequestDetails.AddAsync(newDetail);
    //        }
    //        await _unitOfWork.SaveChangesAsync();
    //    }
    //    return Ok(lang == "EN" ? "Request Updated Successfully" : "تم تحديث الطلب بنجاح");
    //}
}
