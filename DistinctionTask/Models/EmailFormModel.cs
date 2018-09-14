using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DistinctionTask.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Subject")]
        public string Subject { get; set; }
        [Required, Display(Name = "Emails")]
        public string ToEmails { get; set; }
        [Required]
        public string Message { get; set; }
    }
}