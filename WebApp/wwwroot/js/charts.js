
document.addEventListener('themeChanged', () => {
    reinitialiseChartsForTheme();
});

// ====== Constants ======

const statusColours = {
    "Pending Requester": "#e0c424",      
    "Pending Initial Approval": "#38bed6",
    "Pending Work": "#ff9317",            
    "Completed": "#3bad59",              
    "Closed": "#979c98"                  
};

const backgroundColours = [
    "#b277c9", // developers colour
    "#cf5763", // redcap colour
    "#5ea2d6" // data validation colour
];

// ====== Theme Change Functions ======


function getTextColor() {
    return getComputedStyle(document.documentElement)
        .getPropertyValue('--font-color')
        .trim();
}
function reinitialiseChartsForTheme() {
    const newColor = getTextColor();

    const applyColours = (chart) => {
        if (!chart) return;

        if (chart.options.plugins?.title) {
            chart.options.plugins.title.color = newColor;
        }

        if (chart.options.plugins?.legend?.labels) {
            chart.options.plugins.legend.labels.color = newColor;
        }

        if (chart.options.scales) {
            const scales = chart.options.scales;

            if (scales.x) {
                if (!scales.x.ticks) scales.x.ticks = {};
                scales.x.ticks.color = newColor;

                if (!scales.x.title) scales.x.title = {};
                scales.x.title.color = newColor;
            }

            if (scales.y) {
                if (!scales.y.ticks) scales.y.ticks = {};
                scales.y.ticks.color = newColor;

                if (!scales.y.title) scales.y.title = {};
                scales.y.title.color = newColor;
            }
        }

        chart.update();
    };

    applyColours(wrsByTrialChart);
    applyColours(pieChart);
    applyColours(durationStackedChart);
    applyColours(scatterChart);
    applyColours(assignmentGroupChart);
    applyColours(incompleteScatterChart);
}

// ====== Charts ======

// --- 1. Stacked Bar Chart: Top 10 Trials by Status ---
let wrsByTrialChart;

    const trialNames = [...new Set(totalWRsPerTrial.map(item => item.trialName))];
    const statuses = Object.keys(statusColours);

    const datasets = statuses.map(status => {
        const dataPoints = trialNames.map(trial => {
            const match = totalWRsPerTrial.find(item => item.trialName === trial && item.status === status);
            return match ? match.count : 0;
        });
        return {
            label: status,
            data: dataPoints,
            backgroundColor: statusColours[status] || "rgba(0, 0, 0, 0.5)"
        };
    });

    const ctx = document.getElementById('stackedChart').getContext('2d');

    if (wrsByTrialChart) {
        wrsByTrialChart.data.labels = trialNames;
        wrsByTrialChart.data.datasets = datasets;
        wrsByTrialChart.update();
    } else {
        wrsByTrialChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: trialNames,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    title: {
                        display: true,
                        text: 'Top 10 Trials by Status',
                        color: getTextColor()
                    },
                    legend: {
                        labels: {
                            color: getTextColor()
                        }
                    }
                },
                scales: {
                    x: {
                        stacked: true,
                        ticks: {
                            color: getTextColor()
                        }
                    },
                    y: {
                        stacked: true,
                        ticks: {
                            color: getTextColor()
                        }
                    }
                }
            }
        });
    }

// --- 2. Pie Chart: All Work Requests by Status ---
const pieCtx = document.getElementById('statusPieChart').getContext('2d');

const statusLabels = allWRsByStatus.map(item => item.statusName);
const statusCounts = allWRsByStatus.map(item => item.count);
const statusBackgrounds = statusLabels.map(label => statusColours[label] || "rgba(100,100,100,0.5)");

const sumStatusCounts = statusCounts.reduce((a, b) => a + b, 0);
const completedObject = allWRsByStatus.find(item => item.statusName === 'Completed');
const closedObject = allWRsByStatus.find(item => item.statusName === 'Closed');
const completedClosedCount = (completedObject ? completedObject.count : 0) + (closedObject ? closedObject.count : 0);
const chart2SubtitleText = `${completedClosedCount} Finalised of ${sumStatusCounts} Total`;

