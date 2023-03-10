# Create a new directory to store the downloaded file
New-Item -ItemType Directory -Path "C:\Downloads"

# Download a file from a URL and save it to the new directory
Invoke-WebRequest "https://example.com/file.zip" -OutFile "C:\Downloads\file.zip"

# Unzip the downloaded file to a new directory
Expand-Archive "C:\Downloads\file.zip" -DestinationPath "C:\UnzippedFiles"

# Rename a file in the unzipped directory
Rename-Item "C:\UnzippedFiles\oldfilename.txt" -NewName "newfilename.txt"

# Print the contents of the renamed file to the console
Get-Content "C:\UnzippedFiles\newfilename.txt"
