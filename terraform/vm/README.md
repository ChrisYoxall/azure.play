

To get VM sizes available in a region: az vm list-sizes --location "australiaeast"



This Terraform example generates a SSH key (view the state). To write to a file: terraform output -raw tls_private_key > id_rsa

To get the public IP from terraform (can just look in Azure portal): terraform output public_ip_address



If you want to generate a SSH key yourself in the current directory:

    echo $(pwd)/id_rsa | xargs ssh-keygen -m PEM -t rsa -b 4096 -f

If creating a VM in the portal, paste the public key (id_rsa.pub) into the space provided. Specify the username or will default to 'azureuser'. Log in by doing:

    ssh username@20.213.75.40 -i ./id_rsa
