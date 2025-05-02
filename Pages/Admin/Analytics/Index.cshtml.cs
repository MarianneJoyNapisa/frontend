using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Analytics
{
    public class IndexModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        // Facility Reservation Analytics
        public Facility? MostReservedFacility { get; set; }
        public int MostReservedFacilityCount { get; set; }
        public Resident? TopResidentReservations { get; set; }
        public int TopResidentReservationsCount { get; set; }
        public string? MostCommonEventType { get; set; }
        public int MostCommonEventTypeCount { get; set; }
        public int TotalFacilityRequests { get; set; }
        public int ApprovedFacilityRequests { get; set; }
        public List<FacilityReservationData> FacilityReservationData { get; set; } = new List<FacilityReservationData>();
        public List<FacilityReviewData> FacilityReviewsData { get; set; } = new List<FacilityReviewData>();

        // Service Request Analytics
        public Models.Service? MostRequestedService { get; set; }
        public int MostRequestedServiceCount { get; set; }
        public Resident? TopResidentServiceRequests { get; set; }
        public int TopResidentServiceRequestsCount { get; set; }
        public string? MostCommonServiceCategory { get; set; }
        public int MostCommonServiceCategoryCount { get; set; }
        public Models.Staff? MostActiveStaff { get; set; }
        public int MostActiveStaffCount { get; set; }
        public List<ServiceRequestData> ServiceRequestsData { get; set; } = new List<ServiceRequestData>();
        public List<Models.Staff> StaffList { get; set; } = new List<Models.Staff>();
        public List<StaffPerformanceData> StaffPerformanceData { get; set; } = new List<StaffPerformanceData>();

        public async Task OnGetAsync()
        {
            await LoadFacilityReservationAnalytics();
            await LoadServiceRequestAnalytics();
        }

        private async Task LoadFacilityReservationAnalytics()
        {
            // Most reserved facility
            var facilityReservations = await _context.FacilityRequests
                .Where(fr => fr.Status == RequestStatus.Approved)
                .GroupBy(fr => fr.FacilityId)
                .Select(g => new { FacilityId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (facilityReservations.Any())
            {
                MostReservedFacilityCount = facilityReservations.First().Count;
                MostReservedFacility = await _context.Facilities
                    .FirstOrDefaultAsync(f => f.FacilityId == facilityReservations.First().FacilityId);
            }

            // Resident with most reservations
            var residentReservations = await _context.FacilityRequests
                .Where(fr => fr.Status == RequestStatus.Approved)
                .GroupBy(fr => fr.ResidentId)
                .Select(g => new { ResidentId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (residentReservations.Any())
            {
                TopResidentReservationsCount = residentReservations.First().Count;
                TopResidentReservations = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == residentReservations.First().ResidentId);
            }

            // Most common event type
            var eventTypes = await _context.Events
                .GroupBy(e => e.EventType)
                .Select(g => new { EventType = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (eventTypes.Any())
            {
                MostCommonEventType = eventTypes.First().EventType.ToString();
                MostCommonEventTypeCount = eventTypes.First().Count;
            }

            // Total and approved facility requests
            TotalFacilityRequests = await _context.FacilityRequests.CountAsync();
            ApprovedFacilityRequests = await _context.FacilityRequests
                .CountAsync(fr => fr.Status == RequestStatus.Approved);

            // Facility reservations data for chart
            FacilityReservationData = await _context.Facilities
                .Select(f => new FacilityReservationData
                {
                    FacilityId = f.FacilityId,
                    FacilityName = f.Name,
                    ReservationCount = _context.FacilityRequests
                        .Count(fr => fr.FacilityId == f.FacilityId && fr.Status == RequestStatus.Approved)
                })
                .Where(f => f.ReservationCount > 0)
                .OrderByDescending(f => f.ReservationCount)
                .ToListAsync();

            // Facility reviews data for chart
            FacilityReviewsData = await _context.Facilities
                .Select(f => new FacilityReviewData
                {
                    FacilityId = f.FacilityId,
                    FacilityName = f.Name,
                    ReviewCount = f.FacilityReviews.Count
                })
                .Where(f => f.ReviewCount > 0)
                .OrderByDescending(f => f.ReviewCount)
                .ToListAsync();
        }

        private async Task LoadServiceRequestAnalytics()
        {
            // Most requested service
            var serviceRequests = await _context.ServiceRequests
                .Where(sr => sr.Status != Statuses.cancelled)
                .GroupBy(sr => sr.ServiceId)
                .Select(g => new { ServiceId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (serviceRequests.Any() && serviceRequests.First().ServiceId.HasValue)
            {
                MostRequestedServiceCount = serviceRequests.First().Count;
                MostRequestedService = await _context.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceRequests.First().ServiceId.Value);
            }

            // Resident with most service requests
            var residentServiceRequests = await _context.ServiceRequests
                .Where(sr => sr.Status != Statuses.cancelled)
                .GroupBy(sr => sr.RequestedBy)
                .Select(g => new { ResidentId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (residentServiceRequests.Any() && residentServiceRequests.First().ResidentId.HasValue)
            {
                TopResidentServiceRequestsCount = residentServiceRequests.First().Count;
                TopResidentServiceRequests = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == residentServiceRequests.First().ResidentId.Value);
            }

            // Most common service category
            var serviceCategories = await _context.Services
                .GroupBy(s => s.ServiceCategory)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (serviceCategories.Any() && serviceCategories.First().Category.HasValue)
            {
                MostCommonServiceCategory = serviceCategories.First().Category.Value.ToString();
                MostCommonServiceCategoryCount = serviceCategories.First().Count;
            }

            // Most active staff
            var activeStaff = await _context.ServiceRequests
                .Where(sr => sr.Status != Statuses.cancelled && sr.StaffAcceptedBy.HasValue)
                .GroupBy(sr => sr.StaffAcceptedBy)
                .Select(g => new { StaffId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            if (activeStaff.Any() && activeStaff.First().StaffId.HasValue)
            {
                MostActiveStaffCount = activeStaff.First().Count;
                MostActiveStaff = await _context.Staffs
                    .FirstOrDefaultAsync(s => s.UserId == activeStaff.First().StaffId.Value);
            }

            // Service requests data for chart
            ServiceRequestsData = await _context.Services
                .Select(s => new ServiceRequestData
                {
                    ServiceId = s.ServiceId,
                    ServiceName = s.Title,
                    RequestCount = _context.ServiceRequests.Count(sr => sr.ServiceId == s.ServiceId && sr.Status != Statuses.cancelled)
                })
                .Where(s => s.RequestCount > 0)
                .OrderByDescending(s => s.RequestCount)
                .ToListAsync();

            // Staff list for dropdown
            StaffList = await _context.Staffs.ToListAsync();

            // Staff performance data
            StaffPerformanceData = await _context.Staffs
                .Select(s => new StaffPerformanceData
                {
                    StaffId = s.UserId,
                    StaffName = $"{s.FName} {s.LName}",
                    TotalServices = _context.ServiceRequests.Count(sr => sr.StaffAcceptedBy == s.UserId && sr.Status == Statuses.completed),
                    MostCommonService = _context.ServiceRequests
                        .Where(sr => sr.StaffAcceptedBy == s.UserId)
                        .GroupBy(sr => sr.ServiceId)
                        .Select(g => new { ServiceId = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .Select(x => x.ServiceId)
                        .FirstOrDefault(),
                    MostCommonServiceCount = _context.ServiceRequests
                        .Where(sr => sr.StaffAcceptedBy == s.UserId)
                        .GroupBy(sr => sr.ServiceId)
                        .Select(g => new { Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .Select(x => x.Count)
                        .FirstOrDefault()
                })
                .Where(s => s.TotalServices > 0)
                .ToListAsync();

            // Get service names for most common services
            foreach (var staff in StaffPerformanceData)
            {
                if (staff.MostCommonService.HasValue)
                {
                    var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == staff.MostCommonService.Value);
                    staff.MostCommonServiceName = service?.Title ?? "Unknown";
                }
            }
        }
    }

    public class FacilityReservationData
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public int ReservationCount { get; set; }
    }

    public class FacilityReviewData
    {
        public int FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public int ReviewCount { get; set; }
    }

    public class ServiceRequestData
    {
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int RequestCount { get; set; }
    }

    public class StaffPerformanceData
    {
        public int StaffId { get; set; }
        public string? StaffName { get; set; }
        public int TotalServices { get; set; }
        public int? MostCommonService { get; set; }
        public string? MostCommonServiceName { get; set; }
        public int MostCommonServiceCount { get; set; }
    }
}