using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class CaseModel
    {
        public int CaseId
        {
            get;
            set;
        }
        public int CaseNo
        {
            get;
            set;
        }
        public string PatientFirstName
        {
            get;
            set;
        }
        public string PatientLastName
        {
            get;
            set;
        }
        public string InsuranceName
        {
            get;
            set;
        }
        public string InsuranceAddress
        {
            get;
            set;
        }
        public string InsuranceCity
        {
            get;
            set;
        }
        public string InsuranceState
        {
            get;
            set;
        }
        public string InsuranceZip
        {
            get;
            set;
        }

        public string InsurancePhone
        {
            get;
            set;
        }
        public string InsuranceEmail
        {
            get;
            set;
        }
        public string PatientAddress
        {
            get;
            set;
        }
        public string PatientStreet
        {
            get;
            set;
        }
        public string PatientCity
        {
            get;
            set;
        }
        public string PatientState
        {
            get;
            set;
        }
        public string PatientZip
        {
            get;
            set;
        }
        public string PatientPhone
        {
            get;
            set;
        }
        public string PolicyNumber
        {
            get;
            set;
        }
        public string ClaimNumber
        {
            get;
            set;
        }
        public string BillStatusName
        {
            get;
            set;
        }
        public string AttorneyName
        {
            get;
            set;
        }
        public string AttorneyLastName
        {
            get;
            set;
        }
        public string AttorneyAddress
        {
            get;
            set;
        }
        public string AttorneyCity
        {
            get;
            set;
        }
        public string AttorneyState
        {
            get;
            set;
        }
        public string AttorneyZip
        {
            get;
            set;
        }
        public string AttorneyFax
        {
            get;
            set;
        }
        public string SocialSecurityNo
        {
            get;
            set;
        }


        public string PolicyHolder
        {
            get;
            set;
        }
        public string BillNumber
        {
            get;
            set;
        }
        public string FltBillAmount
        {
            get;
            set;
        }
        public string FltPaid
        {
            get;
            set;
        }
        public string FltBalance
        {
            get;
            set;
        }
        public DateTime? FirstVisitDate
        {
            get;
            set;
        }
        public DateTime? LastVisitDate
        {
            get;
            set;
        }
        public string CaseTypeName
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }
        public string CompanyId
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public string ProviderName
        {
            get;
            set;
        }
        public string ProviderAddress
        {
            get;
            set;
        }
        public string ProviderCity
        {
            get;
            set;
        }
        public string ProviderZip
        {
            get;
            set;
        }
        public string ProviderState
        {
            get;
            set;
        }
        public string ProviderTaxId
        {
            get;
            set;
        }
        public string DoctorTaxId
        {
            get;
            set;
        }
        public string DoctorName
        {
            get;
            set;
        }
        public string Specialty
        {
            get;
            set;
        }
        public DateTime? DateofAccident
        {
            get;
            set;
        }

        //public string Verification
        //{
        //    get;
        //    set;
        //}

        public string AssignedLawFirmId
        {
            get;
            set;
        }

        public string TransferAmount
        {
            get;
            set;
        }
        public DateTime? DateOfTransferred
        {
            get;
            set;
        }
        public DateTime? BillDate
        {
            get;
            set;
        }
        public string Providerid
        {
            get;
            set;
        }
        public string Insurancecompanyid
        {
            get;
            set;
        }
        public string DenialReason1
        {
            get;
            set;
        }
        public string DenialReason2
        {
            get;
            set;
        }
        public string DenialReason3
        {
            get;
            set;
        }
        public string DomainId
        {
            get;
            set;
        }

        public string AtlasProviderId
        {
            get;
            set;
        }
        public int IsDuplicateCase
        {
            get;
            set;
        }

        public string TreatmentDetails { get; set; }

        public string DiagnosisCodes { get; set; }

        public string POMStampDate { get; set; }
        public string POMGenerated { get; set; }
        public string POMid { get; set; }
        public string ReferringDoctorName { get; set; }
        public string ReferringProviderName { get; set; }
        public string ChartNo { get; set; }
    }
}
