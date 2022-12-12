using System;
using System.Globalization;
using System.Numerics;
using Google.Apis.Calendar.v3.Data;
using ImGuiNET;

namespace EventCalendar
{
    public class PluginUI : IDisposable
    {
        // this extra bool exists for ImGui, since you can't ref a property
        private bool _visible;

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        private readonly Events _events;
        
        public PluginUI()
        {
            var calendarApi = new CalendarAPI();
            _events = calendarApi.GetEvents();
        }

        public void Draw()
        {
            if (!Visible)
            {
                return;
            }
            
            ImGui.SetNextWindowSize(new Vector2(1000, 500), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Events", ref _visible, ImGuiWindowFlags.None))
            {
                ImGui.Spacing();

                if (ImGui.BeginTable("table", 4, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.BordersInner))
                {
                    ImGui.TableHeader("Events");
                    ImGui.TableSetupColumn("Title", ImGuiTableColumnFlags.WidthFixed);
                    ImGui.TableSetupColumn("Description", ImGuiTableColumnFlags.WidthStretch);
                    ImGui.TableSetupColumn("Starts", ImGuiTableColumnFlags.WidthFixed);
                    ImGui.TableSetupColumn("Ends", ImGuiTableColumnFlags.WidthFixed);
                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    foreach (var eventItem in _events.Items)
                    {
                        DrawCalendarEntry(eventItem.Summary, eventItem.Description, eventItem.Start, eventItem.End);
                    }

                    ImGui.EndTable();
                }

                ImGui.End();
            }
        }

        private void DrawCalendarEntry(string title, string description, EventDateTime starts, EventDateTime ends)
        {
            DateTime start = GetDateTime(starts);
            DateTime end = GetDateTime(ends);
            bool isActive = DateTime.Now >= start && DateTime.Now <= end;

            NextColumn(isActive);
            ImGui.Text(title);
            
            NextColumn(isActive);
            ImGui.TextWrapped(description);
            
            NextColumn(isActive);
            ImGui.Text(start.ToString(CultureInfo.InvariantCulture));
            
            NextColumn(isActive);
            ImGui.Text(start.ToString(CultureInfo.InvariantCulture));
            
            ImGui.TableNextRow();
        }

        private void NextColumn(bool isActive)
        {
            ImGui.TableNextColumn();
            var vector = isActive
                ? new Vector4(0.369f, 0.729f, 0.49f, 1.0f)
                : new Vector4(0.933f, 0.247f, 0.267f, 1.0f);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, ImGui.ColorConvertFloat4ToU32(vector));
        }

        private DateTime GetDateTime(EventDateTime eventDateTime)
        {
            DateTime dateTime;
            if (eventDateTime.DateTime.HasValue)
            {
                dateTime = (DateTime)eventDateTime.DateTime;
            }
            else
            {
                dateTime = DateTime.ParseExact(eventDateTime.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return dateTime;
        }
        
        public void Dispose()
        {
        }
    }
}