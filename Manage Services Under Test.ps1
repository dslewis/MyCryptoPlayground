Clear-Host
Write-Host "What key should we start with?"
$key = Read-Host
$time = "15000"
$servicePath = "C:\Users\dslewis\Source\Repos\cryptocurrency\CryptoObserver\CoinBaseLedgerManager\bin\Debug\CoinBaseLedgerManager.exe"
 Invoke-Command -ScriptBlock {param($sp) C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /install "$sp"} -Args $servicePath
 try{
 sc.exe start LedgerManager $key $time
 Write-Host "Your service has started.  Enter Any key to stop and uninstall it."
 Read-Host
 Stop-Service LedgerManager
 Invoke-Command -ScriptBlock {param($sp) C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /uninstall "$sp"} -Args $servicePath
 }
 Catch [system.exception] {
    $error | fl * -f
 }