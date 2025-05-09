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
    private readonly IEmailService _emailService;

    public RequestController(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    [HttpPost("AddRequest")]
    public async Task<IActionResult> AddRequest(string lang, [FromBody] CreateRequestHeaderDto request)
    {
        if (request == null)
        {
            return BadRequest(new { message =  lang == "EN" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });
        }
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        var openingStatus = await _unitOfWork.Status.FindAsync(s => s.NameEn == "Open");
        if (openingStatus == null)
        {
            return BadRequest(new { message = lang == "EN" ? "Opening status not found." : "لم يتم العثور على حالة فتح الطلب" });
        }
        var newRequest = new RequestHeader
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Code = Helpers.GenerateRequestCode(),
            HotelId = request.HotelId,
            RoomId = request.RoomId,
            Note = request.Note,
            SpecialRequest = request.SpecialRequest,
            StatusId = openingStatus.Id
        };

        await _unitOfWork.RequestHeader.AddAsync(newRequest);
        await _unitOfWork.SaveChangesAsync();

        if (request.ServiceIds != null && request.ServiceIds.Any())
        {
            foreach (var serviceId in request.ServiceIds)
            {
                var newDetail = new RequestDetails
                {
                    RequestHeaderId = newRequest.Id,
                    StatusId = openingStatus.Id,
                    ServiceId = serviceId
                };
                await _unitOfWork.RequestDetails.AddAsync(newDetail);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        return Ok(new { message = lang == "EN" ? "Request Created Successfully" : "تم إنشاء الطلب بنجاح" });
    }


    //string requestDetailsUrl = $"https://yourfrontenddomain.com/request-details.html?id={newRequest.Id}";
    //await _emailService.SendAsync(new EmailDto
    //{
    //    To = newRequest.CustEmail,
    //    Subject = lang == "EN" ? "Your Request Has Been Created" : "تم إنشاء طلبك",
    //    Body = lang == "EN"
    //        ? $"Thank you for your request. You can view the details here: <a href='{requestDetailsUrl}'>View Request</a>"
    //        : $"شكراً لطلبك. يمكنك عرض تفاصيل الطلب من هنا: <a href='{requestDetailsUrl}'>عرض الطلب</a>",
    //    IsHtml = true
    //});

    [HttpGet("GetUserRequestDetails{id}")]
    public async Task<IActionResult> GetUserRequestDetails(string lang, int id)
    {
        var request = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id, ["Hotel", "Room", "Status", "RequestDetails.Service", "RequestDetails.Status"]);
        if (request == null)
        {
            return NotFound(new { message = lang == "EN" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        var requestDetails = new UserRequestHeaderDto
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Reply = request.Reply,
            SpecialRequest = request.SpecialRequest,
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
    public async Task<IActionResult> AddReviewToRequest(string lang, [FromBody] UpdateRequestDto review)
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
        existingRequest.Review = review.Text;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new { message = lang == "EN" ? "Review Added Successfully" : "تم إضافة المراجعة بنجاح" });
    }

    [HttpGet("GetAllRequests")]
    public async Task<IActionResult> GetAllRequests(string lang)
    {
        var requests = (await _unitOfWork.RequestHeader.FindAllAsync(includes: ["Status"])).OrderBy(r => r.UpdatedAt).ThenBy(r => r.CreatedAt);
        if (requests == null || !requests.Any())
        {
            return NotFound(new { message = lang == "EN" ? "Request not found" : "هذا الطلب غير موجود" });
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
            return NotFound(new { message = lang == "EN" ? "Request not found" : "هذا الطلب غير موجود" });
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
    public async Task<IActionResult> AddReplyToRequest(string lang, [FromBody] UpdateRequestDto reply)
    {
        if (reply == null)
        {
            return BadRequest(new { message = lang == "EN" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });
        }
        var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == reply.Id);
        if (existingRequest == null)
        {
            return NotFound(new { message = lang == "EN" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        existingRequest.Reply = reply.Text;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new { message = lang == "EN" ? "Reply Added Successfully" : "تم إضافة الرد بنجاح" });
    }
}
