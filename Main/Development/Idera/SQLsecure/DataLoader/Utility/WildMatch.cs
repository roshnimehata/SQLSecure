/******************************************************************
 * Name: WildMatch.cs
 *
 * Description: Wild card matching class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections;

namespace Idera.SQLsecure.Collector.Utility
{
	internal enum TokenType
	{
		TOKEN_ALL_END,       // L"*" at end
		TOKEN_ONE,           // L"?"
		TOKEN_ONE_END,       // L"?" at end
		TOKEN_ONE_FLT,       // L"*?"
		TOKEN_ONE_FLT_END,   // L"*?" at end
		TOKEN_NUM,           // L"#"
		TOKEN_NUM_END,       // L"#" at end
		TOKEN_NUM_FLT,       // L"*#"
		TOKEN_NUM_FLT_END,   // L"*#" at end
		TOKEN_ALP,           // L"alphabetic"
		TOKEN_ALP_END,       // L"alphabetic" at end
		TOKEN_ALP_FLT,       // L"*alphabetic"
		TOKEN_ALP_FLT_END,   // L"*alphabetic" at end
	} ;

	/// <summary>
	/// Summary description for WildMatch.
	/// </summary>
	public class WildMatch
	{
		#region Member Variables

		private string _match ;
		private bool _compiled ;
		private bool _all ;
		private ArrayList _tokens ;

		#endregion

		#region Properties

        public string MatchString
        {
            get { return _match; }
        }

		#endregion

		#region Construction/Destruction

		public WildMatch()
		{
			_match = "" ;
			_all = false ;
			_compiled = false ;
			_tokens = new ArrayList();
		}

		public WildMatch(string sMatchString) : this()
		{
			Compile(sMatchString) ;
		}

		#endregion

		private void Erase()
		{
			_tokens.Clear() ;
			_compiled = false ;
			_all = false ;
			_match = "" ;
		}

		public bool Compile(string matchString)
		{
			bool bFloat = false ;
			int tokenStart = 0 ;

			Erase() ;
            if (matchString != null) { _match = matchString; }

            if (string.IsNullOrEmpty(_match))
            {
                _all = true;
            }
            else
            {
                for (int i = 0; i < _match.Length; i++)
                {
                    char currentChar, nextChar;
                    bool bLastChar;

                    currentChar = _match[i];
                    if (_match.Length - 1 == i)
                    {
                        bLastChar = true;
                        nextChar = '\0';
                    }
                    else
                    {
                        bLastChar = false;
                        nextChar = _match[i + 1];
                    }

                    TokenType tokenType;

                    switch (currentChar)
                    {
                        case '*':
                            if (bLastChar)
                            {
                                if (_tokens.Count == 0)
                                    _all = true;
                                else
                                    TokenAppend(TokenType.TOKEN_ALL_END, "");
                            }
                            bFloat = true;
                            tokenStart = i + 1;
                            break;
                        case '?':
                            if (!bLastChar)
                                tokenType = bFloat ? TokenType.TOKEN_ONE_FLT : TokenType.TOKEN_ONE;
                            else
                                tokenType = bFloat ? TokenType.TOKEN_ONE_FLT_END : TokenType.TOKEN_ONE_END;
                            TokenAppend(tokenType, "");
                            tokenStart = i + 1;
                            bFloat = false;
                            break;
                        case '#':
                            if (!bLastChar)
                                tokenType = bFloat ? TokenType.TOKEN_NUM_FLT : TokenType.TOKEN_NUM;
                            else
                                tokenType = bFloat ? TokenType.TOKEN_NUM_FLT_END : TokenType.TOKEN_NUM_END;
                            TokenAppend(tokenType, "");
                            bFloat = false;
                            tokenStart = i + 1;
                            break;
                        default:
                            if (!bLastChar)
                            {
                                if (nextChar == '*' || nextChar == '?' || nextChar == '#')
                                {
                                    tokenType = bFloat ? TokenType.TOKEN_ALP_FLT : TokenType.TOKEN_ALP;
                                    TokenAppend(tokenType, _match.Substring(tokenStart, i - tokenStart + 1));
                                    tokenStart = i + 1;
                                    bFloat = false;
                                }
                            }
                            else
                            {
                                tokenType = bFloat ? TokenType.TOKEN_ALP_FLT_END : TokenType.TOKEN_ALP_END;
                                TokenAppend(tokenType, _match.Substring(tokenStart, i - tokenStart + 1));
                                tokenStart = i + 1;
                                bFloat = false;
                            }
                            break;
                    }
                }
            }
			_compiled = true ;
			return true ;
		}

		public bool Match(string subject)
		{
			if(subject == null)
				throw new ArgumentNullException("subject") ;

			if(!_compiled)
				return false ;
			if(_all)
				return true ;
			if(_tokens.Count == 0)
			{
				if(subject.Length == 0)
					return true ;
				else
					return false ;
			}

			bool retVal = false ;
			bool bDone = false ;
			int indexSubject = 0 ;
			int indexSubjectAlt = -1 ;
			int itemAltIndex = -1;
			int itemIndex ;

			for(itemIndex = 0 ; itemIndex < _tokens.Count && !bDone ; itemIndex++)
			{
				switch(((TokenItem)_tokens[itemIndex]).TokenType)
				{
					case TokenType.TOKEN_ALL_END: // "*" at end
						bDone = true ;
						retVal = true ;
						break ;
					case TokenType.TOKEN_ONE: // "?"
						if(indexSubject < subject.Length)
							indexSubject++ ;
						else
							bDone = true ;
						break ;
					case TokenType.TOKEN_ONE_END: // "?" at end
						if((indexSubject + 1) == subject.Length)
						{
							retVal = true ;
							bDone = true ;
						}
						else if((indexSubject < subject.Length) && (itemAltIndex != -1))
						{
							// alternation
							itemIndex = itemAltIndex - 1 ;
							indexSubject = indexSubjectAlt + 1 ;
						}
						else
							bDone = true ;
						break ;
					case TokenType.TOKEN_ONE_FLT: // "*?"
						if(indexSubject < subject.Length)
						{
							itemAltIndex = itemIndex ;
							indexSubjectAlt = indexSubject ;
							indexSubject++ ;
						}else
							bDone = true ;
						break ;
					case TokenType.TOKEN_ONE_FLT_END: // "*?" at end
						if(indexSubject < subject.Length)
							retVal = true ;
						bDone = true ;
						break ;
					case TokenType.TOKEN_NUM: // "#"
						if(indexSubject < subject.Length)
						{
							char thisChar = subject[indexSubject] ;
							if(thisChar == '#' || Char.IsDigit(thisChar))
								indexSubject++ ;
							else if(itemAltIndex != -1)
							{
								// Alternation
								itemIndex = itemAltIndex - 1 ;
								indexSubject = indexSubjectAlt + 1 ;
							}else
								bDone = true ;
						}else
							bDone = true ;
						break ;
					case TokenType.TOKEN_NUM_END: // "#" at end
						if((indexSubject + 1) == subject.Length)
						{
							char thisChar = subject[indexSubject] ;
							if(thisChar == '#' || Char.IsDigit(thisChar))
							{
								retVal = true ;
								bDone = true ;
							}
						}
						else if((indexSubject < subject.Length) && (itemAltIndex != -1))
						{
							// Alternation
							itemIndex = itemAltIndex - 1 ;
							indexSubject = indexSubjectAlt + 1 ;
						}else
							bDone = true ;
						break ;
					case TokenType.TOKEN_NUM_FLT: // "*#"
						while(indexSubject < subject.Length)
						{
							char thisChar = subject[indexSubject] ;
							if(thisChar == '#' || Char.IsDigit(thisChar))
								break ;
							else
								indexSubject++ ;
						}
						if(indexSubject < subject.Length)
						{
							itemAltIndex = itemIndex ;
							indexSubjectAlt = indexSubject ;
							indexSubject++ ;
						}
						else
							bDone = true ;
						break ;
					case TokenType.TOKEN_NUM_FLT_END:  // "*#" at end
						if(indexSubject < subject.Length)
						{
							char lastChar = subject[subject.Length - 1] ;
							if(lastChar == '#' || Char.IsDigit(lastChar))
								retVal = true ;
						}
						bDone = true ;
						break ;
					case TokenType.TOKEN_ALP: // alpha
						if((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) <= subject.Length)
						{
							if(String.Compare(((TokenItem)_tokens[itemIndex]).Token, 0, subject, indexSubject, 
								((TokenItem)_tokens[itemIndex]).Length, true) == 0)
							{
								indexSubject += ((TokenItem)_tokens[itemIndex]).Length ;
							}
							else if(itemAltIndex != -1)
							{
								itemIndex = itemAltIndex - 1 ;
								indexSubject = indexSubjectAlt + 1 ;
							}else
								bDone = true ;
						}else
							bDone = true ;
						break ;
					case TokenType.TOKEN_ALP_END: // alpha at end
						if((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) == subject.Length)
						{
							if(String.Compare(((TokenItem)_tokens[itemIndex]).Token, 0, subject, indexSubject,
								((TokenItem)_tokens[itemIndex]).Length, true) == 0)
							{
								retVal = true ;
								bDone = true ;
							}
						}
						else if(((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) <= subject.Length) &&
							(itemAltIndex != -1))
						{
							itemIndex = itemAltIndex - 1 ;
							indexSubject = indexSubjectAlt + 1 ;
						}
						else
							bDone = true ;
						break ;
					case TokenType.TOKEN_ALP_FLT: // *alpha
						while((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) <= subject.Length)
						{
							if(String.Compare(((TokenItem)_tokens[itemIndex]).Token, 0, subject, indexSubject,
								((TokenItem)_tokens[itemIndex]).Length, true) == 0)
								break ;
							else
								indexSubject++ ;
						}
						if((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) <= subject.Length)
						{
							itemAltIndex = itemIndex ;
							indexSubjectAlt = indexSubject ;
							indexSubject += ((TokenItem)_tokens[itemIndex]).Length ;
						}else
							bDone = true ;

						break ;
					case TokenType.TOKEN_ALP_FLT_END: // *alpha at end
						if((indexSubject + ((TokenItem)_tokens[itemIndex]).Length) <= subject.Length)
						{
							if(String.Compare(((TokenItem)_tokens[itemIndex]).Token, 0, subject, 
                        subject.Length - ((TokenItem)_tokens[itemIndex]).Length, ((TokenItem)_tokens[itemIndex]).Length, true) == 0)
								retVal = true ;
						}
						bDone = true ;
						break ;
					default: // abort match
						bDone = true ;
						break ;
				}
			}
			if(!bDone)
				return (indexSubject == subject.Length) ;
			return retVal ;
		}

		private void TokenAppend(TokenType tokenType, string tokenStr)
		{
			TokenItem item = new TokenItem() ;

			item.TokenType = tokenType ;
			item.Token = tokenStr ;
			_tokens.Add(item) ;
		}
	}

	internal class TokenItem
	{
		private TokenType _type ;
		private string _token ;

		public TokenType TokenType
		{
			get { return _type ; }
			set { _type = value ; }
		}

		public int Length
		{
			get { return _token.Length ; }
		}

		public string Token
		{
			get { return _token ; }
			set 
			{
				if(value == null)
					_token = "" ;
				else
					_token = value ;
			}
		}
	}
}
