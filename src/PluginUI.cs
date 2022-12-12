using System;
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
            ImGui.TableNextColumn();
                
            // SetCellColor();
            ImGui.Text(title);
            
            ImGui.TableNextColumn();
            // SetCellColor();
            ImGui.TextWrapped(description);
            
            ImGui.TableNextColumn();
            //SetCellColor();
            ImGui.Text(string.IsNullOrWhiteSpace(starts.Date) ? starts.DateTime.ToString() : starts.Date);
            
            ImGui.TableNextColumn();
            //SetCellColor();
            ImGui.Text(string.IsNullOrWhiteSpace(ends.Date) ? ends.DateTime.ToString() : ends.Date);
            
            ImGui.TableNextRow();
        }

        private void SetCellColor()
        {
            var greenColor = new Vector4(0.0742f, 0.530f, 0.150f, 1.0f);
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, ImGui.ColorConvertFloat4ToU32(greenColor));
        }

        public void Dispose()
        {
        }
    }
}