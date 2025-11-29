using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.Enums
{

        public enum Specialization
        {
        [Display(Name = "باطنة")]
        باطنة,

        [Display(Name = "أطفال")]
        أطفال,

        [Display(Name = "نساء وتوليد")]
        نساء_وتوليد,

        [Display(Name = "عظام")]
        عظام,

        [Display(Name = "جلدية")]
        جلدية,

        [Display(Name = "أنف وأذن")]
        أنف_وأذن,

        [Display(Name = "عيون")]
        عيون,

        [Display(Name = " قلب وأوعية دموية")]
        قلب_أوعية_دموية,

        [Display(Name = "أسنان")]
        أسنان,

        [Display(Name = " مخ وأعصاب")]
         مخ_أعصاب ,

        [Display(Name = "نفسية")]
        نفسية,

        [Display(Name = "مسالك بولية")]
        مسالك_بولية,

        [Display(Name = "جراحة")]
        جراحة,

        [Display(Name = "جهاز هضمي")]
        جهاز_هضمي,

        [Display(Name = "صدرية")]
        صدرية,

        [Display(Name = "غدد صماء")]
        غدد_صماء
    }

}
