

namespace IXM.SQL
{
    public interface IEventSQL
    {
        string GetEventsListing(string? pEvtId, string? pFieldName);
        public string GetEventDetailListing(string? pSearchValue, string? pSearchField);
        public string GetEventDetailScan(string? pEventId, string? pSearchValue, string? pSearchField);
        public string GetEventStatsComponent(string? pEvtId);
        public string GetEventDetailComponent(string? pEvtId);
        public string GetEventComponents(string? pEvtId);
        public string GetEventStatsEmpType(string? pEvtId);
        public string GetEventProgramme(string? pEvtId);
    }

}

