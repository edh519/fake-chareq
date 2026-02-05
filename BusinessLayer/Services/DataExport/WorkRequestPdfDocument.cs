using BusinessLayer.Helpers;
using BusinessLayer.Services.Models.DTOs;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Enums.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLayer.Services.DataExport
{
    public class WorkRequestPdfDocument : IDocument
    {
        private readonly WorkRequestExportDto _data;
        private readonly string _exportedBy;

        public WorkRequestPdfDocument(
            WorkRequestExportDto data,
            string exportedBy)
        {
            _data = data;
            _exportedBy = exportedBy;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            List<WorkRequestEventTypeEnum> closedWorkRequestEvents = new List<WorkRequestEventTypeEnum>
                    {
                        WorkRequestEventTypeEnum.Complete,
                            WorkRequestEventTypeEnum.Closed,
                    };
            List<SubTaskEventTypeEnum> closedSubTaskEvents = new List<SubTaskEventTypeEnum>
                    {
                        SubTaskEventTypeEnum.Approve,
                            SubTaskEventTypeEnum.Reject,
                            SubTaskEventTypeEnum.Abandon
                    };

            WorkRequestEvent closedEvent = _data.WorkRequest.WorkRequestEvents.Where(i => closedWorkRequestEvents.Contains(i.EventType.WorkRequestEventTypeId)).FirstOrDefault();

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.Header().Column(col =>
                {
                    col.Item()
                        .Text($"Data Export - #{_data.WorkRequest.WorkRequestId}")
                        .SemiBold()
                        .FontSize(24)
                        .AlignCenter();

                    col.Item().Text($"Exported by {CommonHelpers.RemoveDomainFromEmail(_exportedBy)} at {DateTime.Now}").AlignCenter();

                    col.Item().Text("");
                });

                page.Content().Column(col =>
                {
                    col.Item().Text(text =>
                    {
                        text.Span($"Trial: ").Bold();
                        text.Span($"{_data.WorkRequest.Trial.TrialName}");
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"Work Request Id: ").Bold();
                        text.Span($"#{_data.WorkRequest.WorkRequestId}");
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"Title: ").Bold();
                        text.Span($"{_data.WorkRequest.Reference}");
                    });

                    col.Item().Text("");

                    col.Item().Text(text =>
                    {
                        text.Span($"Status: ").Bold();
                        text.Span($"{_data.WorkRequest.Status.WorkRequestStatusName}");
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"Created By: ").Bold();
                        text.Span($"{CommonHelpers.RemoveDomainFromEmail(_data.WorkRequest.CreatedBy)} - {_data.WorkRequest.CreationDateTime}");
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"Completed By: ").Bold();
                        text.Span($"{CommonHelpers.RemoveDomainFromEmail(closedEvent.CreatedBy.Email)} - {closedEvent.CreatedAt}");
                    });

                    col.Item().Text("");

                    col.Item().Text("Description").Bold().FontSize(14);
                    col.Item().Text(HtmlHelper.RemoveHtmlElementsFromString(_data.WorkRequest.DetailDescription));

                    col.Item().Text("");

                    col.Item().Text("Metadata").Bold().FontSize(14);

                    col.Item().Text(text =>
                    {
                        text.Span($"Assignees: ").Bold();
                        text.Span(string.Join(", ", _data.WorkRequest.Assignees.Select(a => CommonHelpers.RemoveDomainFromEmail(a.Email))));
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"Labels: ").Bold();
                        text.Span(string.Join(", ", _data.WorkRequest.Labels.Select(a => a.LabelShort)));
                    });
                    col.Item().Text(text =>
                    {
                        text.Span($"GitHub Issue: ").Bold();
                        if (_data.GHIssue == null)
                        {
                            col.Item().Text("n/a");
                        }
                        else
                        {
                            text.Hyperlink(_data.GHIssue.HtmlUrl, _data.GHIssue.HtmlUrl);
                        }
                    });

                    col.Item().Text("");

                    col.Item().Text("Files:").Bold().FontSize(14);
                    if (_data.WorkRequest.SupportingFiles.Count() == 0)
                    {
                        col.Item().Text("n/a");
                    }
                    else
                    {
                        foreach (FileUpload f in _data.WorkRequest.SupportingFiles)
                        {
                            col.Item().Text(f.ReadableFileName);
                        }
                    }

                    col.Item().Text("");

                    col.Item().Text("Discussion").Bold().FontSize(14);

                    foreach (DataExportDiscussionViewModel dvm in _data.DiscussionVM)
                    {
                        col.Item().Text($"[{dvm.CreatedAt}] {dvm.Content}");
                    }

                    col.Item().Text("");

                    if (_data.SubTasks.Count() != 0)
                    {
                        col.Item().Text("Sub Tasks").Bold().FontSize(14);

                        col.Item().PaddingLeft(30).Column(innerCol =>
                        {
                            foreach (SubTask subtask in _data.SubTasks)
                            {
                                SubTaskEvent closedSubTaskEvent = subtask.SubTaskEvents.FirstOrDefault(i => closedSubTaskEvents.Contains(i.EventType.SubTaskEventTypeId));

                                innerCol.Item().Text("");
                                innerCol.Item().Text($"Subtask #{subtask.SubTaskId} - {HtmlHelper.RemoveHtmlElementsFromString(subtask.SubTaskTitle)}").SemiBold().FontSize(12).FontColor("#808080");
                                innerCol.Item().Text("");

                                innerCol.Item().PaddingLeft(15).Column(innerInnerCol =>
                                {
                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Sub Task Id: ").Bold();
                                        text.Span($"#{subtask.SubTaskId}");
                                    });

                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Title: ").Bold();
                                        text.Span($"{HtmlHelper.RemoveHtmlElementsFromString(subtask.SubTaskTitle)}");
                                    });

                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Assignee: ").Bold();
                                        text.Span($"{CommonHelpers.RemoveDomainFromEmail(subtask.Assignee.Email)}");
                                    });

                                    innerInnerCol.Item().Text("");

                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Status: ").Bold();
                                        text.Span($"{subtask.Status.SubTaskStatusName}");
                                    });

                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Created By: ").Bold();
                                        text.Span($"{CommonHelpers.RemoveDomainFromEmail(subtask.CreatedBy)} - {subtask.CreationDateTime}");
                                    });

                                    innerInnerCol.Item().Text(text =>
                                    {
                                        text.Span("Completed By: ").Bold();
                                        text.Span($"{CommonHelpers.RemoveDomainFromEmail(closedSubTaskEvent.CreatedBy.Email)} - {closedSubTaskEvent.CreatedAt}");
                                    });
                                    innerInnerCol.Item().Text("");
                                    innerInnerCol.Item().Text("Discussion: ").Bold();

                                    foreach (SubTaskEvent ste in subtask.SubTaskEvents)
                                    {
                                        string? stContent = HtmlHelper.RemoveHtmlElementsFromString(ste.Content);

                                        string? stEventContent = null;

                                        if (stContent.Contains(" at "))
                                        {
                                            stEventContent = stContent.Substring(0, stContent.IndexOf(" at ")).Trim();
                                        }
                                        else
                                        {
                                            stEventContent = $"{CommonHelpers.RemoveDomainFromEmail(ste.CreatedBy.Email)} - {ste.EventType.SubTaskEventTypeName.ToLower()} - '{stContent}'";
                                        }

                                        innerInnerCol.Item().Text($"[{ste.CreatedAt}] {stEventContent}");
                                    }

                                });
                            }
                        });
                    }
                });

                page.Footer().AlignRight().Text(text =>
                {
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        }
    }

}
