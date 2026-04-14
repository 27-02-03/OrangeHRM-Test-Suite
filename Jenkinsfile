pipeline {
    agent any

    environment {
        CI = 'true'
    }

    stages {

        stage('Checkout Code') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/27-02-03/OrangeHRM-Test-Suite.git'
            }
        }

        stage('Restore Dependencies') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build Project') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Install Playwright Browsers') {
            steps {
               bat 'pwsh OrangeHRM.Tests/bin/Release/net10.0/playwright.ps1 install'
            }
        }

        stage('Run Tests') {
            steps {
                bat 'dotnet test --logger "trx"'
            }
        }

        stage('Publish Test Results') {
            steps {
                junit '**/*.trx'
            }
        }
    }

    post {
        always {
            archiveArtifacts artifacts: '**/TestResults/**', fingerprint: true
        }
        success {
            echo '✅ Tests Passed'
        }
        failure {
            echo '❌ Tests Failed'
        }
    }
}
