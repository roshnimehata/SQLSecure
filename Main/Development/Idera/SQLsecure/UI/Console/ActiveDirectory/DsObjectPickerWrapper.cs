using System;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Interop;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.ActiveDirectory
{
    public class ObjectPickerWrapper
	{
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.ActiveDirectory.ObjectPickerWrapper");

        public string[] AttributeList = new string[] {"sAMAccountName", "objectSid"};

        public ADObject[] ShowObjectPicker(IntPtr parentHandle)
		{
            using (logX.loggerX.InfoCall())
            {
                packLPArray packedAttributeList = null;
                DSOP_SCOPE_INIT_INFO[] scopeInitInfo = new DSOP_SCOPE_INIT_INFO[2];
                DSObjectPicker picker = new DSObjectPicker();
                DSOP_INIT_INFO initInfo = new DSOP_INIT_INFO();
                IDataObject dataObj = null;

                logX.loggerX.Debug("Initialize AD Picker");
                IDsObjectPicker ipicker =
                    Initialize(ref picker, ref packedAttributeList, ref scopeInitInfo, ref initInfo);

                logX.loggerX.Debug("Invoke AD Picker Dialog");
                ipicker.InvokeDialog(parentHandle, out dataObj);

                return ProcessSelections(dataObj);
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        private IDsObjectPicker Initialize(ref DSObjectPicker picker,
            ref packLPArray packedAttributeList,
            ref DSOP_SCOPE_INIT_INFO[] scopeInitInfo,            
            ref DSOP_INIT_INFO initInfo)
		{
            IDsObjectPicker ipicker;
            ipicker = (IDsObjectPicker)picker;
            using (logX.loggerX.InfoCall())
            {
                logX.loggerX.Debug("Initialize 1st search scope");
                // Initialize 1st search scope			
                scopeInitInfo[0].cbSize = (uint) Marshal.SizeOf(typeof (DSOP_SCOPE_INIT_INFO));
                scopeInitInfo[0].flType = DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG;
                scopeInitInfo[0].flScope = DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE |
                                           DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH |
                                           DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS |
                                           DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS;
                scopeInitInfo[0].FilterFlags.Uplevel.flBothModes =
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE;
                scopeInitInfo[0].FilterFlags.flDownlevel = DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS |
                                                           DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS |
                                                           DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS;
                scopeInitInfo[0].pwzADsPath = null;
                scopeInitInfo[0].pwzDcName = null;
                scopeInitInfo[0].hr = 0;

                logX.loggerX.Debug("Initialize 2nd search scope");
                // Initialize 2nd search scope			
                scopeInitInfo[1].cbSize = (uint) Marshal.SizeOf(typeof (DSOP_SCOPE_INIT_INFO));
                scopeInitInfo[1].flType = DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE |
                                          DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE;
                scopeInitInfo[1].flScope = DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_GC |
                                           DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS |
                                           DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS;
                scopeInitInfo[1].FilterFlags.Uplevel.flBothModes =
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
                    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE;
                scopeInitInfo[1].FilterFlags.flDownlevel = DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS |
                                                           DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS |
                                                           DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS;
                scopeInitInfo[1].pwzADsPath = null;
                scopeInitInfo[1].pwzDcName = null;
                scopeInitInfo[1].hr = 0;

                logX.loggerX.Debug("Allocate unmanaged mem for scope searchers");
                // Allocate memory from the unmananged mem of the process, this should be freed later!??
                IntPtr refScopeInitInfo = Marshal.AllocHGlobal
                    (Marshal.SizeOf(typeof (DSOP_SCOPE_INIT_INFO))*2);

                logX.loggerX.Debug("Marshal 1st search scope");
                // Marshal structs to pointers
                Marshal.StructureToPtr(scopeInitInfo[0],
                                       refScopeInitInfo, false);

                logX.loggerX.Debug("Marshal 2nd search scope");
                Marshal.StructureToPtr(scopeInitInfo[1],
                                       (IntPtr) ((int) refScopeInitInfo + Marshal.SizeOf
                                                                              (typeof (DSOP_SCOPE_INIT_INFO))), false);


                logX.loggerX.Debug("Initialize initInfo structure");
                // Initialize structure with data to initialize an object picker dialog box. 
                initInfo.cbSize = (uint) Marshal.SizeOf(initInfo);
                initInfo.pwzTargetComputer = null; // local computer			
                initInfo.cDsScopeInfos = 2;
                initInfo.aDsScopeInfos = refScopeInitInfo;
                // Flags that determine the object picker options. Allow user to select
                // multiple objects and specify that this is not a domain controller
                initInfo.flOptions = /*DSOP_INIT_INFO_FLAGS.DSOP_FLAG_MULTISELECT | */
                    DSOP_INIT_INFO_FLAGS.DSOP_FLAG_SKIP_TARGET_COMPUTER_DC_CHECK;
                //initInfo.flOptions = 0;

                logX.loggerX.Debug("Pack array");
                packedAttributeList = new packLPArray(AttributeList);
                initInfo.cAttributesToFetch = (uint) packedAttributeList.Length;
                initInfo.apwzAttributeNames = packedAttributeList.arrayPtr;

                logX.loggerX.Debug("ipicker.Initialize");
                // Initialize the Object Picker Dialog Box with our options
                ipicker.Initialize(ref initInfo);
            }
		    return ipicker;
		}

		private ADObject[] ProcessSelections(IDataObject dataObj)
		{
			if(dataObj == null)
				return null;
            using (logX.loggerX.InfoCall())
            {
                ADObject[] selections = null;

                // The STGMEDIUM structure is a generalized global memory handle used for data transfer operations
                STGMEDIUM stg = new STGMEDIUM();
                stg.tymed = (uint) TYMED.TYMED_HGLOBAL;
                stg.hGlobal = IntPtr.Zero;
                stg.pUnkForRelease = IntPtr.Zero;

                // The FORMATETC structure is a generalized Clipboard format.
                FORMATETC fe = new FORMATETC();
                fe.cfFormat = (short) System.Windows.Forms.DataFormats.GetFormat("CFSTR_DSOP_DS_SELECTION_LIST").Id;
                //The CFSTR_DSOP_DS_SELECTION_LIST clipboard format is provided by the IDataObject obtained by calling IDsObjectPicker::InvokeDialog
                fe.ptd = IntPtr.Zero;
                fe.dwAspect = (uint) DVASPECT.DVASPECT_CONTENT;
                fe.lindex = -1; // all of the data
                fe.tymed = (uint) TYMED.TYMED_HGLOBAL; //The storage medium is a global memory handle (HGLOBAL)

                dataObj.GetData(ref fe, ref stg);

                IntPtr pDsSL = PInvoke.GlobalLock(stg.hGlobal);

                try
                {
                    // the start of our structure
                    IntPtr current = pDsSL;
                    // get the # of items selected
                    logX.loggerX.Debug("Start Read Int 32");
                    int cnt = (int) Marshal.ReadInt32(current);
                    logX.loggerX.Debug("Finish Read Int 32");

                    // if we selected at least 1 object
                    if (cnt > 0)
                    {
                        selections = new ADObject[cnt];
                        logX.loggerX.Debug("Line: 1");
                        // increment the pointer so we can read the DS_SELECTION structure
                        current = (IntPtr) ((int) current + (Marshal.SizeOf(typeof (uint))*2));
                        logX.loggerX.Debug("Line: 2");
                        // now loop through the structures
                        for (int i = 0; i < cnt; i++)
                        {
                            // marshal the pointer to the structure
                            DS_SELECTION s = (DS_SELECTION) Marshal.PtrToStructure(current, typeof (DS_SELECTION));
                            logX.loggerX.Debug("Line: 3");

                           // Marshal.DestroyStructure(current, typeof (DS_SELECTION));
                            logX.loggerX.Debug("Line: 4");

                            // increment the position of our pointer by the size of the structure
                            current = (IntPtr) ((int) current + Marshal.SizeOf(typeof (DS_SELECTION)));
                            logX.loggerX.Debug("Line: 5");

                            selections[i] = new ADObject();
                            selections[i].Name = s.pwzName;
                            selections[i].AdsPath = s.pwzADsPath;
                            selections[i].UPN = s.pwzUPN;
                            selections[i].ClassName = s.pwzClass;

                            IntPtr ptrFetched = s.pvarFetchedAttributes;
                            logX.loggerX.Debug("Line: 6");

                            VARIANT vr = (VARIANT) Marshal.PtrToStructure(ptrFetched, typeof (VARIANT));
                            logX.loggerX.Debug("Line: 7");

                            string str = Marshal.PtrToStringUni(vr.bstrVal);
                            logX.loggerX.Debug("Line: 8");

                            selections[i].SamAccountName = str;
                            int vt = vr.vt;

                            IntPtr ptrSid = (IntPtr) ((int) ptrFetched + (Marshal.SizeOf(typeof (VARIANT))));
                            logX.loggerX.Debug("Line: 9");

                            vr = (VARIANT) Marshal.PtrToStructure(ptrSid, typeof (VARIANT));
                            logX.loggerX.Debug("Line: 10");

                            IntPtr pSids = IntPtr.Zero;
                            IntPtr pUBound = IntPtr.Zero;
                            int hr = PInvoke.SafeArrayAccessData(vr.pArray, out pSids);
                            logX.loggerX.Debug("Line: 11");

                            hr = PInvoke.SafeArrayGetUBound(vr.pArray, 1, out pUBound);
                            logX.loggerX.Debug("Line: 12");

                            Byte[] sid = new Byte[pUBound.ToInt32() + 1];
                            logX.loggerX.Debug("Line: 13");

                            for (int nBytes = 0; nBytes <= pUBound.ToInt32(); nBytes++)
                            {
                                sid.SetValue(Marshal.ReadByte(pSids, nBytes), nBytes);
                            }
                            logX.loggerX.Debug("Line: 14");

                            selections[i].Sid = new Sid(sid);

                            ////This is how to obtain the Sid manually via lookup instead of by attribute 
                            ////Binding to the User object
                            //DirectoryEntry dr = new DirectoryEntry(selections[i].AdsPath);
                            //dr.RefreshCache();
                            ////To get ObjectSid
                            //selections[i].Sid = new Sid((byte[])dr.Properties["objectSid"].Value);

                            //This will get another string value
                            //ptrNext = (IntPtr)((int)ptrSid + (Marshal.SizeOf(typeof(VARIANT))));
                            //vr = (VARIANT)Marshal.PtrToStructure(ptrNext, typeof(VARIANT));
                            //str = Marshal.PtrToStringUni(vr.bstrVal);

                            //build the SamAccount string with domain\SamAccountName
                            string domain;
                            bool isFlat;
                            if (Path.IsWinntPath(s.pwzADsPath))
                            {
                                // the browser doesn't return a SamAccountName for winnt paths
                                // so process the path for it
                                string samPath;
                                Path.GetSamPathFromWinNTPath(s.pwzADsPath, out samPath);
                                selections[i].SamAccountName = samPath;
                            }
                            else
                            {
                                //Get Domain Name and build it on to the SamAccountName
                                Path.ExtractDomainFromPath(s.pwzADsPath, out domain, out isFlat);
                                DOMAIN_CONTROLLER_INFO dcInfo;
                                if (!isFlat)
                                {
                                    uint nameInFlag = (uint) DsGetDcFlags.DS_IS_DNS_NAME;
                                    // Call DsGetDcName to get the flat domain info.
                                    uint flags = ((uint) DsGetDcFlags.DS_RETURN_FLAT_NAME | nameInFlag);
                                    uint rc = DS.DsGetDcName("", domain, flags, out dcInfo);
                                    if (rc != Win32Errors.ERROR_SUCCESS)
                                    {
                                        logX.loggerX.Error("ERROR - Failed to get flat name DC info for the domain",
                                                           domain);
                                        return null;
                                    }
                                    else
                                    {
                                        domain = dcInfo.DomainName;
                                    }
                                }
                                logX.loggerX.Debug("Line: 100");

                                selections[i].SamAccountName = Path.MakeSamPath(domain, selections[i].SamAccountName);
                            }
                        }
                    }
                }
                finally
                {
                    PInvoke.GlobalUnlock(pDsSL);
                }

                return selections;
            }
		}
	}
}
