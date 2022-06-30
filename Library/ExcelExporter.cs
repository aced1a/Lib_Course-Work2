using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Library.Model.LibraryEntities;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Win32;

namespace Library
{
    class ExcelExporter
    {
        Excel.Application excelApp;
        Excel._Worksheet worksheet;
        int col = 0, row = 0;
        Dictionary<string,string> ProperiesNames;

        public ExcelExporter()
        {
            ProperiesNames = new Dictionary<string, string>() { 
                { "FirstName", "Имя"},
                {"MiddleName","Имя" },
                {"LastName","Фамилия" },
                {"Name","Название" },
                {"Title","Название" },
                {"Year","" },
                {"ISBN","ISBN" },
                {"Authors","Авторы"},
                {"Genres","Жанры"},
                {"Publishers","Издатели"},
                {"Stories","Рассказы"},
                {"Note","Заметка" },
                {"Description","Описание" }
           };
        }

        string GetPropertyName(string name) 
        {
            if (ProperiesNames.ContainsKey(name))
                return ProperiesNames[name];
            else
                return null;
        }


        public void ToTable<T>(IEnumerable<T> items)
        {
          
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            string text;
            foreach (var property in propertyInfos)
            {
                if ((text = GetPropertyName(property.Name)) != null)
                {
                    worksheet.Cells[row, col].Value = text; col++;
                }
            }
           // col = 1;row++;
            foreach(var item in items)
            {
                col = 1; row++;
                foreach (var property in propertyInfos)
                {
                    if (GetPropertyName(property.Name) != null)
                    {
                        worksheet.Cells[row, col].Value = property.GetValue(item); col++;
                    }
                } 
            }
        }

        async Task<bool> Export<T>(IEnumerable<T> collection)
        {
            return await Task.Run(() =>     {
                excelApp = new Excel.Application();
                excelApp.Workbooks.Add();
                worksheet = excelApp.ActiveSheet;
                try
                {

                    col = 1; row = 1;

                    ToTable(collection);
                    worksheet.Range[worksheet.Cells[1,1], worksheet.Cells[row, col]].AutoFit();
                    return true;
                }
                catch (Exception) { return false; }
            } );
            
        }

        async public void ExportToExcel<T>(IEnumerable<T> collection)
        {
            SaveFileDialog saveFile = new SaveFileDialog() { Filter = "xlsx files(.xlsx)|*.xlsx|All files (*.*)|(*.*)", FilterIndex = 1 };

            bool res = await Export(collection);

            if (saveFile.ShowDialog() == true )
            {
                if (res)
                {
                    try
                    {
                        worksheet.SaveAs(saveFile.FileName);
                    }
                    catch { }
                }
            }
            excelApp.Quit();
        }
    }
}
