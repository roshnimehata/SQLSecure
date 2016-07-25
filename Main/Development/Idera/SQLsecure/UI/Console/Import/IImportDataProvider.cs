using System.Collections.Generic;
using System.IO;
using Idera.SQLsecure.UI.Console.Import.Models;

namespace Idera.SQLsecure.UI.Console.Import
{
    public interface IImportDataProvider
    {
        List<ImportItem> ParseStream(Stream stream);
    }
}