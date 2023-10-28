
module "storage_account" {

  source = "./modules/storage_account"

  config = {
    yoxalltfdemosa01 = {
      location               = "Australia East"
      rgname                 = "sa-demo-one-rg"
      hierarchical_namespace = true
      sftp                   = true
      containers = [
        {
          container_name        = "firstcontainer"
          container_access_type = "private"
        },
        {
          container_name        = "secondcontainer"
          container_access_type = "private"
        }
      ]
    },
    yoxalltfdemosa02 = {
      location = "Australia East"
      rgname   = "sa-demo-two-rg"
      containers = [
        {
          container_name        = "thirdcontainer"
          container_access_type = "private"
        },
        {
          container_name        = "fourthcontainer"
          container_access_type = "private"
        }
      ]
    }
  }

}