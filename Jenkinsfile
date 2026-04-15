pipeline {
    agent any

    environment {
        CI = 'true'
        // Forces Playwright to store browsers in the workspace to avoid permission issues
        PLAYWRIGHT_BROWSERS_PATH = "${WORKSPACE}/pw-browsers"
    }

    stages {
        stage('Checkout Code') {
            steps {
                git branch: 'main', 
                    url: 'https://github.com/27-02-03/OrangeHRM-Test-Suite.git'
            }
        }

        stage('Restore & Build') {
            steps {
                bat 'dotnet restore'
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Install Playwright Browsers') {
            steps {
                // Installs the necessary browser binaries on the Jenkins Agent
                bat 'dotnet run --project OrangeHRM.Tests/OrangeHRM.Tests.csproj -- playwright install chromium'
            }
        }

        stage('Run Tests') {
            steps {
                // Runs tests and outputs TRX and Allure results
                // We use '|| exit 0' if you want the pipeline to continue even if tests fail
                bat 'dotnet test OrangeHRM.Tests/OrangeHRM.Tests.csproj --configuration Release --no-build --logger "trx"'
            }
        }
    }

    post {
        always {
            // 1. Archive standard TRX results for the Jenkins 'Test Result Trend'
            archiveArtifacts artifacts: '**/TestResults/*.trx', allowEmptyArchive: true

            script {
                try {
                    // 2. Resolve the path to the 'allure' tool defined in Global Tool Configuration
                    // This fixes the "Can't find allure commandline" error
                    def allureHome = tool name: 'allure', type: 'org.allurereport.jenkins.tools.AllureCommandlineInstallation'
                    
                    // 3. Manually add the Allure bin folder to the environment PATH for this block
                    withEnv(["PATH+ALLURE=${allureHome}/bin"]) {
                        allure includeProperties: false, 
                               jdk: '', 
                               results: [[path: '**/allure-results']]
                    }
                } catch (Exception e) {
                    echo "Allure Reporting failed. Root cause: ${e.message}"
                    echo "Ensure Java is installed on the Jenkins Agent and the Tool Name matches 'allure'."
                }
            }
        }
        success {
            echo 'Tests Passed'
        }
        failure {
            echo 'Tests Failed'
        }
    }
}
