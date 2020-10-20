using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessPlatform.Models
{
    public class Ctdatatable
    {
        public tMember Cmember { get; set; }
        public IQueryable<tOrder> Corder { get; set; }
        public IQueryable<tTime> Ctime { get; set; }
        public IQueryable<tRecord> Crecord { get; set; }
        public tCoach Ccoach { get; set; }
    }
}