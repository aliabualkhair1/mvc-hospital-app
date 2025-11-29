using System.ComponentModel.DataAnnotations;

namespace Hospital_Project.Entities.Enums
{
    public enum DoctorRank
    {
        [Display(Name ="طبيب مقيم")]
    طبيب_مقيم =0,
        [Display(Name = "طبيب عام")]
        طبيب_عام = 1,
        [Display(Name = " أخصائى")]
        أخصائى = 2,
        [Display(Name = " أخصائى أول")]
        أخصائى_أول = 3,
        [Display(Name = " إستشارى")]
        إستشارى = 4,
        [Display(Name = " أستاذ دكتور")]
        أستاذ_دكتور = 5
    }
}
