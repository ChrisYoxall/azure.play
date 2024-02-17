## About

Create an Azure VM using Terraform.

## Notes

To get VM sizes available in a region: az vm list-sizes --location "australiaeast"


Use the AZ CLI to find the image.  Refer https://learn.microsoft.com/en-us/cli/azure/vm/image

    Intereting publishers of images are Canonical, RedHat & Debian.

    Documentation on querying is at https://learn.microsoft.com/en-us/cli/azure/query-azure-cli

    Can get all Canonical image offer names by doing:
    
        az vm image list-offers -l australiaeast -p Canonical --query "[].name"

    An image offer returned from the previous query is '0001-com-ubuntu-server-jammy', to get the SKUs:

        az vm image list-skus -l australiaeast -p Canonical -f 0001-com-ubuntu-server-jammy

    Then get version:

        az vm image list -l australiaeast -p Canonical -f 0001-com-ubuntu-server-jammy -s 22_04-lts-gen2

    NOTE that it can be faster to go through the create process in the portal then select 'download template' on the
    final screen, or to actually do a deploy from portal then copy values from properties.


## SSH


This Terraform example generates a SSH key (view the state). To write to a file: terraform output -raw tls_private_key > vm_id_rsa

To get the public IP from terraform (can also look in Azure portal): terraform output public_ip_address

Before you login will have to change the permissions on the key so not everyone can read it. Then do (azureuser is user set
in the terraform):

    ssh -i ./vm_id_rsa azureuser@4.196.65.150
    



If you want to generate a SSH key yourself in the current directory:

    echo $(pwd)/id_rsa | xargs ssh-keygen -m PEM -t rsa -b 4096 -f

If creating a VM in the portal, paste the public key (id_rsa.pub) into the space provided. Specify the username or will default to 'azureuser'. Log in by doing:

    ssh username@20.213.75.40 -i ./id_rsa
