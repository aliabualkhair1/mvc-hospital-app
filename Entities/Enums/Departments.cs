using System.ComponentModel.DataAnnotations;

public enum Departments
{
    [Display(Name = "الطوارئ")]
    الطوارئ = 1,

    [Display(Name = "الباطنة")]
    الباطنة,

    [Display(Name = "الجراحة العامة")]
    الجراحة_العامة,

    [Display(Name = "العظام")]
    العظام,

    [Display(Name = "الأطفال")]
    الأطفال,

    [Display(Name = "النساء والتوليد")]
    النساء_والتوليد,

    [Display(Name = "القلب")]
    القلب,

    [Display(Name = "المخ والأعصاب")]
    المخ_والأعصاب,

    [Display(Name = "الجلدية")]
    الجلدية,

    [Display(Name = "المسالك البولية")]
    المسالك_البولية,

    [Display(Name = "الأنف والأذن والحنجرة")]
    الأنف_والأذن_والحنجرة,

    [Display(Name = "العيون")]
    العيون,

    [Display(Name = "الأورام")]
    الأورام,

    [Display(Name = "التخدير")]
    التخدير,

    [Display(Name = "العناية المركزة")]
    العناية_المركزة
}
