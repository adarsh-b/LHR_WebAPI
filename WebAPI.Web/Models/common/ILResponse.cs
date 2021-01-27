using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Web.Models.common
{
    public class ILResponse
    {
        public string MessageCode { set; get; }
        public string MessageText { set; get; }
        public EnumMessageType MessageType { set; get; }
        public string TransactionID { set; get; }
        public bool HasException { set; get; }
        public string Exception { set; get; }
        public string UpdatedDate { set; get; }

        private List<Object> oData;
        public List<Object> Data
        {
            get
            {
                return oData;
            }
            set
            {
                oData = value;
            }
        }

        public void Add(Object p_Object)
        {
            if (oData == null)
            {
                oData = new List<Object>();
            }
            oData.Add(p_Object);
        }
    }

    public enum EnumMessageType
    {
        OPERATION_SUCCESS = 1, OPERATION_TECHNICAL_ERROR = 2, OPERATION_APPLICATION_ERROR = 3
    }
}