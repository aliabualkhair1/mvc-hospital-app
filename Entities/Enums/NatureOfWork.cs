using System.ComponentModel.DataAnnotations;
namespace Hospital_Project.Entities.Enums
{
    public enum NatureOfWork
    {
        [Display(Name = "موظف استقبال")]
        موظف_استقبال,
        [Display(Name = "فني مختبر")]
        فني_مختبر,

        [Display(Name = "فني أشعة")]
        فني_أشعة,

        [Display(Name = "موظف سجلات طبية")]
        موظف_سجلات_طبية,

        [Display(Name = "محاسب")]
        محاسب,

        [Display(Name = "فني تعقيم")]
        فني_تعقيم,

        [Display(Name = "عامل نظافة")]
        عامل_نظافة,

        [Display(Name = "مسؤول أمن")]
        مسؤول_أمن,

        [Display(Name = "مدير موارد بشرية")]
        مدير_موارد_بشرية
    }

}
