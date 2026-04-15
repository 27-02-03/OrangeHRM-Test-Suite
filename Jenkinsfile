pipeline {
    agent any

    environment {
        CI = 'true'
        // Store Playwright browsers inside workspace
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
                // Targets the specific project to install binaries
                bat 'dotnet run --project OrangeHRM.Tests/OrangeHRM.Tests.csproj -- playwright install chromium'
            }
        }

        stage('Run Tests') {
            steps {
                // Runs NUnit tests and generates the raw Allure data
                bat 'dotnet test OrangeHRM.Tests/OrangeHRM.Tests.csproj --configuration Release --no-build --logger "trx"'
            }
        }
    }

    post {
        always {
            // Archive standard TRX results
            archiveArtifacts artifacts: '**/TestResults/*.trx', allowEmptyArchive: true

            script {
                // 1. Resolve the path of the 'allure' tool from your Global Tool Configuration
                def allureHome = tool name: 'allure', type: 'org.allurereport.jenkins.tools.AllureCommandlineInstallation'
                
                // 2. Add the tool to the PATH so the plugin finds the .bat file
                withEnv(["PATH+ALLURE=${allureHome}/bin"]) {
                    allure includeProperties: false, 
                           jdk: '', 
                           results: [[path: '**/allure-results']]
                }
            }
        }
        success {
            echo '✅ Tests Passed'
        }
        failure {
            echo '❌ Tests Failed'
        }
    }
}
