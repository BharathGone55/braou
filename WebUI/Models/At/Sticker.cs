using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Database;

namespace WebUI.Models.At
{
    public class Sticker
    {
        public string IHTNO1 { get; set; }
        public string SName1 { get; set; }
        public string Subject1 { get; set; }
        public string PaperCode1 { get; set; }
        public string PaperName1 { get; set; }
        public string Semester1 { get; set; }
        public string Medium1 { get; set; }
        public string ExamDate1 { get; set; }
        public string ExamCentreCode1 { get; set; }
        public string ExamCentreName1 { get; set; }
        public string StickerNo1 { get; set; }
        public string Barcode1 { get; set; }
        public DateTime StartTime1 { get; set; }
        public int PrintSeq1 { get; set; }
        public string IHTNO2 { get; set; }
        public string SName2 { get; set; }
        public string Subject2 { get; set; }
        public string PaperCode2 { get; set; }
        public string PaperName2 { get; set; }
        public string Semester2 { get; set; }
        public string Medium2 { get; set; }
        public string ExamDate2 { get; set; }
        public string ExamCentreCode2 { get; set; }
        public string ExamCentreName2 { get; set; }
        public string StickerNo2 { get; set; }
        public string Barcode2 { get; set; }
        public DateTime StartTime2 { get; set; }
        public int PrintSeq2 { get; set; }
        public string IHTNO3 { get; set; }
        public string SName3 { get; set; }
        public string Subject3 { get; set; }
        public string PaperCode3 { get; set; }
        public string PaperName3 { get; set; }
        public string Semester3 { get; set; }
        public string Medium3 { get; set; }
        public string ExamDate3 { get; set; }
        public string ExamCentreCode3 { get; set; }
        public string ExamCentreName3 { get; set; }
        public string StickerNo3 { get; set; }
        public string Barcode3 { get; set; }
        public DateTime StartTime3 { get; set; }
        public int PrintSeq3 { get; set; }
        public static List<Sticker> Get(int eid, string examcentrecode, string examdate = null, string examtime = null, bool islatecomer = false, bool isbuffer = false)
        {
            using (var db = Connection.Create())
            {
                return db.Query<Sticker>("at.formdata_sticker @eid,@examcentrecode,@examdate,@examtime,@islatecomer,@isbuffer", new { eid, examcentrecode, examdate, examtime, islatecomer, isbuffer }).ToList();
            }
        }
    }
}