//--------------------------------------------------------------------------------
// Various constants required to detect isntallation of various features
const
   // .NET Framework detection parameters
   DotNetFxRegistryPath =       'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full';
   DotNetFxDownloadUrl =        'http://download.microsoft.com/download/C/3/A/C3A5200B-D33C-47E9-9D70-2F7C65DAAD94/NDP46-KB3045557-x86-x64-AllOS-ENU.exe';
   
   // Haali Media Splitter detection parameter
   // This is the CLSID of splitter.ax filter
   HaaliRegistryPath =          'SOFTWARE\Classes\CLSID\{55DA30FC-F16B-49FC-BAA5-AE59FC65F82D}\InprocServer32';

   // This means to read default value. It should return the installed location of splitter.ax.
   HaaliRegistryValue =         '';

   // FFDShow Media Codecs detection parameter
   // This is the CLSID of ffdshow.ax filter
   FFDShowRegistryPath =        'SOFTWARE\Classes\CLSID\{4DB2B5D9-4556-4340-B189-AD20110D953F}\InprocServer32';
   // This means to read default value. It should return the installed location of ffdshow.ax.
   FFDShowRegistryValue =       ''; 

   // Application Names
   PlayerAppShortName =         'opmedia.protone';
   MediaLibraryAppShortName =   'opmedia.medialibrary';
   
   // Application Long Names
   PlayerAppName =              'ProTONE Player';
   MediaLibraryAppName =        'ProTONE Media Library';
   
//--------------------------------------------------------------------------------
// External functions

// Importing ShowWindow Windows API from User32.DLL
function ShowWindow(hWnd: Integer; uType: Integer): Integer;
external 'ShowWindow@user32.dll stdcall';

function isxdl_Download(hWnd: Integer; URL, Filename: String): Integer;
external 'isxdl_Download@files:isxdl.dll stdcall';

procedure isxdl_AddFile(URL, Filename: String);
external 'isxdl_AddFile@files:isxdl.dll stdcall';

procedure isxdl_AddFileSize(URL, Filename: String; Size: Cardinal);
external 'isxdl_AddFileSize@files:isxdl.dll stdcall';

function isxdl_DownloadFiles(hWnd: Integer): Integer;
external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';

procedure isxdl_ClearFiles;
external 'isxdl_ClearFiles@files:isxdl.dll stdcall';

function isxdl_IsConnected: Integer;
external 'isxdl_IsConnected@files:isxdl.dll stdcall';

function isxdl_SetOption(Option, Value: String): Integer;
external 'isxdl_SetOption@files:isxdl.dll stdcall';

function isxdl_GetFileName(URL: String): String;
external 'isxdl_GetFileName@files:isxdl.dll stdcall';


//--------------------------------------------------------------------------------
// Checks whether the feature specified via its reg path & reg value
// is associated with a valid (existing) file path
function IsFeatureInstalled(regPath, regVal: String): boolean;
var
   success: boolean;
   instDir: string;

begin
    success := RegQueryStringValue(HKLM, regPath, regVal, instDir);
    result := success and (FileExists(instDir));
end;

//--------------------------------------------------------------------------------
// Checks whether FFDShw codecs require installation
function FFDShowIsMissing: boolean;
begin
   result := not IsFeatureInstalled(FFDShowRegistryPath, FFDShowRegistryValue);
end;

//--------------------------------------------------------------------------------
// Checks whether HAALI Media splitter requires installation
function HaaliIsMissing: boolean;
begin
   result := not IsFeatureInstalled(HaaliRegistryPath, HaaliRegistryValue);
end;

//--------------------------------------------------------------------------------
// Checks whether any media codec requires installation
function CodecsAreMissing: boolean;
begin
   result := FFDShowIsMissing OR HaaliIsMissing;
end;

//--------------------------------------------------------------------------------
procedure DecodeVersion( verstr: String; var verint: array of Integer );
var
  i,p: Integer; s: string;
