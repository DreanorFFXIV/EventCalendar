using System;
using ImGuiNET;

namespace EventCalendar
{
    public class PluginUI : IDisposable
    {
        private const string PublicCalendarId =
            "5de3023ee82a703ccd7c4b71f0e34418967b28f0001cdb722cb261e11d51dc8d@group.calendar.google.com";
        
        // this extra bool exists for ImGui, since you can't ref a property
        private bool _visible;

        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        public PluginUI()
        {
        }

        public void Draw()
        {
            DrawCalendarEntry("title", "desc", "url");
        }

        private void DrawCalendarEntry(string title, string description, string imgUrl)
        {
            ImGui.TableNextColumn();

            if (description.Length > 200)
            {
                description = description[..200] + "...";
            }

            ImGui.TextWrapped(description);

            ImGui.TableNextRow();
        }

        public void Dispose()
        {
        }
    }
}