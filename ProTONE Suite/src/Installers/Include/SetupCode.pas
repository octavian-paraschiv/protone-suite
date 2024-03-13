//--------------------------------------------------------------------------------
// Various constants required to detect isntallation of various features
const
   // .NET Framework detection parameters
   DotNetFxDownloadUrl =        'https://go.microsoft.com/fwlink?LinkID=863265';
   
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

//--------------------------------------------------------------------------------

var
  DownloadPage: TDownloadWizardPage;

function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if Progress = ProgressMax then
    Log(Format('Successfully downloaded file to {tmp}: %s', [FileName]));
  Result := True;
end;

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
function VerifyApplications(isInstall: Boolean) : Integer;
var
   RunningAppsFileName, ExeFileName: String;
   RunningApps: AnsiString;   
   ResultCode: integer;

begin

  if (isInstall) then
  begin
    ExeFileName := ExpandConstant('{tmp}') + '\OPMedia.Utility.exe';
    RunningAppsFileName := ExpandConstant('{tmp}') + '\OPMedia.RunningApps.res';
  end
  else
  begin
    ExeFileName := ExpandConstant('{app}') + '\OPMedia.AudioWorker.exe';
    RunningAppsFileName := ExpandConstant('{app}') + '\OPMedia.RunningApps.res';
  end;

  Exec(ExeFileName, '-killall -redirectToFile', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);

  if ( LoadStringFromFile(RunningAppsFileName, RunningApps) and ( Length(RunningApps) > 0 ) ) then 
  begin
    result := MsgBox(FmtMessage(CustomMessage('AppsStillRunning'), [ Chr(13) + RunningApps + Chr(13) ]), mbError, MB_RETRYCANCEL);
    exit;
  end;

  DeleteFile(RunningAppsFileName);
  result := IDOK;

end;

//--------------------------------------------------------------------------------
function AreApplicationsStopped(isInstall: Boolean) : Boolean;
var
   res: integer;
begin

  if (isInstall) then
    ExtractTemporaryFile('OPMedia.Utility.exe');
       
  res := VerifyApplications(isInstall);
  while(res = IDRETRY) do
  begin
    res := VerifyApplications(isInstall);
  end;

  if (res = IDCANCEL) then
  begin
    result := false;
    exit;
  end;

  result := true;
end;


//--------------------------------------------------------------------------------
// Installs .NET Framework
function InstallDotNETFx: boolean;
var
   ResultCode : integer;
begin
    Exec(ExpandConstant('{tmp}\dotnet_install.exe'), '/q', '', SW_SHOW, ewWaitUntilTerminated, ResultCode);
    DeleteFile(ExpandConstant('{tmp}\dotnet_install.exe'))

    if (IsDotNetInstalled(net472, 0) = false) then
    begin
      MsgBox(CustomMessage('DotNetInstallFailedLong'), mbCriticalError, MB_OK);
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
   DownloadPage: TDownloadWizardPage;
   success: boolean;
   xx: string;
begin

   DependencyPage := CreateOutputProgressPage(
    CustomMessage('InstallDependencies'),
    CustomMessage('WaitInstallingDependencies'));

   DependencyPage.Show;

   DependencyPage.SetProgress(1, 4);
   DependencyPage.SetText(CustomMessage('DotNetInstalling'), '');

   // Detect and install .NET Framework if not present
   if (IsDotNetInstalled(net472, 0) = false) then
   begin

    // .NET Framework was not detected.
    res := MsgBox(CustomMessage('DotNetRequired'), mbConfirmation, MB_YESNO);
    if (res = IDNO) then 
    begin
      result := CustomMessage('installAborted');
      exit;
    end;

    DownloadPage := CreateDownloadPage(
      CustomMessage('DotNetDownload'),
      CustomMessage('DotNetDownloadWait'), 
      @OnDownloadProgress);

    DownloadPage.Clear;

    DownloadPage.Add(DotNetFxDownloadUrl, 'dotnet_install.exe', '');
    DownloadPage.Show;

    try
      DownloadPage.Download;
      DependencyPage.SetProgress(2, 4);
    except
      MsgBox(AddPeriod(GetExceptionMessage), mbCriticalError, MB_OK);
      success := False;
    finally
      DownloadPage.Hide;
    end;

    if (success = False) then
    begin
      result := CustomMessage('DotNetInstallFailed');
    end;

    success := InstallDotNETFx;
    if (success = False) then
    begin
      result := CustomMessage('DotNetInstallFailed');
      exit;
    end;
  end;

  DependencyPage.SetProgress(3, 4);
  DependencyPage.SetText(CustomMessage('DependenciesInstallSuccess'), '');
  DependencyPage.SetProgress(2, 2);

  result := '';

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
// EVENT FUNCTIONS
//--------------------------------------------------------------------------------
function PrepareToInstall(var needsRestart : Boolean) : String;
begin
   result := CheckPrerequisites();
end;

//--------------------------------------------------------------------------------
// Setup initialisation
function InitializeSetup: Boolean;
var
   res : integer;
begin
  if (AreApplicationsStopped(true) = false) then
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
   if (AreApplicationsStopped(false) = false) then
   begin
    result := false;
    exit;
   end;

   result := true;

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
    Exec(s, '', '', SW_SHOW, ewNoWait, rc);
   end;
end;
