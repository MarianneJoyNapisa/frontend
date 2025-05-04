using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using HomeownersMS.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HomeownersMS.Pages.Staff.Services
{
    [Authorize(Roles = "staff,admin")]
    public class IndexModel(HomeownersContext context, INotificationService notificationService) : PageModel 
    {
        private readonly HomeownersContext _context = context;
        private readonly INotificationService _notificationService = notificationService;

        public Models.Staff? CurrentStaff { get; set; }
        public List<Models.Service> AvailableServices { get; set; } = new List<Models.Service>();
        public List<ServiceRequest> ActiveServiceRequests { get; set; } = new List<ServiceRequest>();
        public List<ServiceRequest> ServiceLogs { get; set; } = new List<ServiceRequest>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User identifier claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            CurrentStaff = await _context.Staffs
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (CurrentStaff == null)
            {
                return NotFound();
            }

            // Get available services based on staff job
            AvailableServices = CurrentStaff.Job switch
            {
                StaffJob.Maintenance => await _context.Services
                    .Where(s => s.ServiceCategory == ServiceCategory.repairsAndMaintenance)
                    .ToListAsync(),
                StaffJob.Janitorial => await _context.Services
                    .Where(s => s.ServiceCategory == ServiceCategory.cleaningServices || 
                               s.ServiceCategory == ServiceCategory.landscapingMaintenance)
                    .ToListAsync(),
                StaffJob.Special => await _context.Services
                    .Where(s => s.ServiceCategory == ServiceCategory.otherServices)
                    .ToListAsync(),
                _ => new List<Models.Service>()
            };

            // Get active service requests (pending or inProgress)
            ActiveServiceRequests = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.Resident)
                .Where(sr => sr.StaffAcceptedBy == CurrentStaff.UserId && 
                           (sr.Status == Statuses.pending || sr.Status == Statuses.inProgress))
                .OrderByDescending(sr => sr.RequestedDate)
                .ToListAsync();

            // Get service logs (completed or cancelled)
            ServiceLogs = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.Resident)
                .Where(sr => sr.StaffAcceptedBy == CurrentStaff.UserId && 
                           (sr.Status == Statuses.completed || sr.Status == Statuses.cancelled))
                .OrderByDescending(sr => sr.RequestApprovedDateTime)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostTakeServiceAsync(int serviceRequestId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User identifier claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            var staff = await _context.Staffs.FirstOrDefaultAsync(s => s.UserId == userId);

            if (staff == null)
            {
                return NotFound();
            }

            // Check if staff already has an in-progress service
            var hasInProgress = await _context.ServiceRequests
                .AnyAsync(sr => sr.StaffAcceptedBy == staff.UserId && sr.Status == Statuses.inProgress);

            if (hasInProgress)
            {
                TempData["ErrorMessage"] = "You already have a service in progress. Complete it before taking another one.";
                return RedirectToPage();
            }

            var serviceRequest = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .FirstOrDefaultAsync(sr => sr.ServiceRequestId == serviceRequestId);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            // Validate that staff can take this service based on their job
            bool canTakeService = staff.Job switch
            {
                StaffJob.Maintenance => serviceRequest.Service != null && serviceRequest.Service.ServiceCategory == ServiceCategory.repairsAndMaintenance,
                StaffJob.Janitorial => serviceRequest.Service?.ServiceCategory == ServiceCategory.cleaningServices || 
                                        serviceRequest.Service?.ServiceCategory == ServiceCategory.landscapingMaintenance,
                StaffJob.Special => serviceRequest.Service?.ServiceCategory == ServiceCategory.otherServices,
                _ => false
            };

            if (!canTakeService)
            {
                TempData["ErrorMessage"] = "You are not authorized to take this type of service.";
                return RedirectToPage();
            }

            // Update service request
            serviceRequest.StaffAcceptedBy = staff.UserId;
            serviceRequest.RequestApprovedDateTime = DateTime.Now;
            serviceRequest.Status = Statuses.inProgress;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Service request accepted successfully!";


            // Reverse lookup to find the staff who accepted the service request
            var staffWhoAccepted = await _context.Staffs
                .FirstOrDefaultAsync(s => s.UserId == serviceRequest.StaffAcceptedBy);

            var staffName = $"{staffWhoAccepted?.FName} {staffWhoAccepted?.LName}";

            if (staffWhoAccepted == null || staffWhoAccepted.LName == null || staffWhoAccepted.FName == null)
            {
                staffName = "N/A";
            }

            var title = $"{serviceRequest.Service?.Title ?? "No service"} (ID/{serviceRequest.ServiceRequestId})";
            var message = $"Your service request (ID/{serviceRequest.ServiceRequestId}) has been accepted by {staffName}.";
            var url = "/Service/Service/#current-services-table";
            var messageType = MessageTypes.service;
            var createdBy = serviceRequest.StaffAcceptedBy ?? 0;
            var userGroup = new List<int>{ serviceRequest.RequestedBy ?? 0 };

            // Notify resident for staff successfully serving their service
            await _notificationService.CreateNotificationForGroup( 
                title,
                message,
                url,
                messageType,
                createdBy,
                userGroup
            );

            return RedirectToPage();
        }
    }
}