## About

Example of using a managed identity.  Demonstrates authenticating an application running on a VM to a storage account.

## Notes


To run this demo on managed identities on a VM will need to assign a managed identity to the VM either at creation or afterwards by doing:

    az vm identity assign -g chris-test-rg -n chris-test-vm --identities chris-identity-demo

When running locally it will use your currently loged in AZ CLI account.



The application will read a file out of a blob storage container.  Look at the code for currently configured details. As well as asiging
the managed identity to the VM will need to add it's clientId in the code. The managed identity will need access granted on the storage
account.



To install .NET SDK and ASP.NET Core runtime on VM do: sudo apt-get update && sudo apt-get install -y dotnet-sdk-6.0 aspnetcore-runtime-6.0



To copy the application to the new VM it will be something like (copying a file called archive.tar.gz to /home/azureuser on the VM):

    scp -i ./id_rsa2  ~/Code/dotnet.colsole.app/dotnet.colsole.app/bin/Debug/net6.0/achive.tar.gz azureuser@20.213.93.100:/home/azureuser