const pieChart = new Chart(pieCtx, {
    type: 'pie',
    data: {
        labels: statusLabels,
        datasets: [{
            data: statusCounts,
            backgroundColor: statusBackgrounds,
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        plugins: {
            title: {
                display: true,
                text: 'Work Requests per Status',
                color: getTextColor()
            },
            subtitle: {
                display: true,
                text: chart2SubtitleText,
                color: getTextColor()
            },
            legend: {
                labels: {
                    color: getTextColor()
                }
            }
        }
    }
});

// --- 3. Stacked Duration Chart: Approval & Completion by Month ---
const durationLabels = averageDecisionsAndCompletedTime.map(item => item.groupKey);

const approvalDurations = averageDecisionsAndCompletedTime.map(item => item.averageDurationToApproval);
const completionDurations = averageDecisionsAndCompletedTime.map(item => item.averageDurationToCompletion);

const nonZeroApprovalDurations = approvalDurations.filter(function (val) { return val != 0 });
const nonZeroCompletionDurations = completionDurations.filter(function (val) { return val != 0 });

const avgApprovalDurations = nonZeroApprovalDurations.reduce((a, b) => a + b) / nonZeroApprovalDurations.length;
const avgCompletionDurations = nonZeroCompletionDurations.reduce((a, b) => a + b) / nonZeroCompletionDurations.length;
const chart3SubtitleText = `Avg ${avgApprovalDurations.toFixed(2)} days to Approval / ${avgCompletionDurations.toFixed(2)} days to Completion`;

const durationStackedChart = new Chart(document.getElementById("durationStackedChart"), {
    type: 'bar',
    data: {
        labels: durationLabels,
        datasets: [
            {
                label: "To Approval",
                data: approvalDurations,
                backgroundColor: "#ff9317"
            },
            {
                label: "To Completion",
                data: completionDurations,
                backgroundColor: "#3bad59"
            }
        ]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            title: {
                display: true,
                text: "Average Duration to Approval & Completion",
                color: getTextColor()
            },
            subtitle: {
                display: true,
                text: chart3SubtitleText,
                color: getTextColor()
            },
            legend: {
                labels: {
                    color: getTextColor()
                }
            }
        },
        scales: {
            x: {
                stacked: true,
                ticks: {
                    color: getTextColor(),
                    autoSkip: false,
                    maxRotation: 45,
                    minRotation: 0
                }
            },
            y: {
                stacked: true,
                ticks: {
                    color: getTextColor()
                },
                title: {
                    display: true,
                    text: "Average Duration (days)",
                    color: getTextColor()
                }
            }
        }
    }
});

// --- 4. Scatter Chart: Individual WR Duration ---
const approvalScatterData = decisionsAndCompletedTime
    .filter(wr => wr.durationToApproval !== null)
    .map(wr => ({
        x: new Date(wr.creationDate),
        y: wr.durationToApproval,
        workRequestId: wr.workRequestId
    }));

const completionScatterData = decisionsAndCompletedTime
    .filter(wr => wr.durationToCompletion !== null)
    .map(wr => ({
        x: new Date(wr.creationDate),
        y: wr.durationToCompletion,
        workRequestId: wr.workRequestId
    }));

const scatterCtx = document.getElementById('durationScatterChart').getContext('2d');

const sumCompletetionTime = decisionsAndCompletedTime.reduce((a, b) => { return a + b.durationToCompletion }, 0);
let chart4SubtitleText;
if (sumCompletetionTime < (365 * 2)) { //If less that 2 years worth, then display as days, otherwise years. 20,000 days doesn't mean anything!
    chart4SubtitleText = `${sumCompletetionTime.toFixed(2)} total days from created to completed for all ChaReqs`;
} else {
    chart4SubtitleText = `${(sumCompletetionTime / 365).toFixed(2)} total years from created to completed for all ChaReqs`;
}

let timeUnit;
switch (dateRange) {
    case "1w":
        timeUnit = "day";
        break;
    case "4w":
    case "3m":
        timeUnit = "week";
        break;
    case "6m":
    case "12m":
    default:
        timeUnit = "month";
        break;
}
const scatterChart = new Chart(scatterCtx, {
    type: 'scatter',
    data: {
        datasets: [
            {
                label: 'Duration to Approval (days)',
                data: approvalScatterData,
                backgroundColor: '#ff9317'
            },
            {
                label: 'Duration to Completion (days)',
                data: completionScatterData,
                backgroundColor: '#3bad59'
            }
        ]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            x: {
                type: 'time',
                time: {
                    unit: timeUnit,
                    tooltipFormat: 'dd MMM yyyy',
                    displayFormats: {
                        day: 'dd MMM yyyy',
                        week: 'dd MMM yyyy',
                        month: 'MMM yyyy'
                    }
                },
                title: {
                    display: true,
                    text: 'Creation Date',
                    color: getTextColor()
                },
                ticks: {
                    color: getTextColor()
                }
            },
            y: {
                title: {
                    display: true,
                    text: 'Duration (Days)',
                    color: getTextColor()
                },
                ticks: {
                    color: getTextColor()
                },
                beginAtZero: true
            }
        },
        plugins: {
            title: {
                display: true,
                text: "Individual Work Request Approval and Completion Times",
                color: getTextColor()
            },
            subtitle: {
                display: true,
                text: chart4SubtitleText,
                color: getTextColor()
            },
            legend: {
                labels: {
                    color: getTextColor()
                }
            },
            tooltip: {
                callbacks: {
                    label: context => {
                        const id = context.raw?.workRequestId ?? 'Unknown';
                        const duration = context.raw?.y ?? '?';
                        const date = new Date(context.raw?.x).toLocaleDateString('en-GB', {
                            day: '2-digit',
                            month: 'short',
                            year: 'numeric'
                        });
                        return [
                            `WR ${id}: ${context.dataset.label} = ${duration} days`,
                            `WR Creation Date = ${date}`
                        ];
                    }
                }
            }
        }
    }
});

