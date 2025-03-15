# Azure Play #

Various Azure samples used to learn and practice using Azure features. 

See https://learn.microsoft.com/en-us/dotnet/azure/




## Log Analytics ##

Can do this to get the latest logs from all tables. Very inefficient but useful to see what tables get populated with data:

    search *  
    | top 500 by TimeGenerated// return the latest 500 results