begin
  // initialize array
  verint := [0,0,0,0];
  i := 0;
  while ( (Length(verstr) > 0) and (i < 4) ) do
  begin
   p := pos('.', verstr);
   if p > 0 then
   begin
      if p = 1 then s:= '0' else s:= Copy( verstr, 1, p - 1 );
     verint[i] := StrToInt(s);
     i := i + 1;
     verstr := Copy( verstr, p+1, Length(verstr));
   end
   else
   begin
     verint[i] := StrToInt( verstr );
     verstr := '';
   end;
  end;

end;

//--------------------------------------------------------------------------------
// This function compares version string
// return -1 if ver1 < ver2
// return  0 if ver1 = ver2
// return  1 if ver1 > ver2
function CompareVersion( ver1, ver2: String ) : Integer;
var
  verint1, verint2: array of Integer;
  i: integer;
begin

  SetArrayLength( verint1, 4 );
  DecodeVersion( ver1, verint1 );

  SetArrayLength( verint2, 4 );
  DecodeVersion( ver2, verint2 );

  Result := 0; i := 0;
  while ( (Result = 0) and ( i < 4 ) ) do
  begin
   if verint1[i] > verint2[i] then
     Result := 1
   else
      if verint1[i] < verint2[i] then
       Result := -1
     else
       Result := 0;

   i := i + 1;
  end;

end;

//--------------------------------------------------------------------------------
function DownloadFile(sLabel, sDesc, sUrl, sDest : string ) : Integer;
begin
   isxdl_SetOption('label', sLabel);
   isxdl_SetOption('description', sDesc);
   isxdl_AddFile(sUrl, sDest);
   result := isxdl_DownloadFiles(StrToInt(ExpandConstant('{wizardhwnd}')));
end;

//--------------------------------------------------------------------------------
// Indicates whether .NET Framework 4.6 is installed.
function IsDotNETFxDetected: boolean;
var
    success: boolean;
    install: cardinal;
    instVersion: string;

begin
    success := RegQueryDWordValue(HKLM, DotNetFxRegistryPath, 'Install', install);
    if (success and (install = 1)) then
    begin
        success := RegQueryStringValue(HKLM, DotNetFxRegistryPath, 'Version', instVersion);
        if (success) then
        begin
            result := (CompareVersion(instVersion, '4.6') >= 0);
            exit;
        end;
    end;
    result := false;
end;

//--------------------------------------------------------------------------------
// Installs .NET Framework
function InstallDotNETFx: boolean;
var
   ResultCode : integer;
   wicPresent : boolean;
begin

   DownloadFile('Download .NET 4.6 installation ...', 'Please wait while downloading Microsoft .NET Framework 4.6 ...', 
       DotNetFxDownloadUrl, ExpandConstant('{tmp}\dotnet46.exe'));

   Exec(ExpandConstant('{tmp}\dotnet46.exe'), '/q', '', SW_SHOW, ewWaitUntilTerminated, ResultCode)
   
   DeleteFile(ExpandConstant('{tmp}\dotnet46.exe'))

   if (IsDotNETFxDetected = false) then
   begin
    MsgBox(CustomMessage('DotNetInstallFailedLong'), mbCriticalError, MB_OK);
    result := false;
    exit;
   end;

   result := true;
end;

//---------------------------------------------------------------------------------
procedure StopPersistenceService;
var
   ResultCode: Integer;
