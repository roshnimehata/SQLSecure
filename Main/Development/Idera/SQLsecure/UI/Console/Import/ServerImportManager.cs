﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Data;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Import.Models;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.SQL;
using Idera.SQLsecure.UI.Console.Utility;


namespace Idera.SQLsecure.UI.Console.Import
{
    public class ServerImportManager
    {


        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Import.ServerImportManager");
        public static bool ImportItem(ImportItem itemToImport, Repository repository)
        {
            return ImportItem(itemToImport, repository, ImportSettings.Default);
        }


        public static bool ImportItem(ImportItem itemToImport, Repository repository, ImportSettings settings)
        {
            using (logX.loggerX.InfoCall())
            {


                string sqlServerVersion, machine, instance, connectionName, edition;
                var errorList = new List<string>();

                if (itemToImport.HasErrors())
                {
                    settings.ChangeStatus(ImportStatusIcon.Warning,
                        string.Format("Skipped due to errors: {0}", itemToImport.GetErrors()));
                    return false;
                }
                settings.ChangeStatus(ImportStatusIcon.Importing, "Importing");


                var serverNameParts = itemToImport.ServerName.Split(',');
                var serverPort = serverNameParts.Length > 1 ? Convert.ToInt32(serverNameParts[1]) : (int?)null;
                ScheduleJob.ScheduleData scheduleData = new ScheduleJob.ScheduleData();

                //Validation of instance connection credentials
                OperationResult<bool> operationResult =
                    ValidateCredentials(itemToImport, out sqlServerVersion, out machine, out instance,
                        out connectionName, out edition);

                if (!operationResult.Value)
                {
                    settings.ChangeStatus(ImportStatusIcon.Error, string.Format("Not imported. {0}", operationResult.GetEventAllMessagesString()));
                    return false; //Skip importing of server
                }

                // SQLsecure 3.1 (Anshul) - Validate server edition based on server type.
                // SQLsecure 3.1 (Anshul) - SQLSECU-1775 - Azure VM : Register a Server as Azure DB by selecting Server type as Azure VM.
                if (!SqlHelper.ValidateServerEdition(itemToImport.ServerType, edition))
                {
                    settings.ChangeStatus(ImportStatusIcon.Error, string.Format("Not imported. {0}",
                        itemToImport.ServerType == ServerType.AzureSQLDatabase ? ErrorMsgs.IncorrectServerTypeAzureSQLDBImportMsg :
                    ErrorMsgs.IncorrectServerTypeSQLServerImportMsg));
                    return false; //Skip importing of server
                }

                //Validation of server acces credentials (using WindowsCredentials)
                //Add operation error if not successful but continue import operation
                operationResult = CheckServerAccess(itemToImport, machine);
                if (!operationResult.Value)
                {
                    errorList.AddRange(operationResult.GetEventMessagesStringList());
                };

                var parsedSqlServerVersion = SqlHelper.ParseVersion(sqlServerVersion);
                ServerInfo serverInfo = new ServerInfo(parsedSqlServerVersion, (itemToImport.AuthType == SqlServerAuthenticationType.WindowsAuthentication || itemToImport.AuthType == SqlServerAuthenticationType.AzureADAuthentication),
                itemToImport.UserName, itemToImport.Password, connectionName, SqlServer.GetValueByName(itemToImport.ServerType));
                try
                {
                    if (repository.RegisteredServers.Find(connectionName) != null)
                    {
                        UpdateCredentials(itemToImport, repository, connectionName);
                        if (errorList.Count > 0)
                            settings.ChangeStatus(ImportStatusIcon.Warning, string.Format("Updated with warnings: {0}", string.Join("\n", errorList.ToArray())));
                        else
                            settings.ChangeStatus(ImportStatusIcon.Imported, "Updated");
                        return true;
                    }
                    string serverType = itemToImport.ServerType == ServerType.OnPremise ? "OP" : (itemToImport.ServerType == ServerType.SQLServerOnAzureVM ? "AVM" : "ADB");
                    RegisteredServer.AddServer(repository.ConnectionString,
                        connectionName, serverPort, machine, instance,
                        (itemToImport.AuthType == SqlServerAuthenticationType.WindowsAuthentication || itemToImport.AuthType == SqlServerAuthenticationType.AzureADAuthentication) ? "W" : "S",
                        itemToImport.UserName, itemToImport.Password,
                        itemToImport.UseSameCredentials ? itemToImport.UserName : itemToImport.WindowsUserName,
                        itemToImport.UseSameCredentials
                            ? itemToImport.Password
                            : itemToImport.WindowsUserPassword,
                        sqlServerVersion, 0, new string[0], serverType);
                    repository.RefreshRegisteredServers();
                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);

                    logX.loggerX.Error(ex.Message);
                    return false;
                }
                var server = repository.RegisteredServers.Find(connectionName);

                //Add email notification
                try
                {
                    int providerId = Program.gController.Repository.NotificationProvider.ProviderId;
                    RegisteredServerNotification rSN =
                        new RegisteredServerNotification(SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry());
                    rSN.RegisteredServerId = server.RegisteredServerId;
                    rSN.ProviderId = providerId;
                    rSN.Recipients = string.Empty;
                    rSN.SnapshotStatus = RegisteredServerNotification.SnapshotStatusNotification_Never;
                    rSN.PolicyMetricSeverity = (int)RegisteredServerNotification.MetricSeverityNotificaiton.Never;
                    rSN.AddNotificationProvider(repository.ConnectionString);

                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);
                }
                // Add rules to the repository.
                try
                {
                    AddRulesToRepository(repository, instance, serverInfo);
                }
                catch (Exception ex)
                {

                    errorList.Add(ex.Message);
                }

