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
            return BadRequest(new { Message =  lang.ToLower() == "en" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });
        }
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        var openingStatus = await _unitOfWork.Status.FindAsync(s => s.NameEn == "Open");
        if (openingStatus == null)
        {
            return BadRequest(new { Message = lang.ToLower() == "en" ? "Opening status not found." : "لم يتم العثور على حالة فتح الطلب" });
        }
        var newRequest = new RequestHeader
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Code = Helpers.GenerateRequestCode(),
            HotelId = request.HotelId,
            RoomId = request.RoomId,
            SpecialRequest = request.SpecialRequest,
            Note = request.Note,
            StatusId = openingStatus.Id
        };

        await _unitOfWork.RequestHeader.AddAsync(newRequest);
        await _unitOfWork.SaveChangesAsync();

        if (request.Services != null && request.Services.Any())
        {
            foreach (var service in request.Services)
            {
                var newDetail = new RequestDetails
                {
                    RequestHeaderId = newRequest.Id,
                    StatusId = openingStatus.Id,
                    ServiceId = service.ServiceId,
                    Note = service.Note,
                };
                await _unitOfWork.RequestDetails.AddAsync(newDetail);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        return Ok(new { Message = lang.ToLower() == "en" ? "Request Created Successfully" : "تم إنشاء الطلب بنجاح" });
    }


    //string requestDetailsUrl = $"https://yourfrontenddomain.com/request-details.html?id={newRequest.Id}";
    //await _emailService.SendAsync(new EmailDto
    //{
    //    To = newRequest.CustEmail,
    //    Subject = lang.ToLower() == "en" ? "Your Request Has Been Created" : "تم إنشاء طلبك",
    //    Body = lang.ToLower() == "en"
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
            return NotFound(new { Message = lang.ToLower() == "en" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        var requestDetails = new UserRequestHeaderDto
        {
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Note = request.Note,
            Reply = request.Reply,
            SpecialRequest = request.SpecialRequest,
            HotelName = request.Hotel?.Name ?? "N/A",
            RoomNumber = request.Room?.Number ?? "N/A",
            StatusName = lang.ToLower() == "en" ? request.Status?.NameEn ?? "N/A" : request.Status?.NameAr ?? "غير معرف",
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                Reply = rd.Reply,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                StatusName = lang.ToLower() == "en" ? rd.Status?.NameEn ?? "N/A" : rd.Status?.NameAr ?? "غير معرف",
                ServiceName = lang.ToLower() == "en" ? rd.Service?.NameEn ?? "N/A" : rd.Service?.NameAr ?? "غير معرف",
            }).ToList()
        };
        return Ok(requestDetails);
    }

    [HttpPut("AddReviewToRequest")]
    public async Task<IActionResult> AddReviewToRequest(string lang, [FromBody] UpdateRequestDto review)
    {
        if (review == null)
        {
            return BadRequest(new { Message = lang.ToLower() == "en" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });
        }
        var existingRequest = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == review.Id);
        if (existingRequest == null)
        {
            return NotFound(new { Message = lang.ToLower() == "en" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        if (existingRequest.Review != null)
        {
            return BadRequest(new { Message = lang.ToLower() == "en" ? "Review already exists." : "المراجعة موجودة بالفعل" });
        }
        existingRequest.Review = review.Text;
        await _unitOfWork.RequestHeader.UpdateAsync(existingRequest);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new { Message = lang.ToLower() == "en" ? "Review Added Successfully" : "تم إضافة المراجعة بنجاح" });
    }

    [HttpGet("GetAllRequests")]
    public async Task<IActionResult> GetAllRequests(string lang)
    {
        var requests = (await _unitOfWork.RequestHeader.FindAllAsync(includes: ["Status", "Room"])).OrderBy(r => r.UpdatedAt).ThenBy(r => r.CreatedAt);
        if (requests == null || !requests.Any())
        {
            return NotFound(new { Message = lang.ToLower() == "en" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        var requestDtos = requests.Select(r => new RequestHeaderDto
        {
            Id = r.Id,
            Code = r.Code,
            CustName = r.CustName,
            CustEmail = r.CustEmail,
            CustPhone = r.CustPhone,
            RoomNumber = r.Room?.Number ?? "N/A",
            StatusName = lang.ToLower() == "en" ? r.Status?.NameEn ?? "N/A" : r.Status?.NameAr ?? "غير معرف"
        }).ToList();
        return Ok(requestDtos);
    }

    [HttpGet("GetRequestDetails{id}")]
    public async Task<IActionResult> GetRequestDetails(string lang, int id)
    {
        var request = await _unitOfWork.RequestHeader.FindAsync(r => r.Id == id, ["Hotel", "Room", "Status", "RequestDetails.Service", "RequestDetails.Status"]);
        if (request == null)
        {
            return NotFound(new { Message = lang.ToLower() == "en" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        var requestDetails = new RequestHeaderDto
        {
            Id = request.Id,
            CustName = request.CustName,
            CustEmail = request.CustEmail,
            CustPhone = request.CustPhone,
            Code = request.Code,
            Note = request.Note,
            SpecialRequest = request.SpecialRequest,
            AttachmentPath = request.AttachmentPath,
            Reply = request.Reply,
            Review = request.Review,
            HotelName = request.Hotel?.Name ?? "N/A",
            RoomNumber = request.Room?.Number ?? "N/A",
            StatusName = lang.ToLower() == "en" ? request.Status?.NameEn ?? "N/A" : request.Status?.NameAr ?? "غير معرف",
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt,
            RequestDetails = request.RequestDetails?.Select(rd => new RequestDetailsDto
            {
                Id = rd.Id,
                Note = rd.Note,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt,
                Reply = rd.Reply,
                StatusName = lang.ToLower() == "en" ? rd.Status?.NameEn ?? "N/A" : rd.Status?.NameAr ?? "غير معرف",
                ServiceName = lang.ToLower() == "en" ? rd.Service?.NameEn ?? "N/A" : rd.Service?.NameAr ?? "غير معرف"
            }).ToList()
        };
        var statusList = (await _unitOfWork.Status.GetAllAsync()).Select(s => new SelectStatusDTO
        {
            Id = s.Id,
            Name = lang.ToLower() == "en" ? s.NameEn : s.NameAr
        }).ToList();
        return Ok(new { requestDetails, statusList });
    }

    [HttpPut("AddResponseToRequest")]
    public async Task<IActionResult> AddResponseToRequest(string lang, [FromBody] UpdateRequestStatusDto model)
    {
        if (model == null)
            return BadRequest(new { Message = lang.ToLower() == "en" ? "Invalid request data." : "بيانات الطلب غير مكتملة او غير صحيحة" });

        var request = await _unitOfWork.RequestHeader.FindAsync(rh => rh.Id == model.RequestId);
        if (request == null)
        {
            return NotFound(new { Message = lang.ToLower() == "en" ? "Request not found" : "هذا الطلب غير موجود" });
        }
        request.Reply = model.Response;
        await _unitOfWork.RequestHeader.UpdateAsync(request);

        await _unitOfWork.SaveChangesAsync();
        return Ok(new { Message = lang.ToLower() == "en" ? "Reply Added Successfully" : "تم إضافة الرد بنجاح" });
    }


    [HttpPut("UpdateServiceRequestStatus")]
    public async Task<IActionResult> UpdateServiceRequestStatus(string lang, [FromBody] UpdateRequestDetailsStatusDto model)
    {
        bool isEnglish = lang.ToLowerInvariant() == "en";

        if (model == null)
            return BadRequest(new { Message = isEnglish ? "Invalid service data." : "بيانات الخدمة غير مكتملة او غير صحيحة" });

        var requestDetails = await _unitOfWork.RequestDetails.FindAsync(rd => rd.Id == model.RequestDetailsId);
        if (requestDetails == null)
            return NotFound(new { Message = isEnglish ? "Service not found" : "هذه الخدمة غير موجود" });

        var status = await _unitOfWork.Status.FindAsync(s => isEnglish ? s.NameEn == model.Status : s.NameAr == model.Status);
        if (status == null)
            return BadRequest(new { Message = isEnglish ? "Invalid status" : "حالة غير صالحة" });

        requestDetails.StatusId = status.Id;
        requestDetails.Reply = model.Note;
        await _unitOfWork.RequestDetails.UpdateAsync(requestDetails);
        await _unitOfWork.SaveChangesAsync();

        var requestHeader = await _unitOfWork.RequestHeader.FindAsync(rh => rh.Id == requestDetails.RequestHeaderId);

        // Fetch all status values at once to reduce DB hits
        var statuses = await _unitOfWork.Status.FindAllAsync(_ => true);
        var completedStatus = statuses.FirstOrDefault(s => s.NameEn == "Completed");
        var rejectedStatus = statuses.FirstOrDefault(s => s.NameEn == "Rejected");
        var inReviewStatus = statuses.FirstOrDefault(s => s.NameEn == "In Review");

        if (completedStatus == null || rejectedStatus == null || inReviewStatus == null)
            return StatusCode(500, new { Message = isEnglish ? "Server configuration error: status values missing." : "خطأ في الإعدادات: حالات الحالة غير موجودة" });

        var allServices = await _unitOfWork.RequestDetails.FindAllAsync(rd => rd.RequestHeaderId == requestHeader.Id);
        int completed = allServices.Count(s => s.StatusId == completedStatus.Id);
        int rejected = allServices.Count(s => s.StatusId == rejectedStatus.Id);

        if (completed == allServices.Count())
            requestHeader.StatusId = completedStatus.Id;
        else if (rejected == allServices.Count())
            requestHeader.StatusId = rejectedStatus.Id;
        else if (rejected + completed == allServices.Count())
            requestHeader.StatusId = completedStatus.Id;
        else
            requestHeader.StatusId = inReviewStatus.Id;

        await _unitOfWork.RequestHeader.UpdateAsync(requestHeader);
        await _unitOfWork.SaveChangesAsync();

        return Ok(new { Message = isEnglish ? "Service status updated successfully" : "تم تحديث حالة الطلب بنجاح" });
    }
}
