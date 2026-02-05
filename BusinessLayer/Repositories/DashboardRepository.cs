using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLayer.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardRepository> _logger;

        public DashboardRepository(ApplicationDbContext context, ILogger<DashboardRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<dynamic> GetWRsByTrial(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;
            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            // top 10 trials ordered
            List<dynamic> topTrialsOrdered = _context.WorkRequests
                .Where(c => c.CreationDateTime >= cutoffDate)
                .GroupBy(wr => wr.Trial.TrialName)
                .Select(g => new
                {
                    TrialName = g.Key,
                    TotalCount = g.Count()
                })
                .OrderByDescending(x => x.TotalCount)
                .Take(10)
                .ToList<dynamic>();

            List<dynamic> topTrialNames = topTrialsOrdered.Select(t => t.TrialName).ToList();

            // group trials by status and trial
            var trialsByStatus = _context.WorkRequests
                .Where(wr => topTrialNames.Contains(wr.Trial.TrialName) && wr.CreationDateTime >= cutoffDate)
                .GroupBy(wr => new { wr.Trial.TrialName, wr.Status.WorkRequestStatusName })
                .Select(g => new
                {
                    g.Key.TrialName,
                    Status = g.Key.WorkRequestStatusName,
                    Count = g.Count()
                })
                .ToList();

            List<dynamic> orderedData = trialsByStatus
                .OrderBy(d => topTrialNames.IndexOf(d.TrialName))
                .ToList<dynamic>();

            return orderedData;
        }

        public List<dynamic> GetAllWRsByStatus(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;
            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            return _context.WorkRequests
                .Where(c => c.CreationDateTime >= cutoffDate)
                .GroupBy(x => x.Status.WorkRequestStatusName)
                .Select(g => new
                {
                    StatusName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ToList<dynamic>();
        }

        public List<dynamic> GetDecisionAndCompletionTime(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;

            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            List<WorkRequestEvent> relevantEvents = _context.WorkRequestEvents
                .Include(e => e.EventType)
                .Include(e => e.WorkRequest)
                .AsSingleQuery()
                .Where(e =>
                    (e.EventType.WorkRequestEventTypeName == "Approve" ||
                     e.EventType.WorkRequestEventTypeName == "Close as Completed")
                     && e.WorkRequest.CreationDateTime >= cutoffDate)
                .ToList();

            return relevantEvents
                .GroupBy(e => e.WorkRequest.WorkRequestId)
                .Select(g =>
                {
                    WorkRequest wr = g.First().WorkRequest;
                    DateTime creationDate = wr.CreationDateTime;

                    WorkRequestEvent approval = g.FirstOrDefault(e => e.EventType.WorkRequestEventTypeName == "Approve");
                    WorkRequestEvent completion = g.FirstOrDefault(e => e.EventType.WorkRequestEventTypeName == "Close as Completed");

                    return new
                    {
                        wr.WorkRequestId,
                        CreationDate = creationDate,
                        MonthYear = new DateTime(creationDate.Year, creationDate.Month, 1),
                        DurationToApproval = approval != null ? Math.Round((decimal)(approval.CreatedAt - creationDate).TotalDays, 2) : 0,
                        DurationToCompletion = completion != null ? Math.Round((decimal)(completion.CreatedAt - creationDate).TotalDays, 2) : 0
                    };
                })
                .ToList<dynamic>();
        }

        public List<dynamic> GetAverageDecisionAndCompletionTime(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;
            bool groupByDay = false;
            bool groupByWeek = false;

            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    groupByDay = true;
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    groupByWeek = true;
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    groupByWeek = true;
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            List<dynamic> durations = GetDecisionAndCompletionTime(dateInput)
                .Where(x => x.CreationDate >= cutoffDate)
                .ToList();

            List<string> expectedLabels = new List<string>();

            DateTime now = DateTime.UtcNow;

            if (groupByDay)
            {
                for (DateTime date = cutoffDate.Date; date <= now.Date; date = date.AddDays(1))
                {
                    expectedLabels.Add(date.ToString("dd MMM yyyy"));
                }
            }
            else if (groupByWeek)
            {
                for (DateTime date = GetWeekStart(cutoffDate); date <= now.Date; date = date.AddDays(7))
                {
                    expectedLabels.Add(date.ToString("dd MMM yyyy"));
                }
            }
            else
            {
                for (DateTime date = new DateTime(cutoffDate.Year, cutoffDate.Month, 1); date <= now.Date; date = date.AddMonths(1))
                {
                    expectedLabels.Add(date.ToString("MMM yyyy"));
                }
            }

            Func<dynamic, string> groupKeySelector;

            if (groupByDay)
            {
                groupKeySelector = x => x.CreationDate.ToString("dd MMM yyyy");
            }
            else if (groupByWeek)
            {
                groupKeySelector = x => GetWeekStart(x.CreationDate).ToString("dd MMM yyyy");
            }
            else
            {
                groupKeySelector = x => x.CreationDate.ToString("MMM yyyy");
            }

            var grouped = durations
                .GroupBy(groupKeySelector)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        AverageDurationToApproval = g.Where(x => x.DurationToApproval != null).Average(x => (double?)x.DurationToApproval) ?? 0,
                        AverageDurationToCompletion = g.Where(x => x.DurationToCompletion != null).Average(x => (double?)x.DurationToCompletion) ?? 0
                    });

            List<dynamic> result = expectedLabels
                .Select(label => new
                {
                    GroupKey = label,
                    AverageDurationToApproval = grouped.ContainsKey(label) ? grouped[label].AverageDurationToApproval : 0,
                    AverageDurationToCompletion = grouped.ContainsKey(label) ? grouped[label].AverageDurationToCompletion : 0
                })
                .ToList<dynamic>();

            return result;
        }

        public List<dynamic> GetAssignmentGroupCounts(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;

            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            List<string> groups = new List<string>
                        {
                            "ytu-developers-group",
                            "ytu-redcap-group",
                            "ytu-datavalidation-group"
                        };

            // all assignment events within the time frame
            List<WorkRequestEvent> assignmentEvents = _context.WorkRequestEvents
                .Include(c => c.EventType)
                .Include(c => c.WorkRequest)
                .Where(c => c.EventType.WorkRequestEventTypeName == "Assignment" && c.Content != null && c.WorkRequest.CreationDateTime >= cutoffDate)
                .ToList();

            // only show user assignment
            List<dynamic> groupCounts = groups
                .Select(group => new
                {
                    Group = group,
                    Count = assignmentEvents.Count(e => e.Content.Contains($"- assigned - {group}"))
                })
                .ToList<dynamic>();

            return groupCounts;
        }

        public Dictionary<string, int> GetCurrentAgeOfIncompleteTasks(string dateInput = "12m")
        {
            DateTime cutoffDate = DateTime.UtcNow;
            switch (dateInput)
            {
                case "1w":
                    cutoffDate = cutoffDate.AddDays(-7);
                    break;
                case "4w":
                    cutoffDate = cutoffDate.AddMonths(-1);
                    break;
                case "3m":
                    cutoffDate = cutoffDate.AddMonths(-3);
                    break;
                case "6m":
                    cutoffDate = cutoffDate.AddMonths(-6);
                    break;
                case "12m":
                    cutoffDate = cutoffDate.AddMonths(-12);
                    break;
            }

            List<string> incompleteStatuses = new() { "Pending Requester", "Pending Initial Approval", "Pending Work" };

            List<WorkRequest> incompleteWRs = _context.WorkRequests
                .Include(x => x.Status)
                .Where(c => incompleteStatuses.Contains(c.Status.WorkRequestStatusName) && c.CreationDateTime >= cutoffDate)
                .ToList();

            Dictionary<string, int> ageBrackets = new()
            {
                { "≤3 days", 0 },
                { "4–7 days", 0 },
                { "8–14 days", 0 },
                { "15–30 days", 0 },
                { ">30 days", 0 }
            };

            foreach (WorkRequest wr in incompleteWRs)
            {
                int age = (DateTime.Now - wr.CreationDateTime).Days;

                if (age <= 3)
                    ageBrackets["≤3 days"]++;
                else if (age <= 7)
                    ageBrackets["4–7 days"]++;
                else if (age <= 14)
                    ageBrackets["8–14 days"]++;
                else if (age <= 30)
                    ageBrackets["15–30 days"]++;
                else
                    ageBrackets[">30 days"]++;
            }

            return ageBrackets;
        }
        public DateTime GetWeekStart(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.Date.AddDays(-diff);
        }
    }
}
