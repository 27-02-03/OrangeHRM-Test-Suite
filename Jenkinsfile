pipeline {
    agent any

    environment {
        CI = 'true'
        // Forces Playwright to download browsers to a specific location in the workspace
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
                // We use the direct DLL or dotnet run to ensure browsers are on the build agent
                bat 'dotnet run --project OrangeHRM.Tests/OrangeHRM.Tests.csproj -- playwright install chromium'
            }
        }

        stage('Run Tests') {
            steps {
                // Ensure results go to the folder Allure expects
                // The --no-build flag saves time since we built in the previous stage
                bat 'dotnet test OrangeHRM.Tests/OrangeHRM.Tests.csproj --configuration Release --no-build --logger "trx"'
            }
        }
    }

    post {
        always {
            // 1. Archive TRX results
            archiveArtifacts artifacts: '**/TestResults/*.trx', fingerprint: true
    
            // 2. Archive Allure raw results (good practice)
            archiveArtifacts artifacts: '**/allure-results/**', fingerprint: true
    
            // 3. Generate Allure Report (safe execution)
            script {
                try {
                    allure([
                        includeProperties: false,
                        jdk: '',
                        results: [[path: '**/allure-results']]
                    ])
                } catch (err) {
                    echo '⚠️ Allure plugin not installed or failed to run.'
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
