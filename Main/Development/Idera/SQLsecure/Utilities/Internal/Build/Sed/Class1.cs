using System;
using System.IO;

namespace Idera.SQLcompliance.Utility.Sed
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
		   int retval = 0;
		   
		   if ( args.Length == 4 )
		   {
			   ProcessFile( args[0], args[1], args[2], args[3], false );
		   }
		   else if ( args.Length == 5 && ( args[0].ToUpper().CompareTo("-V") == 0 ) )
         {		        
			   Console.WriteLine ( String.Format( "Processing: {0}", args[1]) );

			   int changed = ProcessFile( args[1], args[2], args[3], args[4], true );
			   
            Console.WriteLine ( String.Format( "Complete: Lines changed: {0}", changed ) ) ;
			}
			else
			{
            Console.WriteLine( "Usage: MySED [-v] inputFile outputFile originalString replacementString" );
            retval = 1;
			}
			
			return retval;
		}
		
      //------------------------------------------------------------------
      // ProcessFile
      //------------------------------------------------------------------
      private static int
         ProcessFile(
            string      inFile,
            string      outFile,
            string      oldText,
            string      newText,
            bool        verbose
         )
      {
        int linesChanged = 0;
        string status = "Unknown";
        
        try
        {
         
            // Create an instance of StreamReader to read from a file.
            status = "Opening input file";   
            using ( StreamReader sread  = new StreamReader( inFile ) )
            {
               status = "Opening output file";   
               using ( StreamWriter swrite = new StreamWriter( outFile,
                                                               false /* overwrite */ ) )
               {                                                            
                  status = "Processing files";
                  
                  String line;
                  
                  // Read and display the lines from the file until the end of the file is reached.
                  
                  while ( ( line = sread.ReadLine()) != null )
                  {
                     string newline = line.Replace( oldText, newText );
                     
                     if ( verbose )
                     {
                        if ( line.CompareTo(newline) != 0 ) linesChanged++;
                     }
                     
                     swrite.WriteLine( newline );
                  }
            
                  status = "Closing files";
            
                  swrite.Close();
               }
               sread.Close();
            }
        }
        catch ( Exception ex )
        {    
            Console.WriteLine( String.Format( "Error processing files during {0}:", status ) );
            Console.WriteLine(ex.Message);
        }
        return linesChanged;
      }
		
	}
}