                // Add job to repository
                try
                {
                    AddJobToRepository(repository, scheduleData, connectionName);
                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);
                }

                try
                {

                    AddServerToTags(server.RegisteredServerId);
                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);
                }


                var errorMessage = string.Join("\n", errorList.ToArray());
                if (string.IsNullOrEmpty(errorMessage))
                    settings.ChangeStatus(ImportStatusIcon.Imported, "Imported");
                else
                    settings.ChangeStatus(ImportStatusIcon.Warning, string.Format("Imported with warnings: {0}", errorMessage));


                return true;
            }
        }

        private static void AddServerToTags(int serverId)
        {

            try
            {
                List<string> tagsIds = new List<string>();

                TagWorker.AssignServerToTags(serverId, tagsIds);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(ex.Message);
                throw;
            }

        }

        private static void UpdateCredentials(ImportItem itemToImport, Repository repository, string connectionName)
        {
            RegisteredServer.UpdateCredentials(repository.ConnectionString,
                connectionName, itemToImport.UserName, itemToImport.Password,
                (itemToImport.AuthType == SqlServerAuthenticationType.WindowsAuthentication || itemToImport.AuthType == SqlServerAuthenticationType.AzureADAuthentication) ? "W" : "S" ,
                itemToImport.UseSameCredentials ? itemToImport.UserName : itemToImport.WindowsUserName,
                itemToImport.UseSameCredentials
                    ? itemToImport.Password
                    : itemToImport.WindowsUserPassword);
        }

        private static void AddRulesToRepository(Repository repository, string instance, ServerInfo serverInfo)
        {
            try
            {
                var filter = new DataCollectionFilter(instance, "Default rule",
                    "Rule created when the server was imported");

                var filterSelector = new FilterSelection();
                filterSelector.Initialize(filter, serverInfo);
                filterSelector.GetFilter(out filter);
                filter.CreateFilter(repository.ConnectionString, serverInfo.connectionName);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(ex.Message);
                throw;
            }

        }

        private static void AddJobToRepository(Repository repository, ScheduleJob.ScheduleData scheduleData, string connectionName)
        {
            try
            {
                scheduleData.SetDefaults();
                Guid jobID = ScheduleJob.AddJob(repository.ConnectionString,
                    connectionName,
                    repository.Instance, scheduleData, false);

                // Update Registered Server with new jobID 
                repository.GetServer(connectionName).SetJobId(jobID);
            }
            catch (Exception ex)
            {
                logX.loggerX.Error(ex.Message);
                throw;
            }
        }


        private static OperationResult<bool> CheckServerAccess(ImportItem importItem, string machine)
        {
            var result = new OperationResult<bool> { Value = true };
            string errorMsg;
            using (logX.loggerX.InfoCall())
            {
                try
                {
                    if (importItem.ServerType != ServerType.AzureSQLDatabase)
                    {
                        var winUser = importItem.UseSameCredentials ? importItem.UserName : importItem.WindowsUserName;
                        var winUserPassword = importItem.UseSameCredentials
                            ? importItem.Password
                            : importItem.WindowsUserPassword;
                        if (string.IsNullOrEmpty(winUser) && string.IsNullOrEmpty(winUserPassword))
                        {
                            result.Value = false;
                            result.AddErrorEvent(ErrorMsgs.WindowsUserForImportNotSpecifiedMsg);

                        }
                        else
                        {
                            Core.Accounts.Server.ServerAccess sa;

                            // SQLSecure 3.1 (Biresh Kumar Mishra) - Fix server import for Azure VM
                            if ((SqlServer.GetValueByName(importItem.ServerType) == Utility.Activity.TypeServerOnPremise)
                                 || (SqlServer.GetValueByName(importItem.ServerType) == Utility.Activity.TypeServerAzureVM))
                            {
                                sa = Core.Accounts.Server.CheckServerAccess(machine, winUser, winUserPassword, out errorMsg);

                                if (sa != Core.Accounts.Server.ServerAccess.OK)
                                {
                                    if ((!string.IsNullOrEmpty(importItem.ServerName)) && (SqlServer.GetValueByName(importItem.ServerType) == Utility.Activity.TypeServerAzureVM))
                                    {
                                        string tempMachine = string.Empty;

                                        if (importItem.ServerName.IndexOf(@"\") != -1)
                                        {
                                            tempMachine = importItem.ServerName.Substring(0, importItem.ServerName.IndexOf(@"\"));
                                        }
                                        else
                                        {
                                            tempMachine = importItem.ServerName;
                                        }

                                        Core.Accounts.Server.ServerAccess tempSa = sa;
                                        sa = Core.Accounts.Server.ServerAccess.ERROR_OTHER;
                                        string tempErrorMsg = errorMsg;
                                        errorMsg = string.Empty;
                                        sa = Core.Accounts.Server.CheckServerAccess(tempMachine, winUser, winUserPassword, out errorMsg);

                                        if (sa != Core.Accounts.Server.ServerAccess.OK)
                                        {
                                            sa = tempSa;
                                            errorMsg = tempErrorMsg;
                                        }
                                        else
                                        {
                                            machine = tempMachine;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                sa = Core.Accounts.Server.CheckAzureServerAccess(importItem.ServerName, winUser, winUserPassword, out errorMsg, true);

                            }
                            if (!string.IsNullOrEmpty(errorMsg)) result.AddWarningEvent(errorMsg);
                            result.Value = (sa == Core.Accounts.Server.ServerAccess.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Value = false;
                    result.AddErrorEvent(ex.Message);
                    logX.loggerX.Error(ex.Message);

                }
                return result;
            }
        }
        private static WindowsImpersonationContext Impersonate(string userName, string password)
        {
            using (logX.loggerX.InfoCall())
            {
                try
                {
                    WindowsIdentity wi =
                        Impersonation.GetCurrentIdentity(userName, password);
                    return wi.Impersonate();
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(ex.Message);
                    return null;
                }
            }
        }


        private static void UndoImpersonation(WindowsImpersonationContext context)
        {
            using (logX.loggerX.InfoCall())
            {
                if (context != null)
                {
                    context.Undo();
                    context.Dispose();
                }
            }
        }



        private static OperationResult<bool> ValidateCredentials(ImportItem importItem, out string sqlServerVersion, out string machine,
            out string instance, out string fullName, out string edition)
        {
            var result = new OperationResult<bool> { Value = true };
            using (logX.loggerX.InfoCall())
            {
                WindowsImpersonationContext impersonationContext = null;
                string login = string.Empty;
                string password = string.Empty;
                try
                {
                    if (importItem.AuthType != SqlServerAuthenticationType.WindowsAuthentication)
                    {
                        login = importItem.UserName;
                        password = importItem.Password;
                    }
                    else
                        impersonationContext = Impersonate(importItem.UserName, importItem.Password);
                    bool azureADAuth = importItem.AuthType == SqlServerAuthenticationType.AzureADAuthentication ? true : false;

                    SqlServer.GetSqlServerProperties(importItem.ServerName, login, password,
                        out sqlServerVersion, out machine, out instance, out fullName, out edition, SqlServer.GetValueByName(importItem.ServerType), azureADAuth);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(ex.Message);
                    result.Value = false;
                    result.AddErrorEvent(ex.Message);

                    sqlServerVersion = edition = string.Empty;
                    machine = Path.GetComputerFromSQLServerInstance(importItem.ServerName);
                    instance = Path.GetInstanceFromSQLServerInstance(importItem.ServerName);
                    fullName = importItem.ServerName.Trim().ToUpperInvariant();
                    return result;
                }
                finally
                {
                    if (importItem.AuthType == SqlServerAuthenticationType.WindowsAuthentication)
                    {
                        UndoImpersonation(impersonationContext);
                    }
                }
                return result;
            }
        }
    }
}
