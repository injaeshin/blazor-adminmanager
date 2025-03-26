using System.Data;
using ExcelDataReader;

namespace AdminManager.Accessors;
internal class ExcelReadAccessor : IDisposable
{
    private const bool UseHeaderRow = true;
    private const bool SkipSecondRow = false;

    private DataSet _dataSet;
    private IExcelDataReader _dataReader;

    public ExcelReadAccessor()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public bool Open(string file)
    {
        _dataSet?.Clear();

        try
        {
            var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _dataReader = ExcelReaderFactory.CreateReader(stream);
        }
        catch (Exception e)
        {
            Close();

            Console.WriteLine(e.ToString());

            throw;
        }

        return true;
    }

    private void Close()
    {
        _dataSet?.Clear();
        _dataSet?.Dispose();
        _dataSet = null;
        //_stream?.Dispose();
        //_stream = null;
        _dataReader?.Dispose();
        _dataReader = null;
    }

    public void Dispose()
    {
        Close();
    }

    public IEnumerable<string> GetNames()
    {
        _dataSet ??= ToDataSet();

        for (var i = 0; i < _dataSet?.Tables.Count; i++)
        {
            yield return _dataSet.Tables[i].TableName;
        }
    }

    private DataSet ToDataSet()
    {
        if (_dataReader == null)
            return null;

        var conf = new ExcelDataSetConfiguration()
        {
            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration
            {
                UseHeaderRow = UseHeaderRow,
                EmptyColumnNamePrefix = "EmptyColumn",
                FilterRow = (rowReader) => rowReader.Depth > (SkipSecondRow ? 1 : 0)
            }
        };

        return _dataReader.AsDataSet(conf);
    }

    public DataTable ToDataTable(int sheetIndex)
    {
        if (_dataSet != null)
        {
            return _dataSet.Tables[sheetIndex];
        }

        _dataSet = ToDataSet();
        return _dataSet?.Tables.Count == 0 ? null : _dataSet?.Tables[sheetIndex];
    }

    public DataTable ToDataTable(string sheetName)
    {
        if (_dataSet != null)
        {
            return _dataSet.Tables[sheetName];
        }

        _dataSet = ToDataSet();
        return _dataSet?.Tables.Count == 0 ? null : _dataSet.Tables[sheetName];
    }


}
