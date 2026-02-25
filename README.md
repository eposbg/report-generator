# Install ReportGenerator as a Windows Service

Prerequisites
- Administrator privileges 

Publish the service

1. From a developer command prompt, publish the Windows service project.

   - Framework-dependent publish (smaller output, requires the .NET runtime installed on the host):

     ```powershell
     dotnet publish ReportGenerator.WindowsService -c Release -r win-x64 --self-contained false -o ./publish
     ```
      

2. Register the Windows Service

- Using PowerShell `New-Service`:

  ```powershell
  New-Service -Name "PowerPosition" -BinaryPathName "{path to your build bin folder}\PowerPosition\ReportGenerator.WindowsService.exe" -DisplayName "Power Position Service" -Description "It is used to export intraday reports" -StartupType Automatic
  ```

3. Start and manage the service

- Start the service:

  ```powershell
  Start-Service -Name "PowerPosition"
  ```

- Stop the service:

  ```powershell
  Stop-Service -Name "PowerPosition"
  ```

- Check status:

  ```powershell
  Get-Service -Name "PowerPosition"
  ```


Uninstall the service

- Stop and delete the service (Administrator):

  ```powershell
  Stop-Service -Name "PowerPosition" -Force
  sc delete "PowerPosition"
  ```
