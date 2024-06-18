using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.FilesDto;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using GraduationProject.Service.IService;
using GraduationProject.Shared.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;
using OfficeOpenXml.Style;

namespace GraduationProject.Service.Service
{
    public class ExcelHelper : IExcelHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExcelHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public ImportedData<T> Import<T>(string filePath) where T : new()
        {
            ImportedData<T> finalResult = new ImportedData<T>();
            finalResult.ValidationErrors = new();
            XSSFWorkbook workbook;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(stream);
            }

            var sheet = workbook.GetSheetAt(0);

            var rowHeader = sheet.GetRow(0);
            var colIndexList = new Dictionary<string, int>();
            foreach (var cell in rowHeader.Cells)
            {
                var colName = cell.StringCellValue;
                colIndexList.Add(colName, cell.ColumnIndex);
            }

            var listResult = new List<T>();
            var currentRow = 1;
            while (currentRow <= sheet.LastRowNum)
            {
                var row = sheet.GetRow(currentRow);
                if (row == null) break;

                var obj = new T();

                var properties = typeof(T).GetProperties()
                   .Where(prop => prop.CanRead && prop.CanWrite && !prop.IsDefined(typeof(IgnoreFromExcelFIleAttribute), true))
                   .ToList();

                foreach (var property in properties)
                {
                    if (!colIndexList.ContainsKey(property.Name))
                        throw new Exception($"Column {property.Name} not found.");

                    var colIndex = colIndexList[property.Name];
                    var cell = row.GetCell(colIndex);

                    if (cell == null)
                    {
                        property.SetValue(obj, null);
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        cell.SetCellType(CellType.String);
                        property.SetValue(obj, cell.StringCellValue);
                    }
                    else if (property.PropertyType == typeof(int))
                    {
                        cell.SetCellType(CellType.Numeric);
                        property.SetValue(obj, Convert.ToInt32(cell.NumericCellValue));
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        cell.SetCellType(CellType.Numeric);
                        property.SetValue(obj, Convert.ToDecimal(cell.NumericCellValue));
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        property.SetValue(obj, cell.DateCellValue);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        cell.SetCellType(CellType.Boolean);
                        property.SetValue(obj, cell.BooleanCellValue);
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        if (cell.CellType == CellType.Numeric)
                        {
                            if (property.PropertyType.GetEnumUnderlyingType() == typeof(int))
                            {
                                int enumIntValue = (int)cell.NumericCellValue;
                                if (Enum.IsDefined(property.PropertyType, enumIntValue))
                                {
                                    var enumValue = (Enum)Enum.ToObject(property.PropertyType, enumIntValue);
                                    property.SetValue(obj, enumValue);
                                }
                                else
                                {
                                    throw new Exception($"Invalid enum value '{enumIntValue}' for property {property.Name}");
                                }
                            }
                            else if (property.PropertyType.GetEnumUnderlyingType() == typeof(short))
                            {
                                short enumShortValue = (short)cell.NumericCellValue;
                                if (Enum.IsDefined(property.PropertyType, enumShortValue))
                                {
                                    var enumValue = (Enum)Enum.ToObject(property.PropertyType, enumShortValue);
                                    property.SetValue(obj, enumValue);
                                }
                                else
                                {
                                    throw new Exception($"Invalid enum value '{enumShortValue}' for property {property.Name}");
                                }
                            }
                            else
                            {
                                throw new Exception($"Unsupported enum underlying type for property {property.Name}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Cell value is not numeric for property {property.Name}");
                        }
                    }
                    else if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                    {
                        Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);

                        if (cell == null || cell.CellType == CellType.Blank)
                        {
                            property.SetValue(obj, null);
                        }
                        else if (underlyingType == typeof(int))
                        {
                            cell.SetCellType(CellType.Numeric);
                            property.SetValue(obj, Convert.ToInt32(cell.NumericCellValue));
                        }
                        else if (underlyingType == typeof(decimal))
                        {
                            cell.SetCellType(CellType.Numeric);
                            property.SetValue(obj, Convert.ToDecimal(cell.NumericCellValue));
                        }
                        else if (underlyingType == typeof(DateTime))
                        {
                            property.SetValue(obj, cell.DateCellValue);
                        }
                        else if (underlyingType == typeof(bool))
                        {
                            cell.SetCellType(CellType.Boolean);
                            property.SetValue(obj, cell.BooleanCellValue);
                        }
                        else if (underlyingType.IsEnum)
                        {
                            if (cell.CellType == CellType.Numeric)
                            {
                                if (underlyingType.GetEnumUnderlyingType() == typeof(int))
                                {
                                    int enumIntValue = (int)cell.NumericCellValue;
                                    if (Enum.IsDefined(underlyingType, enumIntValue))
                                    {
                                        var enumValue = (Enum)Enum.ToObject(underlyingType, enumIntValue);
                                        property.SetValue(obj, enumValue);
                                    }
                                    else
                                    {
                                        throw new Exception($"Invalid enum value '{enumIntValue}' for property {property.Name}");
                                    }
                                }
                                else if (underlyingType.GetEnumUnderlyingType() == typeof(short))
                                {
                                    short enumShortValue = (short)cell.NumericCellValue;
                                    if (Enum.IsDefined(underlyingType, enumShortValue))
                                    {
                                        var enumValue = (Enum)Enum.ToObject(underlyingType, enumShortValue);
                                        property.SetValue(obj, enumValue);
                                    }
                                    else
                                    {
                                        throw new Exception($"Invalid enum value '{enumShortValue}' for property {property.Name}");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"Unsupported enum underlying type for property {property.Name}");
                                }
                            }
                            else
                            {
                                throw new Exception($"Cell value is not numeric for property {property.Name}");
                            }
                        }
                        else
                        {
                            property.SetValue(obj, Convert.ChangeType(cell.StringCellValue, underlyingType));
                        }
                    }

                }

                var validationResult = Validate(obj);
                if (validationResult.Count == 0)
                {
                    listResult.Add(obj);
                    currentRow++;
                }
                else if (validationResult.Count > 0)
                {
                    var index = currentRow;
                    finalResult.ValidationErrors.Add(new ValidationErrorsModel
                    {
                        Inedx = ++index,
                        Errors = validationResult
                    });
                    currentRow++;
                }
            }

