# PublicTest


Automating the Release and Test Case Execution in GitHub CI
Overview
To improve efficiency and streamline the deployment and testing process, we are automating the release pipeline and test case execution using GitHub CI. This applies only to projects that have been migrated to GitHub.

New Approach
1. Artifact Creation
The release pipeline is manually triggered from GitHub Actions.
Upon execution, the pipeline generates an artifact for deployment.
2. Deployment Process
Deployment is handled through Harness, allowing changes to be deployed to a specific environment as required.
3. Managing Test Cases
A dedicated Manage Test Case Pipeline is required in GitHub.
This pipeline will be manually triggered by passing key environment parameters, such as:
Route URI
User ID
Password
Manage Test Case Pipeline
Workflow Type
Trigger Type: Dispatch Trigger
This allows the pipeline to be executed manually as needed.
Supported Environments
The pipeline will support multiple environments, and the appropriate Route URL will be selected dynamically based on the chosen environment.

QA
Dev
Intg
Required Inputs
To execute test cases, the pipeline will require the following input parameters:

Username – Credentials required for authentication
Password – Secure access to the environment
Route URL – The endpoint for test case execution (URLs will be predefined in the pipeline and selected based on the environment)
CI/CD Pipeline Execution Steps
Step 1: Load Operating System
Ensure the correct OS is available in the runner.
Step 2: Fetch Repository
Clone the GitHub repository and pull the latest changes.
Step 3: Set Up .NET Environment
Install and configure the required .NET version for the project.
Step 4: Set Up Visual Studio
Install necessary Visual Studio components to build and execute tests.
Step 5: Configure Environment Variables
Set the required environment variables in Visual Studio for execution.
Step 6: Load NuGet Packages
Restore all necessary dependencies using NuGet package manager.
Step 7: Build Project/Solution
Compile and build the project to ensure there are no errors before executing test cases.
Step 8: Run Test Cases
Execute test cases with category filtering to run only Manage Test Cases.
This ensures that only relevant tests are executed, improving efficiency.
Benefits of Automation
✅ Reduces Manual Effort – Automates the release and testing process, reducing human intervention.
✅ Consistency & Reliability – Ensures a standardized and repeatable testing and deployment process.
✅ Faster Execution – Speeds up deployments and test execution, enabling quicker feedback.
✅ Improved Traceability – GitHub CI logs provide better visibility and debugging capabilities.

Next Steps
Implement and test the Manage Test Case pipeline.
Validate the integration of GitHub Actions with Harness for deployment.
Optimize test case execution to improve efficiency further.
This version provides more clarity, structure, and detail while keeping it professional and easy to understand. Let me know if you need any further refinements!



name: Manage Test Case Pipeline

on:
  workflow_dispatch:  # Manual trigger
    inputs:
      environment:
        description: 'Select Environment'
        required: true
        type: choice
        options:
          - QA
          - Dev
          - Intg
      username:
        description: 'Username'
        required: true
      password:
        description: 'Password'
        required: true

jobs:
  run-tests:
    runs-on: windows-latest  # Running on Windows OS

    steps:
      - name: Checkout Repository
        uses: actions/checkout@v4  # Fetch the latest code

      - name: Set Up .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'  # Adjust based on project requirements

      - name: Set Up Visual Studio
        uses: microsoft/setup-msbuild@v1  # Install MSBuild for project compilation

      - name: Set Environment Variables
        run: |
          echo "Setting environment variables..."
          echo "ENVIRONMENT=${{ github.event.inputs.environment }}" >> $GITHUB_ENV
          echo "USERNAME=${{ github.event.inputs.username }}" >> $GITHUB_ENV
          echo "PASSWORD=${{ github.event.inputs.password }}" >> $GITHUB_ENV

          # Dynamically set Route URL based on the selected environment
          if [ "${{ github.event.inputs.environment }}" == "QA" ]; then
            echo "ROUTE_URL=https://qa.example.com/api" >> $GITHUB_ENV
          elif [ "${{ github.event.inputs.environment }}" == "Dev" ]; then
            echo "ROUTE_URL=https://dev.example.com/api" >> $GITHUB_ENV
          elif [ "${{ github.event.inputs.environment }}" == "Intg" ]; then
            echo "ROUTE_URL=https://intg.example.com/api" >> $GITHUB_ENV
          fi

      - name: Restore NuGet Packages
        run: dotnet restore

      - name: Build Solution
        run: dotnet build --configuration Release --no-restore

      - name: Run Test Cases (Filter: Manage Test Cases)
        run: dotnet test --filter "Category=ManageTestCases"






Search