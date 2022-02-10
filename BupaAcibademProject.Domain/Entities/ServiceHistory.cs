using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class ServiceHistory : Entity
    {
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        public string MethodName { get; set; }

        [Required(ErrorMessage = "{0} boş olamaz")]
        public int PolicyId { get; set; }

        [Required(ErrorMessage = "{0} boş olamaz")]
        public DateTime RequestDate { get; set; }

        public string RequestData { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string ResponseData { get; set; }

        public bool IsSuccess { get; set; }

        public ServiceHistory() : base()
        {
            RequestDate = DateTime.Now;
        }

        public ServiceHistory CloneAsNew()
        {
            return new ServiceHistory()
            {
                MethodName = MethodName,
                PolicyId = PolicyId,
                RequestDate = RequestDate,
                RequestData = RequestData,
                ResponseDate = ResponseDate,
                ResponseData = ResponseData,
                IsSuccess = IsSuccess,
                CreateDate = CreateDate,
                UpdateDate = UpdateDate
            };
        }

    }
}
