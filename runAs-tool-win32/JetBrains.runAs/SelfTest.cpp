#include "stdafx.h"
#include "SelfTest.h"
#include "Trace.h"
#include "ExitCode.h"
#include "Settings.h"
#include "SecurityManager.h"
#include "ErrorUtilities.h"

Result<ExitCode> SelfTest::Run(const Settings& settings) const
{
	Trace trace(settings.GetLogLevel());
	Handle processToken(L"Process token");
	if (!::OpenProcessToken(GetCurrentProcess(), TOKEN_ALL_ACCESS, &processToken))
	{
		return Result<ExitCode>(ErrorUtilities::GetErrorCode(), ErrorUtilities::GetLastErrorMessage(L"OpenProcessToken"));
	}

	auto hasLogonSIDResult = HasLogonSID(trace, processToken);
	if (hasLogonSIDResult.HasError())
	{
		return Result<ExitCode>(hasLogonSIDResult.GetErrorCode(), hasLogonSIDResult.GetErrorDescription());
	}

	auto hasAdministrativePrivilegesResult = HasAdministrativePrivileges(trace);
	if (hasAdministrativePrivilegesResult.HasError())
	{
		return Result<ExitCode>(hasAdministrativePrivilegesResult.GetErrorCode(), hasAdministrativePrivilegesResult.GetErrorDescription());
	}

	auto hasSeAssignPrimaryTokenPrivilegeResult = HasSeAssignPrimaryTokenPrivilege(trace, processToken);
	if (hasSeAssignPrimaryTokenPrivilegeResult.HasError())
	{
		return Result<ExitCode>(hasSeAssignPrimaryTokenPrivilegeResult.GetErrorCode(), hasSeAssignPrimaryTokenPrivilegeResult.GetErrorDescription());
	}

	if (!hasLogonSIDResult.GetResultValue())
	{		
		// Windows service
		if (!hasAdministrativePrivilegesResult.GetResultValue())
		{
			return EXIT_CODE_NO_ADMIN;
		}

		if (!hasSeAssignPrimaryTokenPrivilegeResult.GetResultValue())
		{
			return EXIT_CODE_NO_ASSIGN_PRIMARY_TOKEN_PRIV;
		}
	}
	
	return Is64OS() ? 64 : 32;
}

Result<bool> SelfTest::HasLogonSID(Trace& trace, const Handle& token) const
{	
	auto tokenGroupsResult = _securityManager.GetTokenGroups(trace, token);
	if (tokenGroupsResult.HasError())
	{
		return Result<bool>(tokenGroupsResult.GetErrorCode(), tokenGroupsResult.GetErrorDescription());
	}

	trace < L"SelfTest::HasLogonSID - Loop through the groups to find the logon SID.";
	auto hasLogonSID = false;	
	auto tokenGroups = tokenGroupsResult.GetResultValue();
	for (auto groupsIterrator = tokenGroups.begin(); groupsIterrator != tokenGroups.end(); ++groupsIterrator)
	{
		if ((groupsIterrator->Attributes & SE_GROUP_LOGON_ID) == SE_GROUP_LOGON_ID)
		{
			hasLogonSID = true;
			break;
		}
	}

	return hasLogonSID;
}

Result<bool> SelfTest::HasAdministrativePrivileges(Trace& trace) const
{
	return IsUserAnAdmin() == TRUE;
}

Result<bool> SelfTest::HasSeAssignPrimaryTokenPrivilege(Trace& trace, const Handle& token) const
{
	auto tokenGroupsResult = _securityManager.GetPrivilegies(trace, token);
	if (tokenGroupsResult.HasError())
	{
		return Result<bool>(tokenGroupsResult.GetErrorCode(), tokenGroupsResult.GetErrorDescription());
	}

	return true;
}

typedef BOOL(WINAPI *LPFN_ISWOW64PROCESS) (HANDLE, PBOOL);
bool SelfTest::IsWow64()
{
	int bIsWow64 = false;
	LPFN_ISWOW64PROCESS fnIsWow64Process;
	fnIsWow64Process = reinterpret_cast<LPFN_ISWOW64PROCESS>(GetProcAddress(GetModuleHandle(TEXT("kernel32")), "IsWow64Process"));
	if (NULL != fnIsWow64Process)
	{
		if (!fnIsWow64Process(GetCurrentProcess(), &bIsWow64))
		{
			return false;
		}
	}

	return bIsWow64 != 0;
}

bool SelfTest::Is64OS()
{
#if defined(_M_X64) || defined(x86_64)
	return true;
#else
	return IsWow64() == true;
#endif
}