            finalResult.MappedData = listResult;
            return finalResult;
        }

        public ImportedData<List<PhoneNumberDto>> ImportFromSpecificColumn(string filePath, string colName)
        {
            ImportedData<List<PhoneNumberDto>> finalResult = new ImportedData<List<PhoneNumberDto>>();
            finalResult.ValidationErrors = new();
            finalResult.MappedData = new();
            XSSFWorkbook workbook;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(stream);
            }

            var sheet = workbook.GetSheetAt(0);

            var rowHeader = sheet.GetRow(0);
            var colIndex = -1;
            foreach (var cell in rowHeader.Cells)
            {
                var headerColName = cell.StringCellValue;
                if (headerColName == colName)
                {
                    colIndex = cell.ColumnIndex;
                    break;
                }
            }

            if (colIndex == -1)
            {
                throw new Exception($"Column '{colName}' not found in the Excel sheet.");
            }

            var currentRow = 1;
            while (currentRow <= sheet.LastRowNum)
            {
                var listResult = new List<PhoneNumberDto>();
                var row = sheet.GetRow(currentRow);
                if (row == null) break;

                var cell = row.GetCell(colIndex);

                if (cell != null)
                {
                    var phoneNumber = typeof(PhoneNumberDto).GetProperty("PhoneNumber");
                    var type = typeof(PhoneNumberDto).GetProperty("Type");

                    string cellVal = cell.StringCellValue;
                    string[] phones = cellVal.Split(", ");
                    foreach (string phone in phones)
                    {
                        PhoneNumberDto obj = new PhoneNumberDto();
                        if (phone.Length == 0) continue;

                        if(phone.Length > 1)
                        {
                            var phoneType = phone.Substring(0, 1);
                            if (phoneType == "P")
                                type.SetValue(obj, PhoneType.Phone);
                            else if (phoneType == "M")
                                type.SetValue(obj, PhoneType.Mobile);
                            string phoneNum = phone.Substring(1, phone.Length - 1);
                            phoneNumber.SetValue(obj,phoneNum);

                            var validationResult = Validate(obj);
                            if (validationResult.Count > 0)
                            {
                                var index = currentRow;
                                finalResult.ValidationErrors.Add(new ValidationErrorsModel
                                {
                                    Inedx = ++index,
                                    Errors = validationResult
                                });
                            }
                        }
                        listResult.Add(obj);
                    }
                }
                currentRow++;
                finalResult.MappedData.Add(listResult);
            }
            return finalResult;
        }

        public string SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(file.FileName);

            var webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var folderPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}.{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return filePath;
        }

        private List<ErrorsList> Validate<T>(T obj)
        {
            List<ErrorsList> errors = new();

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
            foreach (var item in results)
            {
                errors.Add(new ErrorsList { ErrorMessage = item});
            }

            return errors;
        }

        public async Task<MemoryStream> GenerateExcelFileForAssessMethodsAsync(string courseName, string courseCode,
            string lecturerName, string extractorName, List<Dictionary<string, object>> students, List<string> assessMethods)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var headers = new List<string> { "N.", "Student Code", "Student Name" };
                headers.AddRange(assessMethods);
                string lastColumn = GetExcelColumnName(headers.Count);

                var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                sheet.Cells[$"A1:{lastColumn}2"].Merge = true;
                sheet.Cells[$"A3:{lastColumn}3"].Merge = true;
                sheet.Cells[$"A4:{lastColumn}4"].Merge = true;

                sheet.Cells["A1"].Value = $"{courseName} - {courseCode}";
                sheet.Cells["A3"].Value = $"Lecturer: {lecturerName}";
                sheet.Cells["A4"].Value = $"EduWay Extracted at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} by {extractorName}";

                for (int i = 1; i <= 4; i++)
                {
                    sheet.Cells[$"A{i}:F{i}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A{i}:F{i}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A{i}:F{i}"].Style.Font.Bold = true;
                }

                for (int i = 0; i < headers.Count; i++)
                {
                    var cell = sheet.Cells[5, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    var border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                int row = 6;
                foreach (var student in students)
                {
                    for (int i = 0; i < headers.Count; i++)
                    {
                        var header = headers[i];
                        var cell = sheet.Cells[row, i + 1];
                        cell.Value = student.ContainsKey(header) ? student[header] : string.Empty;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    row++;
                }

                sheet.Cells.AutoFitColumns();

                excelPackage.Workbook.Properties.Title = "Sample Excel Report";
                excelPackage.Workbook.Properties.Author = "Fahem";
                excelPackage.Workbook.Properties.Subject = "Student Performance Report";
                excelPackage.Workbook.Properties.Keywords = "Excel, Report, Students";
                excelPackage.Workbook.Properties.Comments = "This is a sample Excel report generated using EPPlus.";

                var memoryStream = new MemoryStream();
                await excelPackage.SaveAsAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        public List<Dictionary<string, object>> ReadExcelFileForAssessMethods(Stream excelStream)
        {
            var students = new List<Dictionary<string, object>>();

            using (var excelPackage = new ExcelPackage(excelStream))
            {
                var sheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (sheet == null) return students;

                var headers = new List<string>();
                int headerRowIndex = 5;

                for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                {
                    var headerValue = sheet.Cells[headerRowIndex, col].Text;
                    if (!string.IsNullOrEmpty(headerValue))
                    {
                        headers.Add(headerValue);
                    }
                    else
                    {
                        break;
                    }
                }

                for (int row = headerRowIndex + 1; row <= sheet.Dimension.End.Row; row++)
                {
                    var studentDict = new Dictionary<string, object>();
                    for (int col = 1; col <= headers.Count; col++)
                    {
                        studentDict[headers[col - 1]] = sheet.Cells[row, col].Text;
                    }
                    students.Add(studentDict);
                }
            }

            return students;
        }
    }
}
