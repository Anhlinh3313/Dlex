using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.ExportExcelModel
{
    public class CustomExportFile
    {
        public string LinkLogo { get; set; }

        public string NameReport { get; set; }

        public string FileNameReport { get; set; }

        public List<string> TextList { get; set; }
        public List<ColumnExcelModel> ColumnExcelModels { get; set; }
    }
    public class ColumnExcelModel
    {
        public string Field { get; set; }
        public string Header { get; set; }

        public string TypeFormat { get; set; }
        public string FormatString { get; set; }
        public int FieldSum { get; set; }
        public bool IsSum { get; set; }
    }
}
