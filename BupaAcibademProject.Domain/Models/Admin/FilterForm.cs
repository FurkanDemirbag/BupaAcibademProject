using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Admin
{
    public class FilterForm
    {
        public string InsurerName { get; set; }
        public string InsurerSurname { get; set; }
        public ProximityType ProximityType { get; set; }
        public string TC { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string OfferNumber { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
    }
}
