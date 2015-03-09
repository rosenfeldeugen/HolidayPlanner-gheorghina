$serviceName = "RedisMaster";
$redisInitialConfigFile = '.\initial.redis.master.conf';
$redisConfigFile = '.\redis.master.conf';
$serviceIP = "127.0.0.1";
$servicePort = "6379";
$serviceMaxHeap = "50mb";
$InstallationCommand = "i"; 

$config = Get-Content $redisInitialConfigFile; 
$config | Foreach-Object {
	$_  -replace '#serviceIP', $serviceIP `
		-replace '#servicePort', $servicePort `
		-replace '#serviceMaxHeap', $serviceMaxHeap 
			}  | Set-Content $redisConfigFile

if ($InstallationCommand -eq "i") 
{
	"Installing the service."
	.\redis-server.exe --service-install $redisConfigFile --service-name $serviceName

	"'$serviceName' was installed."

	"Starting the service."
	.\redis-server.exe --service-start $redisConfigFile --service-name $serviceName 
}
else
{
	"Stopping '$serviceName'"
	.\redis-server.exe --service-stop $redisConfigFile --service-name $serviceName 

	"Uninstalling '$serviceName'"
	.\redis-server.exe --service-uninstall $redisConfigFile --service-name $serviceName  
}

"Completed."