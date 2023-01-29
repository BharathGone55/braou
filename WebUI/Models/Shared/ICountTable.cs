using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Shared
{
    public interface ICountTable
    {
        int Total { get; set; }
        int Present { get; set; }
        int Absent { get; set; }
        int Malpractice { get; set; }
        int Buffer { get; set; }
        int Pending { get; set; }
        int IOPending { get; set; }
        int ScanPending { get; set; }
    }
}