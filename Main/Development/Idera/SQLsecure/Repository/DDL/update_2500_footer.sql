-- provide view access to new tables

GRANT SELECT ON dbo.[assessment] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessment] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessmentnotes] TO SQLSecureView
GRANT SELECT ON dbo.[policyassessmentdetail] TO SQLSecureView
GRANT SELECT ON dbo.[policychangelog] TO SQLSecureView
GO


declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver,2500) >= 2000)
BEGIN
	-- update the server version security check default settings with the latest versions and service packs
	if exists (select * from policymetric where policyid = 0 and assessmentid=0 and metricid = 2 and severityvalues = '''8.00.2187'',''9.00.3054''')
	begin
		update policymetric 
			set severityvalues= '''8.00.2039'',''9.00.4035'',''10.0.1600'''
			where policyid = 0 and assessmentid=0 and metricid = 2
	end

	-- add new security checks
	if not exists (select * from metric where metricid = 62)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (62, 'SQL Logins Not Using Password Expiration', 'Login', 0, 0, '', 'When enabled, this check will identify a risk if any SQL Login is not protected by password expiration.',
										'Determine whether password expiration is enabled for all SQL Logins')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, 62, 1, 2, '', '',
										'Is password expiration enabled for all SQL Logins?')
	end

	if not exists (select * from metric where metricid = 63)
	begin
		insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
						values (63, 'Server is Domain Controller', 'Configuration', 0, 0, '', 'When enabled, this check will identify a risk if the Server is a primary or backup domain controller.',
										'Determine whether the Server is a domain controller')
		insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
						values (0, 0, 63, 1, 1, '', '',
										'Is the server a Domain Controller?')
	end

	-- update the All Servers policy with the new security checks so they are enabled correctly if this is a new install
	if not exists (select * from policymetric where policyid = 1 and assessmentid=1 and metricid in (62, 63))
		insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
			select 1, 1, m.metricid, m.isenabled, m.reportkey, m.reporttext, m.severity, m.severityvalues
				from policymetric m
				where m.policyid = 0
					and m.assessmentid = 0
					and m.metricid in (62, 63)

	-- now add the new security checks to all existing policies, but disable it by default so it won't interfere with current settings
	insert into policymetric (policyid, assessmentid, metricid, isenabled, reportkey, reporttext, severity, severityvalues)
		select a.policyid, a.assessmentid, m.metricid, 0, m.reportkey, m.reporttext, m.severity, m.severityvalues
			from policymetric m, assessment a
			where m.policyid = 0
				and m.assessmentid = 0
				and m.metricid in (62, 63)
				and a.policyid > 0
				-- this check makes it restartable
				and a.assessmentid not in (select assessmentid from policymetric where metricid in (62, 63))
END

-- these have been deferred out of this release because of issues 
--
---- add comparison security checks that are not server specific
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (64, 'Security Check Settings are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the security check settings are different from the settings of the selected assessment.',
--                                'Determine whether the security check settings are different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 64, 0, 3, '', '',
--                                'Are the security check settings different?')
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (65, 'Assessment Findings are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the assessment findings are different from the findings of the selected assessment.',
--                                'Determine whether the results of this assessment are different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 65, 0, 3, '', '',
--                                'Are the assessment findings different?')
--
--insert into metric (metricid, metricname, metrictype, isuserentered, ismultiselect, validvalues, valuedescription, metricdescription)
--                values (66, 'Policy Servers are Different', 'Data Integrity', 0, 1, '''Current Assessment'' : ''c'',''Last Draft Assessment'' : ''d'', ''Last Published Assessment'' : ''p'', ''Last Approved Assessment'' : ''a''', 'When enabled, this check will identify a risk if the servers used for this assessment are diffferent from the servers used for the selected assessment.',
--                                'Determine whether the list of servers is different from the selected assessment.')
--insert into policymetric (policyid, assessmentid, metricid, isenabled, severity, severityvalues, reportkey, reporttext)
--                values (0, 0, 66, 0, 3, '', '',
--                                'Are the policy servers different?')



GO
