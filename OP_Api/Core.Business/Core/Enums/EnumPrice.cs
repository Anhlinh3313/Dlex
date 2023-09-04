using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Core
{
    public enum FormulaPrice
    {
        /// <summary>
        /// Giá chuẩn
        /// </summary>
        StandardPrice = 0,
        /// <summary>
        /// Nhận trọng lượng
        /// </summary>
        MultiplyWeight = 1,
        /// <summary>
        /// Nhân khai giá
        /// </summary>
        MultiplyInsured = 2,
        /// <summary>
        /// Nhân COD
        /// </summary>
        MultiplyCOD = 3,
        /// <summary>
        /// Nhận số kiện
        /// </summary>
        MultiplyPackage = 4,
        /// <summary>
        /// Nhận số lượng sản phẩm
        /// </summary>
        MultiplyProduct = 5,
        /// <summary>
        /// Cộng thêm
        /// </summary>
        PlusPrice = 6,
        /// <summary>
        /// Nhân với khối lượng
        /// </summary>
        MultiplyM3 = 7,
        /// <summary>
        /// % cước chính
        /// </summary>
        PercentDefault = 8,
        /// <summary>
        /// Nhân số kiện
        /// </summary>
        MultiplyBox = 9,
        /// <summary>
        /// Nhân số KM
        /// </summary>
        MultiplyKm = 10
    }
}
