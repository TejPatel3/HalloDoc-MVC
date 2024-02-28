using HalloDoc.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace Services.ViewModels
{
    public class AdminRequestViewModel
    {
        public List<Request> requests { get; set; }

        [MaybeNull]
        public string BlockNotes { get; set; }
    }
}
