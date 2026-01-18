output "writer_connection_string" {
  value     = azurerm_servicebus_topic_authorization_rule.demo_topic_writer_rule.primary_connection_string
  sensitive = true
}
