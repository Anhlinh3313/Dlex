using ClosedXML.Excel;
using Core.Business.ViewModels;
using Core.Business.ViewModels.ExportExcelModel;
using Core.Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Google.Api.Distribution.Types;

namespace Core.Api.Library
{
    public class ExportExcelPartern
    {
        public static dynamic ExportExcel(CustomExportFile filterViewModel, DataTable dtt)
        {
            List<int> Columnnumber = new List<int>();
            List<int> Columndatetime = new List<int>();
            List<string> Columndatetimestring = new List<string>();
            //total value row
            DataTable OneRow = dtt.Clone();
            for (int i = 0; i < OneRow.Columns.Count; i++)
            {
                OneRow.Columns[i].ColumnName = OneRow.Columns[i].ColumnName.ToLower();
            }
            OneRow.Rows.Add(dtt.Rows[0].ItemArray);
            //
            Image img = null;
            DataTable NewDtt = new DataTable();

            if (filterViewModel.ColumnExcelModels.Count() > 0)
            {
                foreach (var item in filterViewModel.ColumnExcelModels)
                {
                    if (!Util.IsNull(item.TypeFormat))
                    {
                        if (item.TypeFormat.ToLower() == "number")
                        {
                            NewDtt.Columns.Add(item.Header, typeof(String));
                        }
                        else if (item.TypeFormat.ToLower() == "date")
                        {
                            NewDtt.Columns.Add(item.Header, typeof(DateTime));
                        }
                        else if (item.TypeFormat.ToLower() == "time")
                        {
                            NewDtt.Columns.Add(item.Header, typeof(String));
                        }
                        else
                        {
                            NewDtt.Columns.Add(item.Header, typeof(String));
                        }
                    }
                    else
                    {
                        NewDtt.Columns.Add(item.Header, typeof(String));
                    }
                }
            }
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                NewDtt.Rows.Add(NewDtt.NewRow());
                NewDtt.AcceptChanges();
            }
            /// if field sum > 0 add row
            if (filterViewModel.ColumnExcelModels[0].FieldSum > 0)
            {
                NewDtt.Rows.Add(NewDtt.NewRow());
                NewDtt.AcceptChanges();
            }
            double[] sum = new double[dtt.Columns.Count];
            int indexSum = 0;
            if (filterViewModel.ColumnExcelModels.Count() > 0)
            {
                int CountIndexColumn = 0;
                int CountColumn = dtt.Columns.Count;

                //change column name , and custom position in file
                foreach (var item in filterViewModel.ColumnExcelModels)
                {
                    //dtt.Columns.Contains("");
                    for (int i = 0; i < CountColumn; i++)
                    {
                        if (item.Field.ToUpper() == dtt.Columns[i].ColumnName.ToUpper())
                        {
                            if (!Util.IsNull(item.TypeFormat))
                            {
                                if (item.TypeFormat.ToLower() == "number")
                                {

                                    Columnnumber.Add(CountIndexColumn);
                                    for (int j = 0; j < dtt.Rows.Count; j++)
                                    {
                                        /// isSum = true
                                        if (item.IsSum)
                                        {
                                            sum[i] += double.Parse(dtt.Rows[j][i].ToString());
                                            indexSum = i;
                                        }
                                        try
                                        {
                                            var a = dtt.Rows[j][i];
                                            NewDtt.Rows[j][CountIndexColumn] = double.Parse(dtt.Rows[j][i].ToString()); 
                                        }
                                        catch (Exception ex)
                                        {
                                            NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i];
                                        }
                                    }
                                    if (item.IsSum)
                                    {
                                        NewDtt.Rows[dtt.Rows.Count][indexSum] = sum[indexSum];
                                    }
                                }
                                else if (item.TypeFormat.ToLower() == "date" && !Util.IsNull(item.FormatString))
                                {
                                    Columndatetime.Add(CountIndexColumn);
                                    Columndatetimestring.Add(item.FormatString);
                                    for (int j = 0; j < dtt.Rows.Count; j++)
                                    {
                                        try
                                        {
                                            if (dtt.Rows[j][i].ToString() != "")
                                            {
                                                string value = Convert.ToDateTime(dtt.Rows[j][i]).ToString(item.FormatString);
                                                var formatInfo = new DateTimeFormatInfo()
                                                {
                                                    ShortDatePattern = item.FormatString
                                                };
                                                DateTime timevalue = Convert.ToDateTime(value, formatInfo);
                                                NewDtt.Rows[j][CountIndexColumn] = timevalue;
                                            }
                                            else
                                            {
                                                NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i];
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i];
                                        }
                                    }
                                }
                                else if (item.TypeFormat.ToLower() == "time" && !Util.IsNull(item.FormatString))
                                {
                                    for (int j = 0; j < dtt.Rows.Count; j++)
                                    {
                                        try
                                        {
                                            if (dtt.Rows[j][i].ToString() != "")
                                            {
                                                var a = dtt.Rows[j][i].ToString();
                                                NewDtt.Rows[j][CountIndexColumn] = DateTime.Parse(a).ToString(item.FormatString);
                                            }
                                            else
                                            {
                                                NewDtt.Rows[j][CountIndexColumn] = "";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < dtt.Rows.Count; j++)
                                    {
                                        try
                                        {
                                            NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i].ToString();
                                        }
                                        catch
                                        {
                                            NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i];
                                        }
                                    }
                                }
                            }
                            else
                            {

                                for (int j = 0; j < dtt.Rows.Count; j++)
                                {
                                    try
                                    {
                                        NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i].ToString();
                                    }
                                    catch
                                    {
                                        NewDtt.Rows[j][CountIndexColumn] = dtt.Rows[j][i].ToString();
                                    }
                                }
                            }

                        }

                    }
                    CountIndexColumn++;
                }
            }
            // sheet name
            string export = "Sheet1";
            //Fill datatable
            //dt = *something *


            int RowStartDataTable = 1;

            //convert datatable to byte[]
            byte[] fileContents;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(export);

                if (filterViewModel.LinkLogo != null)
                {
                    try
                    {
                        WebRequest req = WebRequest.Create(filterViewModel.LinkLogo);
                        WebResponse response = req.GetResponse();
                        Stream stream = response.GetResponseStream();
                        img = Image.FromStream(stream);
                        stream.Close();
                        var FortmatImg = worksheet.Drawings.AddPicture("logo", img);
                        int imawidth = (img.Width / img.Height);
                        string[] sizeheight = (0.6 * 96).ToString().Split(".");
                        int height = int.Parse(sizeheight[0]);
                        FortmatImg.SetPosition(0, 0);
                        FortmatImg.SetSize(imawidth * height, height);
                        RowStartDataTable = RowStartDataTable + 3;
                    }
                    catch
                    {
                        var Imagedefualt = Image.FromFile(Environment.CurrentDirectory + "//Image//ImageDefault.png");
                        var FortmatImg = worksheet.Drawings.AddPicture("logo", Imagedefualt);
                        int imawidth = (Imagedefualt.Width / Imagedefualt.Height);
                        string[] sizeheight = (0.6 * 96).ToString().Split(".");
                        int height = int.Parse(sizeheight[0]);
                        FortmatImg.SetPosition(0, 0);
                        FortmatImg.SetSize(imawidth * height, height);
                        RowStartDataTable = RowStartDataTable + 3;
                    }


                }
                if (filterViewModel.NameReport != null)
                {
                    worksheet.Cells["A" + (RowStartDataTable)].LoadFromText(filterViewModel.NameReport);
                    RowStartDataTable++;
                }
                if (filterViewModel.TextList != null)
                {
                    foreach (var item in filterViewModel.TextList)
                    {
                        if (item != null)
                        {
                            worksheet.Cells["A" + (RowStartDataTable)].LoadFromText(item);
                            RowStartDataTable++;
                        }
                    }
                }
                //load data
                var table = worksheet.Cells["A" + RowStartDataTable].LoadFromDataTable(NewDtt, true);
                table.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                table.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                table.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                table.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                table.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                table.Style.Numberformat.Format = "@";

                foreach (var item in Columnnumber)
                {
                    foreach (var cell in worksheet.Cells[RowStartDataTable + 1, item + 1, NewDtt.Rows.Count + RowStartDataTable, item + 1])
                    {
                        try
                        {
                            cell.Style.Numberformat.Format = "#,##0";
                            cell.Value = Double.Parse(cell.Value + "");
                        }
                        catch
                        {

                        }

                    }
                }
                for (int i = 0; i < Columndatetimestring.Count; i++)
                {
                    foreach (var cell in worksheet.Cells[RowStartDataTable + 1, Columndatetime[i] + 1, NewDtt.Rows.Count + RowStartDataTable, Columndatetime[i] + 1])
                    {
                        try
                        {
                            Debug.WriteLine(cell.Value);
                            if (cell.Value != null)
                            {
                                cell.Style.Numberformat.Format = Columndatetimestring[i];
                            }
                            else
                            {
                                cell.Value = null;
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                package.Save();
                fileContents = package.GetAsByteArray();
            }
            if (fileContents == null || fileContents.Length == 0)
            {
                return null;
            }
            return fileContents;
        }
        private static void NewDtt_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static bool CheckTimeExport()
        {
            bool ischecktime = true;
            TimeSpan MorningStart = new TimeSpan(7, 30, 00);
            TimeSpan MorningEnd = new TimeSpan(9, 30, 00);
            TimeSpan affternoonStart = new TimeSpan(11, 30, 00);
            TimeSpan affternoonEnd = new TimeSpan(14, 30, 00);
            TimeSpan EveningStart = new TimeSpan(16, 30, 00);
            TimeSpan EveningEnd = new TimeSpan(18, 00, 00);
            if (DateTime.Now.TimeOfDay >= MorningStart && DateTime.Now.TimeOfDay <= MorningEnd)
            {

                if (DateTime.Now.TimeOfDay >= affternoonStart && DateTime.Now.TimeOfDay <= affternoonEnd)
                {
                    if (DateTime.Now.TimeOfDay >= EveningStart && DateTime.Now.TimeOfDay <= EveningEnd)
                    {
                        ischecktime = false;
                    }

                }
            }
            return ischecktime;
        }
        public static bool CheckTotalData(int total)
        {
            if (total > 10000)
            {
                return false;
            }
            return true;
        }
        //public static string FormatNumber(double number)
        //{
        //    number.ToString("#,#", CultureInfo.InvariantCulture);
        //    String.Format(CultureInfo.InvariantCulture,
        //                        "{0:#,#}", number);
        //    return number;
        //}
    }
}
