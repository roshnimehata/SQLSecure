/******************************************************************
 * Name: EventMessagesInstaller.cs
 *
 * Description: Event message dll installer function.   Use the 
 * InstallUtil.exe .Net 2.0 Framework utility to install the
 * event messages dll.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace Idera.SQLsecure.Core.Logger
{
    [RunInstaller(true)]
    public class EventMessagesInstaller : Installer
    {
        private EventLogInstaller m_ELI;
        private const int CATEGORY_COUNT = 3;
        private const String EVENT_SOURCE = "Idera.SQLsecure";
        private const String APP_LOG = "Application";
        private const String MESSAGE_FILE = @"\Idera.SQLsecure.Core.EventMessages.dll";

        public EventMessagesInstaller()
        {
            // Create event log installer object.
            m_ELI = new EventLogInstaller();

            // Set the source name of the event log
            m_ELI.Source = EVENT_SOURCE;

            // Set the event log that the source write entries to.
            m_ELI.Log = APP_LOG;

            // Set the message file.
            m_ELI.MessageResourceFile = Directory.GetCurrentDirectory() + MESSAGE_FILE;

            // Set the category file.
            m_ELI.CategoryCount = CATEGORY_COUNT;
            m_ELI.CategoryResourceFile = Directory.GetCurrentDirectory() + MESSAGE_FILE;

            // Add to installer collection.
            Installers.Add(m_ELI);
        }
    }
}
