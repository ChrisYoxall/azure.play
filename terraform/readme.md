
for_each Notes

A couple of things to remember in Terraform:

- You can only use for_each over a set of strings or a map. Refer https://developer.hashicorp.com/terraform/language/meta-arguments/for_each
- You can't perform nested iterations with a for_each in Terraform, you must first flatten your data structure.

This means you often have to manipulate any data structure you might have. For example consider a map called backend_pools (that can be
used to create a load balancer) that has a key for each backend pool to be created with the value being a list of objects containing a VM
name and its associated NIC ID. For example:

    {
        "backend01" = [
            { vm_name = "vm01", nic_id = "aaa" },
            { vm_name = "vm02", nic_id = "bbb" }
        ],
        "backend02" = [
            { vm_name = "vm03", nic_id = "ccc" },
            { vm_name = "vm04", nic_id = "ddd" }
        ]
    }

To iterate over this map to create the azurerm_network_interface_backend_address_pool_association we need a map where each value contains the
name of a backend pool and the NIC ID to put in that pool. Lets say we want to maek this: 

    {
        "backend01.vm01" = { pool_name = "backend01", nic_id = "aaa" },
        "backend01.vm02" = { pool_name = "backend01", nic_id = "bbb" },
        "backend02.vm03" = { pool_name = "backend02", nic_id = "ccc" },
        "backend02.vm04" = { pool_name = "backend02", nic_id = "ddd" }
    }

The key is made up of the name of the pool and the vm name. This is not used but maps must have a unique key.

A 'for' expression allows for the transformation of complex types. Refer https://developer.hashicorp.com/terraform/language/expressions/for

Can convert the backend_pools map to the new structure by doing:

    vm_pools_list = flatten([
    for pool_key, pool_value in var.backend_pools : [
        for vm in pool_value : { pool_name = pool_key, vm_name = vm.vm_name, nic_id = vm.nic_id }
    ]])

    vm_pool_map = { for vm_pool in local.vm_pools_list : "${vm_pool.vm_name}.${vm_pool.pool_name}" => vm_pool }

Some other handy functions to use (there are probably others):

- Flatten: https://developer.hashicorp.com/terraform/language/functions/flatten
- Merge: https://developer.hashicorp.com/terraform/language/functions/merge


Note that I find it useful to do this transformation in a locals block so you can output the variable to confirm the transform has produced
the datastructure you need. Usefull when debugging.
