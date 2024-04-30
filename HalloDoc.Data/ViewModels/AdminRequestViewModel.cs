using HalloDoc.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace Services.ViewModels
{
    public class AdminRequestViewModel
    {
        public List<Request> requests { get; set; }
        public List<Region> regions { get; set; }
        public List<Physician> physicians { get; set; }
        public List<RequestStatusLog> requestStatusLogList { get; set; }
        public List<string> TransferNoteList { get; set; }
        public string patientName { get; set; }
        [MaybeNull]
        public string BlockNotes { get; set; }
        public int requestid { get; set; }

        public List<CaseTag> caseTags { get; set; }

        public string AdminNameViewNotes { get; set; }

        public string PhsysicianNameViewNotes { get; set; }

        public DateTime assigntime { get; set; }
        public int PhysicianId { get; set; }

    }
}

