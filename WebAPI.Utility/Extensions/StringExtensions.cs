using System.Collections.Generic;
using WebAPI.Models;
using System;
using System.Xml;
using System.Configuration;


namespace WebAPI.Utility.Extensions
{
    public static class StringExtensions
    {
        //public static object ConfigurationManager { get; private set; }

        public static string GetNodeType(this string NodeType)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("AOB", "A.O.B");
            dic.Add("ALL AOB", "A.O.B");
            dic.Add("VERIFICATION RESPONSE", "VERIFICATION RESPONSE");
            dic.Add("AR 1", "AR 1");
            dic.Add("BILLS", "BILLS");
            dic.Add("PAYMENTS", "PAYMENTS");
            dic.Add("AR 1 PACKET", "AR 1 PACKET");
            dic.Add("DENIALS", "DENIALS");
            dic.Add("GENERAL DENIAL", "DENIALS");
            dic.Add("MEDICAL REPORTS", "MEDICAL REPORTS");
            dic.Add("MEDICALS", "MEDICAL REPORTS");
            dic.Add("FUREPORT", "MEDICAL REPORTS");
            dic.Add("NOTES SCANNED AGAINST THE BILLS", "MEDICAL REPORTS");
            dic.Add("INITIAL REPORT", "MEDICAL REPORTS");
            dic.Add("INTReport", "MEDICAL REPORTS");
            dic.Add("Referrals", "MEDICAL REPORTS");
            dic.Add("PEER REVIEW", "PEER REVIEW");
            dic.Add("POLICE REPORTS", "POLICE REPORTS");
            dic.Add("PROOF OF MAILING", "PROOF OF MAILING");
            dic.Add("UNCATEGORIZED", "UNCATEGORIZED");
            dic.Add("VERIFICATION REQUESTS", "VERIFICATION REQUEST");
            dic.Add("VERIFICATION REQUEST", "VERIFICATION REQUEST");
            dic.Add("AAA CORRESPONDENCE", "AAA CORRESPONDENCE");
            dic.Add("AAA FILE RECEIPT", "AAA FILE RECEIPT");
            dic.Add("Verification Answered", "VERIFIED ANSWER");


            return (dic.ContainsKey(NodeType.ToUpper()) ? dic[NodeType.ToUpper()] : string.Empty);


        }

        public static Dictionary<int, string> GetReasons(this string reason)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();

            try
            {
                DenialReasons d = new DenialReasons();
                string[] denials = reason.Split(',');
                int i = 0;
                if (!string.IsNullOrEmpty(reason))
                {
                    if (denials.Length > 0)
                    {
                        foreach (var item in denials)
                        {
                            dic.Add(i++, item);
                        }
                    }
                }
                return dic;
            }
            catch (Exception ex)
            {
                string s = ex.Message;

                return dic;
            }

        }
        public static DateTime? TryParseNullable(string val)
        {
            DateTime outValue;
            return DateTime.TryParse(val, out outValue) ? (DateTime?)outValue : null;
        }
    }
}