begin
   Exec('cmd.exe', '/c "sc stop OPMedia.PersistenceService"', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
   Sleep(750);
end;

//--------------------------------------------------------------------------------
function IsApplicationRunning(appName : String) : Boolean;
begin
   result := CheckForMutexes(appName + '.mutex');
end;

//--------------------------------------------------------------------------------
function VerifyApplications() : Integer;
var
   playerRunning : Boolean;
   libraryRunning : Boolean;
   warningMessage : String;
   res: integer;

begin
   playerRunning := IsApplicationRunning(PlayerAppShortName);
   libraryRunning := IsApplicationRunning(MediaLibraryAppShortName);

   if (playerRunning or libraryRunning) then
   begin

    warningMessage := Chr(13);
    if (playerRunning) then
    begin
        warningMessage := warningMessage + PlayerAppName + Chr(13);
    end;

    if (libraryRunning) then
    begin
        warningMessage := warningMessage + MediaLibraryAppName  + Chr(13);
    end;

    result := MsgBox(FmtMessage(CustomMessage('AppsStillRunning'), [ warningMessage ]), mbError, MB_RETRYCANCEL);

    exit;
   end;

   result := IDOK;

end;

//--------------------------------------------------------------------------------
function AreApplicationsStopped() : Boolean;
var
   res: integer;
begin


   res := VerifyApplications();
   while(res = IDRETRY) do
   begin
    res := VerifyApplications();
   end;

   if (res = IDCANCEL) then
   begin
    result := false;
    exit;
   end;

   result := true;

end;

//---------------------------------------------------------------------------------
// Verifies install preconditions
function CheckPrerequisites: String;
var
   res: integer;
   DependencyPage: TOutputProgressWizardPage;

begin

   DependencyPage := CreateOutputProgressPage(
    CustomMessage('InstallDependencies'),
    CustomMessage('WaitInstallingDependencies'));

   DependencyPage.Show;

   DependencyPage.SetText(CustomMessage('PersistenceServiceCheck'), '');
   StopPersistenceService;

   DependencyPage.SetText(CustomMessage('DotNetInstalling'), '');
   DependencyPage.SetProgress(0, 2);

   // Detect and install .NET Framework if not present
   if (IsDotNETFxDetected = false) then
   begin
   
    // .NET Framework was not detected.
    res := MsgBox(CustomMessage('DotNetRequired'), mbConfirmation, MB_YESNO);

    if ((res = IDNO) or (InstallDotNETFx = false)) then
    begin
        result := CustomMessage('DotNetInstallFailed');
        exit;
    end;
   end;

   DependencyPage.SetText(CustomMessage('DependenciesInstallSuccess'), '');
   DependencyPage.SetProgress(2, 2);

   result := '';

end;

//--------------------------------------------------------------------------------
// Setup initialisation
function InitializeSetup: Boolean;
var
   res : integer;
begin
  
   if (AreApplicationsStopped = false) then
   begin
    result := false;
    exit;
   end;

   result := true;

end;

//--------------------------------------------------------------------------------
// Setup initialisation
function InitializeUninstall: Boolean;
var
   res : integer;
begin
   if (AreApplicationsStopped = false) then
   begin
    result := false;
    exit;
   end;

   result := true;

end;

//--------------------------------------------------------------------------------
// Validates setup preconditions
function PrepareToInstall(var needsRestart : Boolean) : String;
begin
   result := CheckPrerequisites();
end;

//--------------------------------------------------------------------------------
// Allows for standard command line parsing assuming a key/value organization
function GetCommandlineParam (inParam: String):String;
var
  LoopVar : Integer;
  BreakLoop : Boolean;
begin
  // Init the variable to known values
  LoopVar :=0;
  Result := '';
  BreakLoop := False;

  // Loop through the passed in arry to find the parameter
  while ( (LoopVar < ParamCount) and
          (not BreakLoop) ) do
  begin
    // Determine if the looked for parameter is the next value
    if ( (ParamStr(LoopVar) = inParam) and
         ( (LoopVar+1) <= ParamCount )) then
    begin
      // Set the return result equal to the next command line parameter
      Result := ParamStr(LoopVar+1);

      // Break the loop }
      BreakLoop := True;
    end;

    //{ Increment the loop variable
    LoopVar := LoopVar + 1;
  end;
end;

//--------------------------------------------------------------------------------
procedure DeinitializeSetup();
var
   s : string;
   rc : integer;
begin
   
   s := GetCommandlineParam('/APPRESTART');
   if (Length(s) > 0) then
   begin
    //MsgBox(s, mbInformation, MB_OK);
    Exec(s, '', '', SW_SHOW, ewNoWait, rc);
   end;
end;