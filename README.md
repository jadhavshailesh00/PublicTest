name: My Reusable Workflow

on:
  workflow_call:
    inputs:
      my_array_argument:
        description: 'An array of items'
        required: true
        type: string  # Change the type to string

jobs:
  example_job:
    runs-on: windows-latest
    steps:
      - name: Print array elements
        shell: pwsh
        run: |
          $array = "${{ inputs.my_array_argument }}" -split '\n'  
          $count = 1
          foreach ($line in $array) {
            Write-Host "$count) $line"
            $count++
            
            # Move file to another location
            Move-Item -Path "source_path\$line" -Destination "destination_path\$line"
          }
