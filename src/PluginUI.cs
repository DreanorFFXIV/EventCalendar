using System;
using System.Diagnostics;
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
                        var title = eventItem.Summary ?? "No title";
                        var url = eventItem.Location ?? "https://eu.finalfantasyxiv.com/lodestone/";
                        var desc = eventItem.Description ?? "No description";
                        
                        DrawCalendarEntry(title, url, desc, eventItem.Start, eventItem.End);
                    }

                    ImGui.EndTable();
                }

                ImGui.End();
            }
        }

        private void DrawCalendarEntry(string title, string url, string description, EventDateTime starts, EventDateTime ends)
        {
            DateTime start = GetDateTime(starts);
            DateTime end = GetDateTime(ends);
            bool isActive = DateTime.Now >= start && DateTime.Now <= end;

            NextColumn(isActive);
            var blackColor = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            ImGui.PushStyleColor(ImGuiCol.Button, blackColor);
            if (ImGui.Button(title))
            {
                Dalamud.Utility.Util.OpenLink(url);
            }
            
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text("Click to visit link");
                ImGui.EndTooltip();
            }
            
            NextColumn(isActive);
            ImGui.TextWrapped(description);
            
            NextColumn(isActive);
            ImGui.Text(start.Date.ToString(CultureInfo.InvariantCulture));
            
            NextColumn(isActive);
            ImGui.Text(end.Date.ToString(CultureInfo.InvariantCulture));
            
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

            return dateTime.ToLocalTime();
        }
 
        public void Dispose()
        {
        }
    }
}