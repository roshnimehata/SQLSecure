using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Idera.Common.ReportsInstaller.ErrorLogging;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// Validates the SSL certificate.
	/// </summary>
	public class SslValidation : ICertificatePolicy
	{
		/// <summary>
		/// Gives the model access to certain methods in the view.
		/// </summary>
		private IViewAccessAdapter _viewAdapter;

		/// <summary>
		/// Whether or not the certificate has been validated yet.
		/// </summary>
		private bool _validated = false;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="viewAdapter">The adapter that give the model access
		/// to parts of the view.</param>
		public SslValidation(IViewAccessAdapter viewAdapter)
		{
			ErrorLog.Singleton.LogSuccess("Entered SslValidation constructor.");
			_viewAdapter = viewAdapter;
			ErrorLog.Singleton.LogSuccess("Exited SslValidation constructor.");
		}
		
		/// <summary>
		/// Implement CheckValidationResult to ignore problems that we are willing to accept.
		/// </summary>
		/// <param name="sp">The http connection.</param>
		/// <param name="cert">The SSL Certificate in a warper class.</param> 
		/// <param name="request">The request to the URI.</param>
		/// <param name="problem">The integer representing the problem associated with
		/// the SSL Certificate (0 if no problem)</param>
		/// <returns>True if a valid certificate; false if not.</returns>
		public bool CheckValidationResult(ServicePoint sp, X509Certificate cert,
			WebRequest request, int problem)
		{ 
			if (problem == 0)
			{
				return true;
			}
			else if (_validated)
			{
				return true;
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("Do you trust this SSL Certificate?\r\n");
				sb.Append("Effective Date: ");
				sb.Append(cert.GetEffectiveDateString());
				sb.Append("\r\n");
				sb.Append("Expiration Date: ");
				sb.Append(cert.GetExpirationDateString());
				sb.Append("\r\n");
				sb.Append("Issued By: ");
				sb.Append(Parser(cert.GetIssuerName()));
				sb.Append("\r\n");
				sb.Append("Issued To: ");
				sb.Append(Parser(cert.GetName()));
				bool check = _viewAdapter.DisplayMessageBoxYesNo(sb.ToString(), "SSL Certificate");
				if (check)
				{
					_validated = true;
				}
				return check;
			}
		}

		/// <summary>
		/// Parses the certification information into something the user can understand.
		/// </summary>
		/// <param name="certInfo">The certification information.</param>
		private string Parser(string certInfo)
		{
			char[] chars = certInfo.ToCharArray();
			StringBuilder sb = new StringBuilder();
			int startIndex = 0;
			int endIndex = 0;
			while (endIndex <= chars.Length-1)
			{
				// looking of variable type before the '=' such as the 'O' in 'O=Idera'
				while (chars[endIndex] != '=')
				{
					endIndex++;
				}
				char[] temp = new char[endIndex-startIndex+1];
				for(int i = 0; i < temp.Length; i++)
				{
					temp[i] = chars[startIndex];
					startIndex++;
				}
				sb.Append(ParserHelper(new String(temp)));
				endIndex++;

				// looking for the information set to each variable such as the 'Idera'
				// in 'O=Idera'
				while ((chars[endIndex] != ',') && endIndex < chars.Length-1)
				{
					endIndex++;
				}
				char[] temp2 = new char[endIndex-startIndex+1];
				for(int i = 0; i < temp2.Length; i++)
				{
					temp2[i] = chars[startIndex];
					startIndex++;
				}
				sb.Append(new String(temp2));
				endIndex++;
				if (startIndex < chars.Length)
				{
					sb.Append(chars[startIndex]);
					startIndex++;
					endIndex++;
				}
			}
			return sb.ToString();

		}

		/// <summary>
		/// Identifies the variable type in the certification information.
		/// </summary>
		/// <param name="certInfoHeader">The variable type.</param>
		private string ParserHelper(string certInfoHeader)
		{
			switch(certInfoHeader)
			{
				case("O="):
					return "Organization: ";
				case("OU="):
					return "Organizational Unit: ";
				case("CN="):
					return "Common Name: ";
				case("C="):
					return "Country/Region: ";
				case("S="):
					return "State/province: ";
				case("L="):
					return "City/Location: ";
				default:
					return "";
			}
		}
	}
}

