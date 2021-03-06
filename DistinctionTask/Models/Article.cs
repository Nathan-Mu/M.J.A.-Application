//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DistinctionTask.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public partial class Article
    {
        [Display(Name = "Article ID")]
        public int ArticleId { get; set; }
        
        [Required(ErrorMessage = "Title cannot be empty.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Publish date cannot be empty.")]
        [Display(Name = "Publish Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Historical(ErrorMessage = "Publish date should be a historical date")]
        public System.DateTime PubDate { get; set; }

        [Required(ErrorMessage = "Text cannot be empty.")]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Journalist ID")]
        public string JournalistId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }

        public class Historical : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value == null)
                    return true;
                else
                {
                    return (DateTime)value <= DateTime.Today;
                }

            }
        }
    }
}
