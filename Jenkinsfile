pipeline {
    agent any

    environment {
        CI = 'true'
        // Store Playwright browsers inside workspace (avoids reinstall issues)
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
                bat 'dotnet run --project OrangeHRM.Tests/OrangeHRM.Tests.csproj -- playwright install chromium'
            }
        }

        stage('Run Tests') {
            steps {
                bat '''
                dotnet test OrangeHRM.Tests/OrangeHRM.Tests.csproj ^
                --configuration Release ^
                --no-build ^
                --logger "trx" ^
                --results-directory "OrangeHRM.Tests/bin/Release/net10.0/allure-results"
                '''
            }
        }
    }

    post {
        always {
            // ✅ Archive TRX test results
            archiveArtifacts artifacts: '**/TestResults/*.trx', fingerprint: true

            // ✅ Archive Allure raw results
            archiveArtifacts artifacts: '**/allure-results/**', fingerprint: true

            // ✅ Generate Allure Report (requires plugin)
            script {
                try {
                    allure([
                        includeProperties: false,
                        jdk: '',
                        results: [[path: 'OrangeHRM.Tests/bin/Release/net10.0/allure-results']]
                    ])
                } catch (err) {
                    echo '⚠️ Allure plugin not installed or failed to run.'
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
