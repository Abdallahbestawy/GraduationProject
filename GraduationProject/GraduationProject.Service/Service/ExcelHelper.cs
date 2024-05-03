using GraduationProject.Data.Enum;
using GraduationProject.Service.DataTransferObject.FilesDto;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using GraduationProject.Service.IService;
using GraduationProject.Shared.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel.DataAnnotations;

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
    }
}
