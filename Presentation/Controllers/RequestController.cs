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
    private readonly IUnitOfWork _unitOfWork;

    public RequestController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("AddRequest")]
    public async Task<IActionResult> AddRequest(string lang, [FromBody] CreateRequestHeaderDto request)
    {
        if (request == null)
        {
            return BadRequest(lang == "EN" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة");
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
            return NotFound(lang == "EN" ? "Request not found" : "هذا الطلب غير موجود");
        }
        var requestDetails = new UserRequestHeaderDto
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Reply = request.Reply,
            HotelName = request.Hotel?.Name ?? "N/A",
            RoomNumber = request.Room?.Number ?? "N/A",
            StatusName = lang == "EN" ? request.Status?.NameEn ?? "N/A" : request.Status?.NameAr ?? "غير معرف",
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                StatusName = lang == "EN" ? rd.Status?.NameEn ?? "N/A" : rd.Status?.NameAr ?? "غير معرف",
                ServiceName = lang == "EN" ? rd.Service?.NameEn ?? "N/A" : rd.Service?.NameAr ?? "غير معرف",
            }).ToList()
        };
        return Ok(requestDetails);
    }

    [HttpPut("AddReviewToRequest")]
    public async Task<IActionResult> AddReviewToRequest(string lang, [FromBody] RequestReviewDto review)
    {
        if (review == null)
        {
            return BadRequest(new { message = lang == "EN" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });
        }
        var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == review.Id);
        if (existingRequest == null)
        {
            return NotFound(new { message = lang == "EN" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        if (existingRequest.Review != null)
        {
            return BadRequest(new { message = lang == "EN" ? "Review already exists." : "المراجعة موجودة بالفعل" });
        }
        existingRequest.Review = review.Review;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new { message = lang == "EN" ? "Review Added Successfully" : "تم إضافة المراجعة بنجاح" });
    }

    [HttpGet("GetAllRequests")]
    public async Task<IActionResult> GetAllRequests(string lang)
    {
        var requests = (await _unitOfWork.RequestHeader.GetAllAsync()).OrderBy(r => r.UpdatedAt).ThenBy(r => r.CreatedAt);
        if (requests == null || !requests.Any())
        {
            return NotFound(lang == "EN" ? "Request not found" : "هذا الطلب غير موجود");
        }
        var requestDtos = requests.Select(r => new RequestHeaderDto
        {
            Id = r.Id,
            Code = r.Code,
            CustName = r.CustName,
            CustEmail = r.CustEmail,
            CustPhone = r.CustPhone,
            StatusName = lang == "EN" ? r.Status?.NameEn ?? "N/A" : r.Status?.NameAr ?? "غير معرف"
        }).ToList();
        return Ok(requestDtos);
    }

    [HttpGet("GetRequestDetails{id}")]
    public async Task<IActionResult> GetRequestDetails(string lang, int id)
    {
        var request = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id, ["Hotel", "Room", "Status", "RequestDetails.Service", "RequestDetails.Status"]);
        if (request == null)
        {
            return NotFound(lang == "EN" ? "Request not found" : "هذا الطلب غير موجود");
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
            HotelName = request.Hotel?.Name ?? "N/A",
            RoomNumber = request.Room?.Number ?? "N/A",
            StatusName = lang == "EN" ? request.Status?.NameEn ?? "N/A" : request.Status?.NameAr ?? "غير معرف",
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                StatusName = lang == "EN" ? rd.Status?.NameEn ?? "N/A" : rd.Status?.NameAr ?? "غير معرف",
                ServiceName = lang == "EN" ? rd.Service?.NameEn ?? "N/A" : rd.Service?.NameAr ?? "غير معرف"
            }).ToList()
        };
        return Ok(requestDetails);
    }

    [HttpPut("AddReplyToRequest{id}")]
    public async Task<IActionResult> AddReplyToRequest(string lang, int id, [FromBody] string reply)
    {
        if (reply == null)
        {
            return BadRequest(lang == "EN" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة");
        }
        var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id);
        if (existingRequest == null)
        {
            return NotFound(lang == "EN" ? "Request not found" : "هذا الطلب غير موجود");
        }
        existingRequest.Reply = reply;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(lang == "EN" ? "Reply Added Successfully" : "تم إضافة الرد بنجاح");
    }
}
