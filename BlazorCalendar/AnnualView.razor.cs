namespace BlazorCalendar;

using BlazorCalendar.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


partial class AnnualView : CalendarBase
{
    [CascadingParameter(Name = "SelectedView")]
    public DisplayedView DisplayedView { get; set; } = DisplayedView.Annual;

    private int _months = 11;
    [Parameter]
    public int Months
    {
        get
        {
            return _months;
        }
        set
        {
            _months = value;
            if (_months > 1) _months--;
        }
    }

    private System.DateTime _firstdate;
    [CascadingParameter(Name = "FirstDate")]
    public System.DateTime FirstDate
    {
        get
        {
            if (_firstdate == System.DateTime.MinValue) _firstdate = DateTimeUtil.TodayDate;
            return _firstdate.Date;
        }
        set
        {
            _firstdate = value;
        }
    }
    
    [CascadingParameter(Name = "TasksList")]
    public Tasks[]? TasksList { get; set; }


    [Parameter]
    public PriorityLabel PriorityDisplay { get; set; } = PriorityLabel.Code;

    [Parameter]
    public EventCallback<ClickTaskParameter> TaskClick { get; set; }

    [Parameter]
    public EventCallback<ClickEmptyDayParameter> EmptyDayClick { get; set; }

    [Parameter]
    public EventCallback<DragDropParameter> DragStart { get; set; }

    [Parameter]
    public EventCallback<DragDropParameter> DropTask { get; set; }

    [Parameter]
    public EventCallback<System.DateTime> HeaderClick { get; set; }

    private System.DateTime m = DateTimeUtil.TodayDate;
    private Tasks? TaskDragged;

    private async Task ClickInternal(MouseEventArgs e, System.DateTime day)
    {
        if (day == default) return;

        // There can be several tasks in one day :
        List<int> listID = new();
        if (TasksList != null )
        {
            for (var k = 0; k < TasksList.Length; k++)
            {
                Tasks t = TasksList[k];

                if (t.DateStart.Date <= day.Date && day.Date <= t.DateEnd.Date)
                {
                    listID.Add(t.ID);
                }
            }
        }

        if (listID.Count > 0)
        {
            if (TaskClick.HasDelegate)
			{
				ClickTaskParameter clickTaskParameter = new()
				{
					IDList = listID,
					X = e.ClientX,
					Y = e.ClientY,
					Day = day
				};

				await TaskClick.InvokeAsync(clickTaskParameter);
			}
        }
        else
        {
            if (EmptyDayClick.HasDelegate)
            {

				ClickEmptyDayParameter clickEmptyDayParameter = new()
				{
					Day = day,
					X = e.ClientX,
					Y = e.ClientY
				};

				await EmptyDayClick.InvokeAsync(clickEmptyDayParameter);
			}
        }
    }

    private async Task HandleDragStart(System.DateTime day, int taskID) 
    {
        if (taskID < 0) return;

        TaskDragged = new Tasks()
        {
            ID = taskID
        };

        DragDropParameter dragDropParameter = new()
        {
            Day = day,
            taskID = TaskDragged.ID
        };

        await DragStart.InvokeAsync(dragDropParameter);
    }

    private async Task HandleDayOnDrop(System.DateTime day)
    {
        if (TaskDragged == null) return;

        DragDropParameter dragDropParameter = new()
        {
            Day = day,
            taskID = TaskDragged.ID
        };

        await DropTask.InvokeAsync(dragDropParameter);

        TaskDragged = null;
    }

    private async Task HandleHeaderClick(System.DateTime month)
    {
        if (!HeaderClick.HasDelegate) return;

        await HeaderClick.InvokeAsync(month);
    }
}
