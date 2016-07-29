REM This is a firewall configuration for accessing your WCF service from remote computer / IoT device in your network.
REM The script is testet under Windows 10. (Maybe be also run under Windows 7)
REM Replace placeholder [The Service Name] with your service name.
REM Replace placeholder [The Used Port] with the port your service is using.

REM Delete rule first, maybe if already exist
netsh advfirewall firewall delete rule name="[The Service Name]" dir=in protocol=TCP localport=[The Used Port]
REM Set the rule
netsh advfirewall firewall add rule name="[The Service Name]" dir=in action=allow protocol=TCP localport=[The Used Port]
