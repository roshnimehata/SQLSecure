using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Idera.SQLsecure.UI.Console.Import.Models;

namespace Idera.SQLsecure.UI.Console.Import
{
    public class CsvDataProvider : IImportDataProvider
    {
        public List<ImportItem> ParseStream(Stream stream)
        {
            var result = new List<ImportItem>();
            using (var reader = new StreamReader(stream))
            {
                var parser = new CsvParser(reader);
                result.AddRange(parser.Parse<ImportItem>());
            } 
            
            return result;
            
        } 
    }
}
