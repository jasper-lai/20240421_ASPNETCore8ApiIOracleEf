namespace ASPNETCore8ApiIOracleEf.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 員工基本資料
    /// </summary>
    public class EmployeeViewModel
    {
        /// <summary>
        /// 員工編號
        /// </summary>
        [Display(Name = "員工編號")]
        public decimal Id { get; set; }

        /// <summary>
        /// 員工姓名
        /// </summary>
        [Display(Name = "員工姓名")]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 員工住址
        /// </summary>
        [Display(Name = "員工住址")]
        public string? Address { get; set; } = string.Empty;

        /// <summary>
        /// 員工照片
        /// </summary>
        [Display(Name = "員工照片")]
        public byte[]? Photo { get; set; }
    }
}