// --- 5. Pie Chart: Triage Assignment Breakdown (Past Year) ---
const groupLabels = groupAssignmentSplit.map(item => item.group);
const groupCounts = groupAssignmentSplit.map(item => item.count);

const assignmentCtx = document.getElementById("assignmentChart").getContext("2d");

const assignmentGroupChart = new Chart(assignmentCtx, {
    type: 'pie',
    data: {
        labels: groupLabels,
        datasets: [{
            data: groupCounts,
            backgroundColor: backgroundColours,
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        plugins: {
            title: {
                display: true,
                text: 'Triage Assignment Breakdown',
                color: getTextColor()
            },
            legend: {
                labels: {
                    color: getTextColor()
                }
            }
        }
    }
});

// --- 6. Bar Chart: Age of Incomplete Tasks ---
const incompleteTasksLabels = Object.keys(ageOfIncompleteTasks);
const incompleteTasksDataValues = Object.values(ageOfIncompleteTasks);

const incompleteCtx = document.getElementById('incompleteTaskBarChart').getContext('2d');

const incompleteScatterChart = new Chart(incompleteCtx, {
    type: 'bar',
    data: {
        labels: incompleteTasksLabels,
        datasets: [{
            label: 'Number of Incomplete Tasks',
            data: incompleteTasksDataValues,
            backgroundColor: 'rgba(75, 192, 192, 0.6)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            title: {
                display: true,
                text: 'Age of Incomplete Tasks',
                color: getTextColor()
            },
            legend: {
                labels: {
                    color: getTextColor()
                },
                display: false
            }
        },
        scales: {
            x: {
                ticks: {
                    color: getTextColor()
                }
            },
            y: {
                beginAtZero: true,
                ticks: {
                    color: getTextColor()
                }
            }
        }
    }
});
